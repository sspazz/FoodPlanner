using FoodPlanner.Models;
using System.Threading.Tasks;

namespace FoodPlanner.Repositories
{
    public interface IMealPlanRepository : IRepository<MealPlan>
    {
        Task<IEnumerable<MealPlan>> GetMealPlansWithRecipesAsync();
    }
}