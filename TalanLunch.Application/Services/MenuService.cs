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

        public async Task<Menu> AddMenuAsync(MenuDto menuDto)
        {
            var newMenu = new Menu
            {
                MenuDescription = menuDto.MenuDescription,
                MenuDate = DateTime.Now,  // Date actuelle
                MenuDishes = new List<MenuDish>()  // Initialise la liste de MenuDish
            };

            var invalidDishIds = new List<int>();  // Liste pour les plats invalides

            if (menuDto.DishIds != null && menuDto.DishIds.Count > 0)
            {
                foreach (var dishId in menuDto.DishIds)
                {
                    var dish = await _dishRepository.GetDishByIdAsync(dishId);  // Recherche le plat par ID
                    if (dish != null)
                    {
                        newMenu.MenuDishes.Add(new MenuDish
                        {
                            Menu = newMenu,  // Lier le menu
                            Dish = dish,     // Lier le plat
                            DishId = dish.DishId  // Lier l'ID du plat
                        });
                    }
                    else
                    {
                        invalidDishIds.Add(dishId);  // Ajoute l'ID du plat invalide à la liste
                    }
                }
            }

            if (invalidDishIds.Count > 0)
            {
                // Si des plats sont invalides, lève une exception avec les ID des plats invalides
                throw new ArgumentException("Certains identifiants de plats sont invalides.", nameof(menuDto.DishIds));
            }

            return await _menuRepository.AddMenuAsync(newMenu);  // Ajoute le menu au repository
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
            return (updatedMenu, false); // Plat ajouté avec succès
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
        public List<int> GetAllMenuIds()
        {
            return _menuRepository.GetAllMenuIds(); // Appel du repository pour récupérer les MenuId
        }
        // Méthode pour récupérer les plats associés à un menu donné
        public List<int> GetDishIdsForMenu(int menuId)
        {
            return _menuRepository.GetDishIdsByMenuId(menuId);
        }
    }
}