using FoodPlanner.Models;
using FoodPlanner.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FoodPlanner.Services
{
    public class RecipeService : IRecipeService
    {
        private readonly IRecipeRepository _recipeRepository;

        public RecipeService(IRecipeRepository recipeRepository)
        {
            _recipeRepository = recipeRepository;
        }

        public async Task<IEnumerable<Recipe>> GetRecipesAsync()
        {
            return await _recipeRepository.GetRecipesWithIngredientsAsync();
        }

        public async Task<Recipe> GetRecipeByIdAsync(int id)
        {
            return await _recipeRepository.GetByIdAsync(id);
        }

        public async Task<int> AddRecipeAsync(Recipe recipe)
        {
            return await _recipeRepository.AddAsync(recipe);
        }

        public async Task UpdateRecipeAsync(Recipe recipe)
        {
            await _recipeRepository.UpdateAsync(recipe);
        }

        public async Task DeleteRecipeAsync(int id)
        {
            await _recipeRepository.DeleteAsync(id);
        }
    }
}
