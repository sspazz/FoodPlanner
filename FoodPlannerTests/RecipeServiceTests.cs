using FoodPlanner.Models;
using FoodPlanner.Repositories;
using FoodPlanner.Services;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace FoodPlanner.Tests
{
    public class RecipeServiceTests
    {
        private readonly Mock<IRecipeRepository> _recipeRepositoryMock;
        private readonly RecipeService _recipeService;

        public RecipeServiceTests()
        {
            _recipeRepositoryMock = new Mock<IRecipeRepository>();
            _recipeService = new RecipeService(_recipeRepositoryMock.Object);
        }

        [Fact]
        public async Task GetRecipesAsync_ShouldReturnAllRecipes()
        {
            // Arrange
            var recipes = new List<Recipe>
            {
                new Recipe { Id = 1, Name = "Tuna Salad", Description = "A healthy and delicious tuna salad." }
            };

            _recipeRepositoryMock.Setup(repo => repo.GetRecipesWithIngredientsAsync())
                .ReturnsAsync(recipes);

            // Act
            var result = await _recipeService.GetRecipesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("Tuna Salad", result.First().Name);
        }

        [Fact]
        public async Task AddRecipeAsync_ShouldAddRecipe()
        {
            // Arrange
            var recipe = new Recipe { Id = 1, Name = "Tuna Salad", Description = "A healthy and delicious tuna salad." };
            _recipeRepositoryMock.Setup(repo => repo.AddAsync(recipe)).ReturnsAsync(1);

            // Act
            var result = await _recipeService.AddRecipeAsync(recipe);

            // Assert
            Assert.Equal(1, result);
            _recipeRepositoryMock.Verify(repo => repo.AddAsync(recipe), Times.Once);
        }

        [Fact]
        public async Task DeleteRecipeAsync_ShouldDeleteRecipe()
        {
            // Arrange
            var recipeId = 1;
            _recipeRepositoryMock.Setup(repo => repo.DeleteAsync(recipeId)).Returns(Task.CompletedTask);

            // Act
            await _recipeService.DeleteRecipeAsync(recipeId);

            // Assert
            _recipeRepositoryMock.Verify(repo => repo.DeleteAsync(recipeId), Times.Once);
        }
    }
}
