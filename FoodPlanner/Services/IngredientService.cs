using FoodPlanner.Models;
using FoodPlanner.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FoodPlanner.Services
{
    public class IngredientService : IIngredientService
    {
        private readonly IIngredientRepository _ingredientRepository;

        public IngredientService(IIngredientRepository ingredientRepository)
        {
            _ingredientRepository = ingredientRepository;
        }

        public async Task<IEnumerable<Ingredient>> GetIngredientsAsync()
        {
            return await _ingredientRepository.GetAllAsync();
        }

        public async Task<Ingredient> GetIngredientByIdAsync(int id)
        {
            return await _ingredientRepository.GetByIdAsync(id);
        }

        public async Task<int> AddIngredientAsync(Ingredient ingredient)
        {
            return await _ingredientRepository.AddAsync(ingredient);
        }

        public async Task UpdateIngredientAsync(Ingredient ingredient)
        {
            await _ingredientRepository.UpdateAsync(ingredient);
        }

        public async Task DeleteIngredientAsync(int id)
        {
            await _ingredientRepository.DeleteAsync(id);
        }
    }
}
