using FoodPlanner.Models;
using FoodPlanner.Repositories;
using FoodPlanner.Services;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace FoodPlanner.Tests
{
    public class MealPlanServiceTests
    {
        private readonly Mock<IMealPlanRepository> _mealPlanRepositoryMock;
        private readonly MealPlanService _mealPlanService;

        public MealPlanServiceTests()
        {
            _mealPlanRepositoryMock = new Mock<IMealPlanRepository>();
            _mealPlanService = new MealPlanService(_mealPlanRepositoryMock.Object);
        }

        [Fact]
        public async Task GetMealPlansAsync_ShouldReturnAllMealPlans()
        {
            // Arrange
            var mealPlans = new List<MealPlan>
            {
                new MealPlan { Id = 1, Name = "Weekly Meal Plan", Date = DateTime.Now }
            };

            _mealPlanRepositoryMock.Setup(repo => repo.GetMealPlansWithRecipesAsync())
                .ReturnsAsync(mealPlans);

            // Act
            var result = await _mealPlanService.GetMealPlansAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("Weekly Meal Plan", result.ToList()[0].Name);
        }

        [Fact]
        public async Task GetMealPlanByIdAsync_ShouldReturnMealPlan()
        {
            // Arrange
            var mealPlan = new MealPlan { Id = 1, Name = "Weekly Meal Plan", Date = DateTime.Now };
            _mealPlanRepositoryMock.Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(mealPlan);

            // Act
            var result = await _mealPlanService.GetMealPlanByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Weekly Meal Plan", result.Name);
        }

        [Fact]
        public async Task AddMealPlanAsync_ShouldAddMealPlan()
        {
            // Arrange
            var mealPlan = new MealPlan { Id = 1, Name = "Weekly Meal Plan", Date = DateTime.Now };
            _mealPlanRepositoryMock.Setup(repo => repo.AddAsync(mealPlan))
                .ReturnsAsync(1);

            // Act
            var result = await _mealPlanService.AddMealPlanAsync(mealPlan);

            // Assert
            Assert.Equal(1, result);
            _mealPlanRepositoryMock.Verify(repo => repo.AddAsync(mealPlan), Times.Once);
        }

        [Fact]
        public async Task DeleteMealPlanAsync_ShouldDeleteMealPlan()
        {
            // Arrange
            var mealPlanId = 1;
            _mealPlanRepositoryMock.Setup(repo => repo.DeleteAsync(mealPlanId))
                .Returns(Task.CompletedTask);

            // Act
            await _mealPlanService.DeleteMealPlanAsync(mealPlanId);

            // Assert
            _mealPlanRepositoryMock.Verify(repo => repo.DeleteAsync(mealPlanId), Times.Once);
        }
    }
}
