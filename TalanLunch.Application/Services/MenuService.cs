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

        
      /*  public async Task<Menu> AddMenuAsync(MenuDto menuDto)
        {
            if (menuDto.Dishes == null || !menuDto.Dishes.Any())
                throw new ArgumentException("Le menu doit contenir au moins un plat.", nameof(menuDto.Dishes));

            var dishIds = menuDto.Dishes.Select(dish => dish.DishId).ToList();

            var dishes = await _dishRepository.GetDishesByIdsAsync(dishIds);

            var dishesById = dishes.ToDictionary(d => d.DishId);
            var invalidDishIds = dishIds.Except(dishes.Select(d => d.DishId)).ToList();

            if (invalidDishIds.Any())
            {
                throw new ArgumentException(
                    $"Les identifiants de plats suivants sont invalides : {string.Join(", ", invalidDishIds)}.",
                    nameof(menuDto.Dishes));
            }

            var newMenu = _mapper.Map<Menu>(menuDto);
            newMenu.MenuDate = DateTime.Now; 

            newMenu.MenuDishes = menuDto.Dishes
                .Where(dishDto => dishesById.ContainsKey(dishDto.DishId))
                .Select(dishDto =>
                {
                    var dish = dishesById[dishDto.DishId];
                    return _mapper.Map<MenuDish>(dishDto);
                })
                .ToList();

            foreach (var menuDish in newMenu.MenuDishes)
            {
                menuDish.Menu = newMenu;
                menuDish.Dish = dishesById[menuDish.DishId];
            }

            return await _menuRepository.AddMenuAsync(newMenu);
        }*/

       /* // update description & add dish to menu 

        public async Task<AddDishToMenuResultDto?> AddDishToMenuAsync(int menuId, AddDishToMenuDto dto)
        {
            var menu = await _menuRepository.GetMenuByIdAsync(menuId);
            if (menu == null)
                return null;

            if (!string.IsNullOrWhiteSpace(dto.NewDescription))
            {
                menu.MenuDescription = dto.NewDescription;
            }

            if (dto.DishId > 0)
            {
                var dish = await _dishRepository.GetDishByIdAsync(dto.DishId);
                if (dish == null)
                    return null;

                if (menu.MenuDishes.Any(md => md.DishId == dto.DishId))
                {
                    return new AddDishToMenuResultDto
                    {
                        DishAlreadyExists = true
                    };
                }

                var menuDish = _mapper.Map<MenuDish>(dto);
                menuDish.Dish = dish;
                menuDish.Menu = menu;
                menuDish.MenuId = menuId;
                menuDish.DishId = dto.DishId;

                menu.MenuDishes.Add(menuDish);
            }

            await _menuRepository.UpdateMenuAsync(menu);

            return new AddDishToMenuResultDto
            {
                DishAlreadyExists = false
            };
        }*/



    /*    // Supprimer un plat du menu
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
        }*/

      /*  // Supprimer un menu
        public async Task DeleteMenuAsync(int id)
        {
            var menu = await _menuRepository.GetMenuByIdAsync(id);
            if (menu != null)
            {
                await _menuRepository.DeleteMenuAsync(id);
            }
        }*/

      /*  // Obtenir un menu par ID
        public async Task<Menu?> GetMenuByIdAsync(int id)
        {
            return await _menuRepository.GetMenuByIdAsync(id);
        }*/
      /*  // Obtenir tous les menus 
        public async Task<IEnumerable<GetAllMenusDto>> GetAllMenusAsync()
        {
            var menus = await _menuRepository.GetAllMenusAsync();
            return _mapper.Map<IEnumerable<GetAllMenusDto>>(menus);
        }*/

      //definir le menu de jour 
     /*   public async Task<bool> SetMenuOfTheDayAsync(int menuId)
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
        }*/




    }
}