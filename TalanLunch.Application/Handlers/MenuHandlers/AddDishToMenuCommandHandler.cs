using AutoMapper;
using MediatR;
using TalanLunch.Application.Commands.Menu;
using TalanLunch.Application.Dtos.Menu;
using TalanLunch.Application.Interfaces;
using TalanLunch.Domain.Entities;

namespace TalanLunch.Application.Handlers.MenuHandlers
{
    public class AddDishToMenuCommandHandler : IRequestHandler<AddDishToMenuCommand, AddDishToMenuResultDto?>
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
        public async Task<AddDishToMenuResultDto?> Handle(AddDishToMenuCommand request, CancellationToken cancellationToken)
    {
        var menu = await _menuRepository.GetMenuByIdAsync(request.MenuId);
        if (menu == null)
            return null;

        if (!string.IsNullOrWhiteSpace(request.DishDto.NewDescription))
        {
            menu.MenuDescription = request.DishDto.NewDescription;
        }

        if (request.DishDto.DishId > 0)
        {
            var dish = await _dishRepository.GetDishByIdAsync(request.DishDto.DishId);
            if (dish == null)
                return null;

            if (menu.MenuDishes.Any(md => md.DishId == request.DishDto.DishId))
            {
                return new AddDishToMenuResultDto { DishAlreadyExists = true };
            }

            var menuDish = _mapper.Map<MenuDish>(request.DishDto);
            menuDish.Dish = dish;
            menuDish.Menu = menu;
            menuDish.MenuId = request.MenuId;
            menuDish.DishId = request.DishDto.DishId;

            menu.MenuDishes.Add(menuDish);
        }

        await _menuRepository.UpdateMenuAsync(menu);

        return new AddDishToMenuResultDto { DishAlreadyExists = false };
    }
}
}
