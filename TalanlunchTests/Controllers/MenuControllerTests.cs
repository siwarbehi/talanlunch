/*using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using TalanLunch.Application.Dtos;
using TalanLunch.Application.Interfaces;
using TalanLunch.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using talanlunch.Controllers;

namespace TalanLunch.Tests.Controllers
{
    public class MenuControllerTests
    {
        private readonly Mock<IMenuService> _mockMenuService;
        private readonly MenuController _controller;

        public MenuControllerTests()
        {
            _mockMenuService = new Mock<IMenuService>();
            _controller = new MenuController(_mockMenuService.Object);
        }

        // Test de l'ajout d'un menu
        [Fact]
        public async Task AddMenuAsync_ValidMenu_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var menuDto = new MenuDto
            {
                MenuDescription = "Menu de test",
                Dishes = new List<DishCreationDto>
                {
                    new DishCreationDto { DishId = 1, DishQuantity = 2 }
                }
            };
            var createdMenu = new Menu
            {
                MenuId = 1,
                MenuDescription = "Menu de test",
                MenuDate = System.DateTime.Now
            };
            _mockMenuService.Setup(service => service.AddMenuAsync(menuDto))
                            .ReturnsAsync(createdMenu);

            // Act
            var result = await _controller.AddMenuAsync(menuDto);

            // Assert
            var actionResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnValue = Assert.IsType<Menu>(actionResult.Value);
            Assert.Equal(createdMenu.MenuId, returnValue.MenuId);
        }

        // Test de l'update de la description d'un menu
        [Fact]
        public async Task UpdateMenuDescription_ValidId_ReturnsNoContent()
        {
            // Arrange
            var menuId = 1;
            var newDescription = "Nouvelle description du menu";
            _mockMenuService.Setup(service => service.UpdateMenuDescriptionAsync(menuId, newDescription))
                            .ReturnsAsync(true);

            // Act
            var result = await _controller.UpdateMenuDescription(menuId, newDescription);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task AddDishToMenu_ValidDish_ReturnsOkResult()
        {
            // Arrange
            var menuId = 1;
            var dishId = 1;
            var quantity = 2;
            var menu = new Menu
            {
                MenuId = menuId,
                MenuDescription = "Menu de test"
            };

            _mockMenuService.Setup(service => service.AddDishToMenuAsync(menuId, dishId, quantity))
                            .ReturnsAsync((menu, false));

            // Act
            var result = await _controller.AddDishToMenu(menuId, dishId, quantity);

            // ✅ Extraire le Result depuis ActionResult<Menu>
            var okResult = Assert.IsType<OkObjectResult>(result.Result);

            // ✅ Vérifier le contenu
            var returnValue = Assert.IsType<Menu>(okResult.Value);
            Assert.Equal(menu.MenuId, returnValue.MenuId);
        }


        // Test de la suppression d'un plat du menu
        [Fact]
        public async Task RemoveDishFromMenu_ValidDish_ReturnsOkResult()
        {
            // Arrange
            var menuId = 1;
            var dishId = 1;
            var menu = new Menu
            {
                MenuId = menuId,
                MenuDescription = "Menu test",
                MenuDishes = new List<MenuDish> {
            new MenuDish { DishId = dishId }
        }
            };

            _mockMenuService.Setup(service => service.RemoveDishFromMenuAsync(menuId, dishId))
                            .ReturnsAsync(menu);

            // Act
            var result = await _controller.RemoveDishFromMenu(menuId, dishId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<Menu>(okResult.Value);
            Assert.Equal(menu.MenuId, returnValue.MenuId);
        }

        // Test de la suppression d'un menu
        [Fact]
        public async Task DeleteMenu_ValidId_ReturnsNoContent()
        {
            // Arrange
            var menuId = 1;
            _mockMenuService.Setup(service => service.DeleteMenuAsync(menuId))
                            .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteMenu(menuId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        // Test de la récupération d'un menu par ID
        [Fact]
        public async Task GetMenu_ValidId_ReturnsOkResult()
        {
            // Arrange
            var menuId = 1;
            var menu = new Menu
            {
                MenuId = menuId,
                MenuDescription = "Menu de test"
            };
            _mockMenuService.Setup(service => service.GetMenuByIdAsync(menuId))
                            .ReturnsAsync(menu);

            // Act
            var result = await _controller.GetMenu(menuId);

            // ✅ Extraire l'objet `Result` depuis `ActionResult<Menu>`
            var okResult = Assert.IsType<OkObjectResult>(result.Result);

            // ✅ Vérifie que le contenu est bien un `Menu`
            var returnValue = Assert.IsType<Menu>(okResult.Value);
            Assert.Equal(menu.MenuId, returnValue.MenuId);
        }


        // Test de la récupération de tous les menus
        [Fact]
      
        public async Task GetMenus_ReturnsOkResult()
        {
            // Arrange
            var menus = new List<GetAllMenusDto>
    {
        new GetAllMenusDto
        {
            MenuId = 1,
            MenuDescription = "Menu 1",
            Dishes = new List<DishMenuAllDto>()
        },
        new GetAllMenusDto
        {
            MenuId = 2,
            MenuDescription = "Menu 2",
            Dishes = new List<DishMenuAllDto>()
        }
    };
            _mockMenuService.Setup(service => service.GetAllMenusAsync())
                            .ReturnsAsync(menus);

            // Act
            var result = await _controller.GetMenus();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<GetAllMenusDto>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }


        // Test de la mise à jour du menu du jour
        [Fact]
        public async Task SetMenuOfTheDay_ValidId_ReturnsOkResult()
        {
            // Arrange
            var menuId = 1;
            _mockMenuService.Setup(service => service.SetMenuOfTheDayAsync(menuId))
                            .ReturnsAsync(true);

            // Act
            var result = await _controller.SetMenuOfTheDay(menuId);

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Menu of the day has been updated.", actionResult.Value);
        }
    }
}*/
