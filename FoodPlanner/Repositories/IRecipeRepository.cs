using FoodPlanner.Models;
using System.Threading.Tasks;

namespace FoodPlanner.Repositories
{
    public interface IRecipeRepository : IRepository<Recipe>
    {
        Task<IEnumerable<Recipe>> GetRecipesWithIngredientsAsync();
    }
}