using FoodPlanner.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FoodPlanner.Services
{
    public interface IRecipeService
    {
        Task<IEnumerable<Recipe>> GetRecipesAsync();
        Task<Recipe> GetRecipeByIdAsync(int id);
        Task<int> AddRecipeAsync(Recipe recipe);
        Task UpdateRecipeAsync(Recipe recipe);
        Task DeleteRecipeAsync(int id);
    }
}
