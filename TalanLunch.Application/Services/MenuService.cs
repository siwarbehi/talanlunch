using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalanLunch.Application.Dtos;
using TalanLunch.Application.Interfaces;
using TalanLunch.Domain.Entities;

namespace TalanLunch.Application.Services
{
    public class MenuService : IMenuService
    {
        private readonly IMenuRepository _menuRepository;
        private readonly IDishRepository _dishRepository;
        public MenuService(IMenuRepository menuRepository, IDishRepository dishRepository)
        {
            _menuRepository = menuRepository;
            _dishRepository = dishRepository;
        }

        public async Task<Menu> AddMenuAsync(MenuDto menuDto)
        {
            if (menuDto == null)
                throw new ArgumentNullException(nameof(menuDto), "Le DTO du menu ne peut pas être null.");

            if (menuDto.Dishes == null || !menuDto.Dishes.Any())
                throw new ArgumentException("Le menu doit contenir au moins un plat.", nameof(menuDto.Dishes));

            // Création du menu
            var newMenu = new Menu
            {
                MenuDescription = menuDto.MenuDescription,
                MenuDate = DateTime.Now,
                MenuDishes = new List<MenuDish>()
            };

            var invalidDishIds = new List<int>();

            // Traitement de chaque plat
            foreach (var dishDto in menuDto.Dishes)
            {
                if (dishDto == null)
                    throw new Exception("dishDto est null");

                if (_dishRepository == null)
                    throw new Exception("_dishRepository est null");

                var dish = await _dishRepository.GetDishByIdAsync(dishDto.DishId);

                if (dish == null)
                    throw new Exception($"Aucun plat trouvé avec l'ID {dishDto.DishId}");

                if (dish != null)
                {
                    var menuDish = new MenuDish
                    {
                        Menu = newMenu,
                        Dish = dish,
                        DishId = dish.DishId,
                        DishQuantity = dishDto.DishQuantity
                    };
                    newMenu.MenuDishes.Add(menuDish);
                }
                else
                {
                    invalidDishIds.Add(dishDto.DishId);
                }
            }

            if (invalidDishIds.Any())
            {
                throw new ArgumentException($"Certains identifiants de plats sont invalides : {string.Join(", ", invalidDishIds)}", nameof(menuDto.Dishes));
            }

            return await _menuRepository.AddMenuAsync(newMenu);
        }

    


        // Modifier la description du menu
        public async Task<bool> UpdateMenuDescriptionAsync(int id, string newDescription)
        {
            var menu = await _menuRepository.GetMenuByIdAsync(id);
            if (menu == null) return false;

            menu.MenuDescription = newDescription;
            await _menuRepository.UpdateMenuAsync(menu);
            return true;
        }

        // Ajouter un plat au menu
        public async Task<(Menu?, bool)> AddDishToMenuAsync(int menuId, int dishId)
        {
            var menu = await _menuRepository.GetMenuByIdAsync(menuId);
            var dish = await _dishRepository.GetDishByIdAsync(dishId);

            if (menu == null || dish == null)
            {
                return (null, false); 
            }

            if (menu.MenuDishes.Any(md => md.DishId == dishId))
            {
                return (menu, true); 
            }

            menu.MenuDishes.Add(new MenuDish
            {
                MenuId = menuId,
                DishId = dishId,
                Menu = menu,
                Dish = dish
            });

            var updatedMenu = await _menuRepository.UpdateMenuAsync(menu);
            return (updatedMenu, false); 
        }




        // Supprimer un plat du menu
        public async Task<Menu?> RemoveDishFromMenuAsync(int menuId, int dishId)
        {
            var menu = await _menuRepository.GetMenuByIdAsync(menuId);
            var dish = await _dishRepository.GetDishByIdAsync(dishId);

            if (menu == null || dish == null) return null;

            var menuDish = menu.MenuDishes.FirstOrDefault(md => md.DishId == dishId);
            if (menuDish != null)
            {
                menu.MenuDishes.Remove(menuDish);
            }

            return await _menuRepository.UpdateMenuAsync(menu);
        }

        // Supprimer un menu
        public async Task DeleteMenuAsync(int id)
        {
            var menu = await _menuRepository.GetMenuByIdAsync(id);
            if (menu != null)
            {
                await _menuRepository.DeleteMenuAsync(id);
            }
        }

        // Obtenir un menu par ID
        public async Task<Menu?> GetMenuByIdAsync(int id)
        {
            return await _menuRepository.GetMenuByIdAsync(id);
        }
        // Obtenir tous les menus 
        public async Task<IEnumerable<GetAllMenusDto>> GetAllMenusAsync() { 
            return await _menuRepository.GetAllMenusAsync(); }
        public List<int> GetAllMenuIds()
        {
            return _menuRepository.GetAllMenuIds(); 
        }
        public List<int> GetDishIdsForMenu(int menuId)
        {
            return _menuRepository.GetDishIdsByMenuId(menuId);
        }
        public async Task<bool> SetMenuOfTheDayAsync(int menuId)
        {
            var menu = await _menuRepository.GetMenuByIdAsync(menuId);
            if (menu == null)
            {
                return false; 
            }

            var allMenuIds = _menuRepository.GetAllMenuIds();

            foreach (var id in allMenuIds)
            {
                var m = await _menuRepository.GetMenuByIdAsync(id);
                if (m == null)
                {
                    continue; 
                }

                if (m.MenuId != menuId)
                {
                    m.IsMenuOfTheDay = false;
                    await _menuRepository.UpdateMenuAsync(m);
                }
            }

            menu.IsMenuOfTheDay = true;

            await _menuRepository.UpdateMenuAsync(menu);

            return true;
        }


        public async Task ResetMenuOfTheDayAsync()
        {
            var allMenuIds = _menuRepository.GetAllMenuIds();

            foreach (var menuId in allMenuIds)
            {
                var menu = await _menuRepository.GetMenuByIdAsync(menuId);
                if (menu != null)
                {
                    menu.IsMenuOfTheDay = false;
                    await _menuRepository.UpdateMenuAsync(menu);  // Mettre à jour chaque menu
                }
            }
        }

    }
}