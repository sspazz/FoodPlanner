using FoodPlanner.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FoodPlanner.Services
{
    public interface IMealPlanService
    {
        Task<IEnumerable<MealPlan>> GetMealPlansAsync();
        Task<MealPlan> GetMealPlanByIdAsync(int id);
        Task<int> AddMealPlanAsync(MealPlan mealPlan);
        Task UpdateMealPlanAsync(MealPlan mealPlan);
        Task DeleteMealPlanAsync(int id);
    }
}
