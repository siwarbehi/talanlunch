using AutoMapper;
using TalanLunch.Application.Dtos.Menu;
using TalanLunch.Application.Interfaces;
using TalanLunch.Domain.Entities;

namespace TalanLunch.Application.Services
{
    public class MenuService : IMenuService
    {
        private readonly IMenuRepository _menuRepository;
        private readonly IDishRepository _dishRepository;
        private readonly IMapper _mapper;
        public MenuService(IMenuRepository menuRepository, IDishRepository dishRepository, IMapper mapper)
        {
            _menuRepository = menuRepository;
            _dishRepository = dishRepository;
            _mapper = mapper;
        }

        // Creation d un menu 
        public async Task<Menu> AddMenuAsync(MenuDto menuDto)
        {
            if (menuDto == null)
                throw new ArgumentNullException(nameof(menuDto), "Le DTO du menu ne peut pas être null.");

            if (menuDto.Dishes == null || !menuDto.Dishes.Any())
                throw new ArgumentException("Le menu doit contenir au moins un plat.", nameof(menuDto.Dishes));

            var invalidDishIds = new List<int>();

            var menu = _mapper.Map<Menu>(menuDto);

            foreach (var dishDto in menuDto.Dishes)
            {
                var dish = await _dishRepository.GetDishByIdAsync(dishDto.DishId);
                if (dish != null)
                {
                    var menuDish = _mapper.Map<MenuDish>(dishDto);
                    menuDish.Dish = dish;
                    menuDish.Menu = menu;
                    menu.MenuDishes.Add(menuDish);
                }
                else
                {
                    invalidDishIds.Add(dishDto.DishId);
                }
            }

            if (invalidDishIds.Any())
                throw new ArgumentException($"Certains plats sont invalides : {string.Join(", ", invalidDishIds)}");

            return await _menuRepository.AddMenuAsync(menu);
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
        public async Task<(Menu?, bool)> AddDishToMenuAsync(int menuId, int dishId, int quantity)
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
                Dish = dish,
                DishQuantity = quantity   
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

            var allMenus = await _menuRepository.GetAllMenus();

            var previousMenu = allMenus.FirstOrDefault(menu => menu.IsMenuOfTheDay == true);

            if (previousMenu != null)
            {
                previousMenu.IsMenuOfTheDay = false; 
                
                await _menuRepository.UpdateMenuAsync(previousMenu);
            }

            menu.IsMenuOfTheDay = true;

            await _menuRepository.UpdateMenuAsync(menu);

            return true;
        }


      

    }
}