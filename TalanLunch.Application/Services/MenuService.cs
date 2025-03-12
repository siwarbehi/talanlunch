using Microsoft.AspNetCore.Http;
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

        // Ajouter un menu
        public async Task<Menu> AddMenuAsync(MenuDto menuDto)
        {
            var newMenu = new Menu
            {
                MenuDescription = menuDto.MenuDescription,
                MenuDate = DateTime.Now,
                MenuDishes = new List<MenuDish>()
            };

            var invalidDishIds = new List<int>();

            if (menuDto.DishIds != null && menuDto.DishIds.Count > 0)
            {
                foreach (var dishId in menuDto.DishIds)
                {
                    var dish = await _dishRepository.GetDishByIdAsync(dishId);
                    if (dish != null)
                    {
                        newMenu.MenuDishes.Add(new MenuDish
                        {
                            Menu = newMenu,
                            Dish = dish,
                            DishId = dish.DishId
                        });
                    }
                    else
                    {
                        invalidDishIds.Add(dishId);
                    }
                }
            }

            if (invalidDishIds.Count > 0)
            {
                throw new ArgumentException("Certains identifiants de plats sont invalides.", nameof(menuDto.DishIds));
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
        public async Task<Menu?> AddDishToMenuAsync(int menuId, int dishId)
        {
            var menu = await _menuRepository.GetMenuByIdAsync(menuId);
            var dish = await _dishRepository.GetDishByIdAsync(dishId);

            if (menu == null || dish == null) return null;

            menu.MenuDishes.Add(new MenuDish
            {
                MenuId = menuId,
                DishId = dishId,
                Menu = menu,         // Associer l'objet menu réel
                Dish = dish          // Associer l'objet plat réel
            });
            return await _menuRepository.UpdateMenuAsync(menu);
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
        //Obtenir tous les menus
        public async Task<IEnumerable<Menu>> GetAllMenusAsync()
        {
            
            return await _menuRepository.GetAllMenusAsync();
        }
    }
}