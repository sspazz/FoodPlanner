using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using FoodPlanner.Models;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace FoodPlanner.Repositories
{
    public class MealPlanRepository : IMealPlanRepository
    {
        private readonly string _connectionString;

        public MealPlanRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public IDbConnection Connection => new SqlConnection(_connectionString);

        public async Task<IEnumerable<MealPlan>> GetAllAsync()
        {
            using (var db = Connection)
            {
                var sql = "SELECT * FROM MealPlans";
                return await db.QueryAsync<MealPlan>(sql);
            }
        }

        public async Task<MealPlan> GetByIdAsync(int id)
        {
            using (var db = Connection)
            {
                var sql = "SELECT * FROM MealPlans WHERE Id = @Id";
                return await db.QueryFirstOrDefaultAsync<MealPlan>(sql, new { Id = id });
            }
        }

        public async Task<int> AddAsync(MealPlan mealPlan)
        {
            using (var db = Connection)
            {
                var sql = @"
                    INSERT INTO MealPlans (Name, Date)
                    VALUES (@Name, @Date);
                    SELECT CAST(SCOPE_IDENTITY() as int)";
                var mealPlanId = await db.QuerySingleAsync<int>(sql, new { mealPlan.Name, mealPlan.Date });

                foreach (var recipe in mealPlan.Recipes)
                {
                    recipe.MealPlanId = mealPlanId;
                    await db.ExecuteAsync(
                        "INSERT INTO MealPlanRecipes (MealPlanId, RecipeId) VALUES (@MealPlanId, @RecipeId)",
                        recipe);
                }

                return mealPlanId;
            }
        }

        public async Task UpdateAsync(MealPlan mealPlan)
        {
            using (var db = Connection)
            {
                var sql = @"
                    UPDATE MealPlans
                    SET Name = @Name, Date = @Date
                    WHERE Id = @Id";
                await db.ExecuteAsync(sql, new { mealPlan.Name, mealPlan.Date, mealPlan.Id });

                foreach (var recipe in mealPlan.Recipes)
                {
                    await db.ExecuteAsync(
                        "UPDATE MealPlanRecipes SET RecipeId = @RecipeId WHERE MealPlanId = @MealPlanId",
                        recipe);
                }
            }
        }

        public async Task DeleteAsync(int id)
        {
            using (var db = Connection)
            {
                await db.ExecuteAsync("DELETE FROM MealPlanRecipes WHERE MealPlanId = @Id", new { Id = id });
                await db.ExecuteAsync("DELETE FROM MealPlans WHERE Id = @Id", new { Id = id });
            }
        }

        public async Task<IEnumerable<MealPlan>> GetMealPlansWithRecipesAsync()
        {
            using (var db = Connection)
            {
                var mealPlanDictionary = new Dictionary<int, MealPlan>();

                var sql = @"
            SELECT mp.Id, mp.Name, mp.Date, 
                   mpr.MealPlanId, mpr.RecipeId, 
                   r.Id as RecipeId, r.Name as RecipeName, r.Description as RecipeDescription
            FROM MealPlans mp
            LEFT JOIN MealPlanRecipes mpr ON mp.Id = mpr.MealPlanId
            LEFT JOIN Recipes r ON mpr.RecipeId = r.Id";

                var mealPlans = await db.QueryAsync<MealPlan, MealPlanRecipe, MealPlan>(
                    sql,
                    (mealPlan, mealPlanRecipe) =>
                    {
                        if (!mealPlanDictionary.TryGetValue(mealPlan.Id, out var currentMealPlan))
                        {
                            currentMealPlan = mealPlan;
                            currentMealPlan.Recipes = new List<MealPlanRecipe>();
                            mealPlanDictionary.Add(currentMealPlan.Id, currentMealPlan);
                        }

                        if (mealPlanRecipe != null)
                        {
                            mealPlanRecipe.RecipeName = mealPlanRecipe.RecipeName; // Ensure RecipeName is set correctly
                            mealPlanRecipe.RecipeDescription = mealPlanRecipe.RecipeDescription; // Ensure RecipeDescription is set correctly
                            currentMealPlan.Recipes.Add(mealPlanRecipe);
                        }

                        return currentMealPlan;
                    },
                    splitOn: "MealPlanId,RecipeId");

                return mealPlans.Distinct().ToList();
            }
        }

    }
}
