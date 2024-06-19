using FoodPlanner.Models;
using FoodPlanner.Repositories;
using FoodPlanner.Services;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace FoodPlanner.Tests
{
    public class IngredientServiceTests
    {
        private readonly Mock<IIngredientRepository> _ingredientRepositoryMock;
        private readonly IngredientService _ingredientService;

        public IngredientServiceTests()
        {
            _ingredientRepositoryMock = new Mock<IIngredientRepository>();
            _ingredientService = new IngredientService(_ingredientRepositoryMock.Object);
        }

        [Fact]
        public async Task GetIngredientsAsync_ShouldReturnAllIngredients()
        {
            // Arrange
            var ingredients = new List<Ingredient>
            {
                new Ingredient { Id = 1, Name = "Tuna" },
                new Ingredient { Id = 2, Name = "Mayonnaise" }
            };

            _ingredientRepositoryMock.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(ingredients);

            // Act
            var result = await _ingredientService.GetIngredientsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Equal("Tuna", result.ToList()[0].Name);
        }

        [Fact]
        public async Task GetIngredientByIdAsync_ShouldReturnIngredient()
        {
            // Arrange
            var ingredient = new Ingredient { Id = 1, Name = "Tuna" };
            _ingredientRepositoryMock.Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(ingredient);

            // Act
            var result = await _ingredientService.GetIngredientByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Tuna", result.Name);
        }

        [Fact]
        public async Task AddIngredientAsync_ShouldAddIngredient()
        {
            // Arrange
            var ingredient = new Ingredient { Id = 1, Name = "Tuna" };
            _ingredientRepositoryMock.Setup(repo => repo.AddAsync(ingredient))
                .ReturnsAsync(1);

            // Act
            var result = await _ingredientService.AddIngredientAsync(ingredient);

            // Assert
            Assert.Equal(1, result);
            _ingredientRepositoryMock.Verify(repo => repo.AddAsync(ingredient), Times.Once);
        }

        [Fact]
        public async Task DeleteIngredientAsync_ShouldDeleteIngredient()
        {
            // Arrange
            var ingredientId = 1;
            _ingredientRepositoryMock.Setup(repo => repo.DeleteAsync(ingredientId))
                .Returns(Task.CompletedTask);

            // Act
            await _ingredientService.DeleteIngredientAsync(ingredientId);

            // Assert
            _ingredientRepositoryMock.Verify(repo => repo.DeleteAsync(ingredientId), Times.Once);
        }
    }
}
