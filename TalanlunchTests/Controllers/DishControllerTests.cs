/*using Moq;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TalanLunch.Application.Interfaces;
using TalanLunch.Application.Dtos;
using TalanLunch.Domain.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using Xunit;
using talanlunch.Controllers;

namespace TalanLunchTests.Controllers
{
    public class DishControllerTests
    {
        private readonly Mock<IDishService> _mockService;
        private readonly DishController _controller;

        public DishControllerTests()
        {
            _mockService = new Mock<IDishService>();
            _controller = new DishController(_mockService.Object);
        }

        [Fact]
        public async Task AddDish_ReturnsCreatedAtActionResult_WhenValid()
        {
            // Arrange
            var dishDto = new DishDto { DishName = "Pizza", DishDescription = "Delicious pizza", DishPrice = 12.99M };
            var createdDish = new Dish { DishId = 1, DishName = "Pizza" };

            _mockService.Setup(s => s.AddDishAsync(dishDto)).ReturnsAsync(createdDish);

            // Act
            var result = await _controller.AddDish(dishDto);

            // Assert
            var actionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal("GetDishById", actionResult.ActionName);  // Ensure action name matches the route
            Assert.Equal(createdDish, actionResult.Value); // Validate returned object is the created dish
            Assert.Equal(201, actionResult.StatusCode); // Ensure status code is "Created"
        }

        [Fact]
        public async Task AddDish_ReturnsBadRequest_WhenDishDtoIsNull()
        {
            // Act
            var result = await _controller.AddDish(null);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode); // Ensure 400 status code is returned
        }

        [Fact]
        public async Task GetDishById_ReturnsDish_WhenFound()
        {
            // Arrange
            var dish = new Dish { DishId = 1, DishName = "Burger" };
            _mockService.Setup(s => s.GetDishByIdAsync(1)).ReturnsAsync(dish);

            // Act
            var result = await _controller.GetDishById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(200, okResult.StatusCode); // Ensure 200 OK status code
            Assert.Equal(dish, okResult.Value);  // Ensure dish returned is the same as expected
        }

        [Fact]
        public async Task GetDishById_ReturnsNotFound_WhenNotFound()
        {
            // Arrange
            _mockService.Setup(s => s.GetDishByIdAsync(99)).ReturnsAsync((Dish)null);

            // Act
            var result = await _controller.GetDishById(99);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result.Result);
            Assert.Equal(404, notFoundResult.StatusCode); // Ensure 404 Not Found status code
        }

        [Fact]
        public async Task GetAllDishes_ReturnsListOfDishes()
        {
            // Arrange
            var dishes = new List<Dish> {
                new Dish { DishId = 1, DishName = "Tacos" },
                new Dish { DishId = 2, DishName = "Sushi" }
            };

            _mockService.Setup(s => s.GetAllDishesAsync()).ReturnsAsync(dishes);

            // Act
            var result = await _controller.GetAllDishes();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode); // Ensure 200 OK status code
            Assert.Equal(dishes, okResult.Value);  // Ensure the list returned matches the mock data
        }

        [Fact]
        public async Task DeleteDish_ReturnsNoContent_WhenSuccess()
        {
            // Arrange
            _mockService.Setup(s => s.DeleteDishAsync(1)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteDish(1);

            // Assert
            Assert.IsType<NoContentResult>(result);  // Ensure NoContent status is returned
            _mockService.Verify(s => s.DeleteDishAsync(1), Times.Once); // Ensure the delete method was called once
        }

        [Fact]
        public async Task DeleteDish_ReturnsNotFound_WhenDishNotExist()
        {
            // Arrange
            var dishId = 99; // Un ID qui n'existe pas
            _mockService.Setup(s => s.DeleteDishAsync(dishId)).ThrowsAsync(new KeyNotFoundException($"Dish with ID {dishId} not found."));

            // Act
            var result = await _controller.DeleteDish(dishId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode); // Vérifie le code 404
            Assert.Equal("Dish with ID 99 not found.", notFoundResult.Value); // Vérifie le message d'erreur
        }

        [Fact]
        public async Task AddDish_ReturnsBadRequest_WhenRequiredFieldsAreMissing()
        {
            // Arrange : Missing DishName should trigger validation failure
            var dishDto = new DishDto { DishDescription = "Missing name", DishPrice = 10.99M };

            // Act
            var result = await _controller.AddDish(dishDto);

            // Assert : Ensure we get a BadRequestObjectResult
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode); // Ensure 400 BadRequest is returned

            // Check if error message contains the expected validation error
            var modelState = badRequestResult.Value as SerializableError;
            Assert.NotNull(modelState);
            Assert.True(modelState.ContainsKey("DishName")); // Vérifie que l'erreur de validation de "DishName" existe
        }

        [Fact]
        public async Task PatchDish_ReturnsOk_WhenDishUpdatedSuccessfully()
        {
            // Arrange
            var existingDish = new Dish { DishId = 1, DishName = "Burger", DishDescription = "Delicious burger", DishPrice = 8.99M };
            var updatedDishDto = new DishUpdateDto { DishName = "Updated Burger", DishDescription = "Updated delicious burger", DishPrice = 9.99M };
            var updatedDish = new Dish { DishId = 1, DishName = "Updated Burger", DishDescription = "Updated delicious burger", DishPrice = 9.99M };

            // Mock the service methods
            _mockService.Setup(s => s.GetDishByIdAsync(1)).ReturnsAsync(existingDish);
            _mockService.Setup(s => s.UpdateDishAsync(existingDish, updatedDishDto, null)).ReturnsAsync(updatedDish);

            // Act
            var result = await _controller.PatchDish(1, updatedDishDto, null);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode); // Ensure status code is OK
            Assert.Equal(updatedDish, okResult.Value); // Ensure the returned dish matches the updated dish
        }

        [Fact]
        public async Task PatchDish_ReturnsNotFound_WhenDishDoesNotExist()
        {
            // Arrange
            var updatedDishDto = new DishUpdateDto { DishName = "Non-Existing Dish" };

            // Mock the service methods
            _mockService.Setup(s => s.GetDishByIdAsync(99)).ReturnsAsync((Dish)null); // Dish with ID 99 does not exist

            // Act
            var result = await _controller.PatchDish(99, updatedDishDto, null);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode); // Ensure status code is NotFound
            Assert.Equal("Dish with id 99 not found.", notFoundResult.Value); // Ensure error message is returned
        }

        [Fact]
        public async Task PatchDish_ReturnsBadRequest_WhenDishDataIsNull()
        {
            // Act
            var result = await _controller.PatchDish(1, null, null);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode); // Ensure BadRequest status code is returned
            Assert.Equal("Dish data is null.", badRequestResult.Value); // Ensure error message is correct
        }
    }
}
*/