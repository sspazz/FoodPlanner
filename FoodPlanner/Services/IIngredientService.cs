using FoodPlanner.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FoodPlanner.Services
{
    public interface IIngredientService
    {
        Task<IEnumerable<Ingredient>> GetIngredientsAsync();
        Task<Ingredient> GetIngredientByIdAsync(int id);
        Task<int> AddIngredientAsync(Ingredient ingredient);
        Task UpdateIngredientAsync(Ingredient ingredient);
        Task DeleteIngredientAsync(int id);
    }
}
