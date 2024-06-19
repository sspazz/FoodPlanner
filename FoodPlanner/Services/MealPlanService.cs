using FoodPlanner.Models;
using FoodPlanner.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FoodPlanner.Services
{
    public class MealPlanService : IMealPlanService
    {
        private readonly IMealPlanRepository _mealPlanRepository;

        public MealPlanService(IMealPlanRepository mealPlanRepository)
        {
            _mealPlanRepository = mealPlanRepository;
        }

        public async Task<IEnumerable<MealPlan>> GetMealPlansAsync()
        {
            return await _mealPlanRepository.GetMealPlansWithRecipesAsync();
        }

        public async Task<MealPlan> GetMealPlanByIdAsync(int id)
        {
            return await _mealPlanRepository.GetByIdAsync(id);
        }

        public async Task<int> AddMealPlanAsync(MealPlan mealPlan)
        {
            return await _mealPlanRepository.AddAsync(mealPlan);
        }

        public async Task UpdateMealPlanAsync(MealPlan mealPlan)
        {
            await _mealPlanRepository.UpdateAsync(mealPlan);
        }

        public async Task DeleteMealPlanAsync(int id)
        {
            await _mealPlanRepository.DeleteAsync(id);
        }
    }
}
