using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using FoodPlanner.Models;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace FoodPlanner.Repositories
{
    public class IngredientRepository : IIngredientRepository
    {
        private readonly string _connectionString;

        public IngredientRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public IDbConnection Connection => new SqlConnection(_connectionString);

        public async Task<IEnumerable<Ingredient>> GetAllAsync()
        {
            using (var db = Connection)
            {
                var sql = "SELECT * FROM Ingredients";
                return await db.QueryAsync<Ingredient>(sql);
            }
        }

        public async Task<Ingredient> GetByIdAsync(int id)
        {
            using (var db = Connection)
            {
                var sql = "SELECT * FROM Ingredients WHERE Id = @Id";
                return await db.QueryFirstOrDefaultAsync<Ingredient>(sql, new { Id = id });
            }
        }

        public async Task<int> AddAsync(Ingredient ingredient)
        {
            using (var db = Connection)
            {
                var sql = @"
                    INSERT INTO Ingredients (Name)
                    VALUES (@Name);
                    SELECT CAST(SCOPE_IDENTITY() as int)";
                return await db.QuerySingleAsync<int>(sql, new { ingredient.Name });
            }
        }

        public async Task UpdateAsync(Ingredient ingredient)
        {
            using (var db = Connection)
            {
                var sql = @"
                    UPDATE Ingredients
                    SET Name = @Name
                    WHERE Id = @Id";
                await db.ExecuteAsync(sql, new { ingredient.Name, ingredient.Id });
            }
        }

        public async Task DeleteAsync(int id)
        {
            using (var db = Connection)
            {
                await db.ExecuteAsync("DELETE FROM RecipeIngredients WHERE IngredientId = @Id", new { Id = id });
                await db.ExecuteAsync("DELETE FROM Ingredients WHERE Id = @Id", new { Id = id });
            }
        }
    }
}
