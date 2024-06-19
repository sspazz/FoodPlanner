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
    public class RecipeRepository : IRecipeRepository
    {
        private readonly string _connectionString;

        public RecipeRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public IDbConnection Connection => new SqlConnection(_connectionString);

        public async Task<IEnumerable<Recipe>> GetAllAsync()
        {
            using (var db = Connection)
            {
                var sql = "SELECT * FROM Recipes";
                return await db.QueryAsync<Recipe>(sql);
            }
        }

        public async Task<Recipe> GetByIdAsync(int id)
        {
            using (var db = Connection)
            {
                var sql = "SELECT * FROM Recipes WHERE Id = @Id";
                return await db.QueryFirstOrDefaultAsync<Recipe>(sql, new { Id = id });
            }
        }

        public async Task<int> AddAsync(Recipe recipe)
        {
            using (var db = Connection)
            {
                var sql = @"
                    INSERT INTO Recipes (Name, Description)
                    VALUES (@Name, @Description);
                    SELECT CAST(SCOPE_IDENTITY() as int)";
                var recipeId = await db.QuerySingleAsync<int>(sql, new { recipe.Name, recipe.Description });

                foreach (var ingredient in recipe.Ingredients)
                {
                    ingredient.RecipeId = recipeId;
                    await db.ExecuteAsync(
                        "INSERT INTO RecipeIngredients (RecipeId, IngredientId, Quantity) VALUES (@RecipeId, @IngredientId, @Quantity)",
                        ingredient);
                }

                return recipeId;
            }
        }

        public async Task UpdateAsync(Recipe recipe)
        {
            using (var db = Connection)
            {
                var sql = @"
                    UPDATE Recipes
                    SET Name = @Name, Description = @Description
                    WHERE Id = @Id";
                await db.ExecuteAsync(sql, new { recipe.Name, recipe.Description, recipe.Id });

                foreach (var ingredient in recipe.Ingredients)
                {
                    await db.ExecuteAsync(
                        "UPDATE RecipeIngredients SET Quantity = @Quantity WHERE RecipeId = @RecipeId AND IngredientId = @IngredientId",
                        ingredient);
                }
            }
        }

        public async Task DeleteAsync(int id)
        {
            using (var db = Connection)
            {
                await db.ExecuteAsync("DELETE FROM RecipeIngredients WHERE RecipeId = @Id", new { Id = id });
                await db.ExecuteAsync("DELETE FROM Recipes WHERE Id = @Id", new { Id = id });
            }
        }

        public async Task<IEnumerable<Recipe>> GetRecipesWithIngredientsAsync()
        {
            using (var db = Connection)
            {
                var recipeDictionary = new Dictionary<int, Recipe>();

                var sql = @"
            SELECT r.Id AS RecipeId, r.Name AS RecipeName, r.Description, 
                   ri.RecipeId, ri.IngredientId, ri.Quantity, 
                   i.Id AS IngredientId, i.Name AS IngredientName
            FROM Recipes r
            LEFT JOIN RecipeIngredients ri ON r.Id = ri.RecipeId
            LEFT JOIN Ingredients i ON ri.IngredientId = i.Id";

                var recipes = await db.QueryAsync<Recipe, RecipeIngredient, Ingredient, Recipe>(
                    sql,
                    (recipe, recipeIngredient, ingredient) =>
                    {
                        if (!recipeDictionary.TryGetValue(recipe.Id, out var currentRecipe))
                        {
                            currentRecipe = recipe;
                            currentRecipe.Ingredients = new List<RecipeIngredient>();
                            recipeDictionary.Add(currentRecipe.Id, currentRecipe);
                        }

                        if (recipeIngredient != null)
                        {
                            currentRecipe.Ingredients.Add(recipeIngredient);
                        }

                        return currentRecipe;
                    },
                    splitOn: "RecipeId,IngredientId");

                return recipes.Distinct().ToList();
            }
        }
    }
}
