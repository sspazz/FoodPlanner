using FoodPlanner.Models;
using FoodPlanner.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FoodPlanner.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MealPlansController : ControllerBase
    {
        private readonly IMealPlanService _mealPlanService;

        public MealPlansController(IMealPlanService mealPlanService)
        {
            _mealPlanService = mealPlanService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MealPlan>>> GetMealPlans()
        {
            var mealPlans = await _mealPlanService.GetMealPlansAsync();
            return Ok(mealPlans);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MealPlan>> GetMealPlan(int id)
        {
            var mealPlan = await _mealPlanService.GetMealPlanByIdAsync(id);
            if (mealPlan == null)
            {
                return NotFound();
            }
            return Ok(mealPlan);
        }

        [HttpPost]
        public async Task<ActionResult<MealPlan>> PostMealPlan(MealPlan mealPlan)
        {
            var mealPlanId = await _mealPlanService.AddMealPlanAsync(mealPlan);
            mealPlan.Id = mealPlanId;
            return CreatedAtAction(nameof(GetMealPlan), new { id = mealPlanId }, mealPlan);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutMealPlan(int id, MealPlan mealPlan)
        {
            if (id != mealPlan.Id)
            {
                return BadRequest();
            }
            await _mealPlanService.UpdateMealPlanAsync(mealPlan);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMealPlan(int id)
        {
            await _mealPlanService.DeleteMealPlanAsync(id);
            return NoContent();
        }
    }
}
