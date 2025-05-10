using AutoMapper;
using MediatR;
using TalanLunch.Application.Menus.Commands.AddDishToMenu;


using TalanLunch.Application.Interfaces;
using TalanLunch.Domain.Entities;



namespace TalanLunch.Application.Menus.MenuHandlers
{
    public class AddDishToMenuCommandHandler : IRequestHandler<AddDishToMenuCommand, AddDishToMenuResult?>
    {
        private readonly IMenuRepository _menuRepository;
        private readonly IDishRepository _dishRepository;
        private readonly IMapper _mapper;

        public AddDishToMenuCommandHandler(IMenuRepository menuRepository, IDishRepository dishRepository, IMapper mapper)
        {
            _menuRepository = menuRepository;
            _dishRepository = dishRepository;
            _mapper = mapper;
        }

        public async Task<AddDishToMenuResult?> Handle(AddDishToMenuCommand request, CancellationToken cancellationToken)
        {
            var menu = await _menuRepository.GetMenuByIdAsync(request.MenuId);
            if (menu == null)
                return null;

            if (!string.IsNullOrWhiteSpace(request.DishDto.NewDescription))
            {
                menu.MenuDescription = request.DishDto.NewDescription;
            }

            if (request.DishDto.DishId <= 0)
                return null;

            var dish = await _dishRepository.GetDishByIdAsync(request.DishDto.DishId);
            if (dish == null)
                return null;

            bool alreadyExists = menu.MenuDishes.Any(md => md.DishId == request.DishDto.DishId);
            if (alreadyExists)
            {
                return new AddDishToMenuResult { DishAlreadyExists = true };
            }

            var menuDish = _mapper.Map<MenuDish>(request.DishDto);
            menuDish.Dish = dish;
            menuDish.Menu = menu;
            menuDish.MenuId = request.MenuId;
            menuDish.DishId = request.DishDto.DishId;

            menu.MenuDishes.Add(menuDish);

            await _menuRepository.UpdateMenuAsync(menu);

            return new AddDishToMenuResult { DishAlreadyExists = false };
        }
    }
}

/*using AutoMapper;
using MediatR;
using TalanLunch.Application.Interfaces;
using TalanLunch.Domain.Entities;
using TalanLunch.Application.Menus.Commands.AddDishToMenu;

namespace TalanLunch.Application.Menus.MenuHandlers
{
    public class AddDishToMenuCommandHandler : IRequestHandler<AddDishToMenuCommand, AddDishToMenuResult?>
    {
        private readonly IMenuRepository _menuRepository;
        private readonly IDishRepository _dishRepository;
        private readonly IMapper _mapper;

        public AddDishToMenuCommandHandler(
            IMenuRepository menuRepository,
            IDishRepository dishRepository,
            IMapper mapper)
        {
            _menuRepository = menuRepository;
            _dishRepository = dishRepository;
            _mapper = mapper;
        }

        public async Task<AddDishToMenuResult?> Handle(AddDishToMenuCommand request, CancellationToken cancellationToken)
        {
            var menu = await _menuRepository.GetMenuByIdAsync(request.MenuId);
            if (menu == null)
                return null;

            if (!string.IsNullOrWhiteSpace(request.NewDescription))
            {
                menu.MenuDescription = request.NewDescription;
            }

            var dish = await _dishRepository.GetDishByIdAsync(request.DishId);
            if (dish == null)
                return null;

            if (menu.MenuDishes.Any(md => md.DishId == request.DishId))
            {
                return new AddDishToMenuResult { DishAlreadyExists = true };
            }

            var menuDish = _mapper.Map<MenuDish>(request); 
            menuDish.Dish = dish;
            menuDish.Menu = menu;
            menuDish.MenuId = menu.MenuId;
            menuDish.DishId = dish.DishId;

            menu.MenuDishes.Add(menuDish);
            await _menuRepository.UpdateMenuAsync(menu);

            return new AddDishToMenuResult { DishAlreadyExists = false };
        }
    }
}
*/