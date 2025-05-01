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
        /*public async Task<Menu> AddMenuAsync(MenuDto menuDto)
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
        }*/

        public async Task<Menu> AddMenuAsync(MenuDto menuDto)
        {
            if (menuDto.Dishes == null || !menuDto.Dishes.Any())
                throw new ArgumentException("Le menu doit contenir au moins un plat.", nameof(menuDto.Dishes));

            // Extraction des IDs de plats
            var dishIds = menuDto.Dishes.Select(dish => dish.DishId).ToList();

            // Chargement des plats en une seule requête
            var dishes = await _dishRepository.GetDishesByIdsAsync(dishIds);

            // Filtrage des plats valides et des plats invalides
            var dishesById = dishes.ToDictionary(d => d.DishId);
            var invalidDishIds = dishIds.Except(dishes.Select(d => d.DishId)).ToList();

            if (invalidDishIds.Any())
            {
                throw new ArgumentException(
                    $"Les identifiants de plats suivants sont invalides : {string.Join(", ", invalidDishIds)}.",
                    nameof(menuDto.Dishes));
            }

            // Création du menu à partir du DTO en utilisant AutoMapper
            var newMenu = _mapper.Map<Menu>(menuDto);
            newMenu.MenuDate = DateTime.Now; // Définir la date du menu

            // Création des MenuDish à partir des plats valides
            newMenu.MenuDishes = menuDto.Dishes
                .Where(dishDto => dishesById.ContainsKey(dishDto.DishId)) // Filtrer uniquement les plats valides
                .Select(dishDto =>
                {
                    var dish = dishesById[dishDto.DishId];
                    return _mapper.Map<MenuDish>(dishDto);
                })
                .ToList();

            // Lier chaque MenuDish avec le Menu et le Plat
            foreach (var menuDish in newMenu.MenuDishes)
            {
                menuDish.Menu = newMenu;
                menuDish.Dish = dishesById[menuDish.DishId];
            }

            // Ajouter le menu dans la base de données
            return await _menuRepository.AddMenuAsync(newMenu);
        }

       

        public async Task<AddDishToMenuResult?> AddDishToMenuAsync(int menuId, int dishId, int quantity, string? newDescription = null)
        {
            var menu = await _menuRepository.GetMenuByIdAsync(menuId);
            if (menu == null)
                return null;

            // Mise à jour de la description si fournie
            if (!string.IsNullOrWhiteSpace(newDescription))
            {
                menu.MenuDescription = newDescription;
            }

            // Si DishId est 0 ou négatif, on ne fait pas d'ajout de plat
            if (dishId > 0)
            {
                var dish = await _dishRepository.GetDishByIdAsync(dishId);
                if (dish == null)
                    return null;

                if (menu.MenuDishes.Any(md => md.DishId == dishId))
                {
                    return new AddDishToMenuResult
                    {
                        Menu = menu,
                        DishAlreadyExists = true
                    };
                }

                menu.MenuDishes.Add(new MenuDish
                {
                    MenuId = menuId,
                    DishId = dishId,
                    Menu = menu,
                    Dish = dish,
                    DishQuantity = quantity
                });
            }

            var updatedMenu = await _menuRepository.UpdateMenuAsync(menu);

            return new AddDishToMenuResult
            {
                Menu = updatedMenu,
                DishAlreadyExists = false
            };
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
        public async Task<IEnumerable<GetAllMenusDto>> GetAllMenusAsync()
        {
            var menus = await _menuRepository.GetAllMenusAsync();
            return _mapper.Map<IEnumerable<GetAllMenusDto>>(menus);
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