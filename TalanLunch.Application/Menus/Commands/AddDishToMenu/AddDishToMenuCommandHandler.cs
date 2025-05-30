using MediatR;
using TalanLunch.Application.Interfaces;
using TalanLunch.Application.Menus.Commands.AddDishToMenu;
using TalanLunch.Domain.Entities;



namespace TalanLunch.Application.Menus.MenuHandlers
{
    public class AddDishToMenuCommandHandler : IRequestHandler<AddDishToMenuCommand, AddDishToMenuCommandResult>
    {
        private readonly IMenuRepository _menuRepository;
        private readonly IDishRepository _dishRepository;

        public AddDishToMenuCommandHandler(IMenuRepository menuRepository, IDishRepository dishRepository)
        {
            _menuRepository = menuRepository;
            _dishRepository = dishRepository;
        }

        public async Task<AddDishToMenuCommandResult> Handle(AddDishToMenuCommand request, CancellationToken cancellationToken)
        {
            var menu = await _menuRepository.GetMenuByIdAsync(request.MenuId)
                .ConfigureAwait(false);

            if (menu == null)
            {
                return new AddDishToMenuCommandResult { Succeeded = false, Error = $"Menu with ID {request.MenuId} not found." };
            }

            bool dishExistsInMenu = menu.MenuDishes.Any(md => md.DishId == request.DishId);

            if (dishExistsInMenu)
            {
                return new AddDishToMenuCommandResult { Succeeded = false, Error = $"Dish with ID {request.DishId} already exists in the menu." };
            }

            if (request.DishId != null)
            {
                var dish = await _dishRepository.GetDishByIdAsync(request.DishId)
                    .ConfigureAwait(false);

                if (dish == null)
                {
                    return new AddDishToMenuCommandResult { Succeeded = false, Error = $"Dish with ID {request.DishId} not found." };
                }

                menu.MenuDishes.Add(new MenuDish { DishId = request.DishId, MenuId = request.MenuId, DishQuantity = request.Quantity });

            }

            if (!string.IsNullOrWhiteSpace(request.Description))
            {
                menu.MenuDescription = request.Description;
            }

            await _menuRepository.UpdateMenuAsync(menu)
                .ConfigureAwait(false);

            return new AddDishToMenuCommandResult { Succeeded = true };
        }
    }
}