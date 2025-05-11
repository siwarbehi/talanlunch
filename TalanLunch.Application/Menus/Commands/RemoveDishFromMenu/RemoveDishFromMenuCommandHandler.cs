using MediatR;
using TalanLunch.Application.Interfaces;
using TalanLunch.Domain.Entities;

namespace TalanLunch.Application.Menus.Commands.RemoveDishFromMenu
{
    public class RemoveDishFromMenuCommandHandler : IRequestHandler<RemoveDishFromMenuCommand, Menu?>
    {
        private readonly IMenuRepository _menuRepository;
        private readonly IDishRepository _dishRepository;

        public RemoveDishFromMenuCommandHandler(IMenuRepository menuRepository, IDishRepository dishRepository)
        {
            _menuRepository = menuRepository;
            _dishRepository = dishRepository;
        }

        public async Task<Menu?> Handle(RemoveDishFromMenuCommand request, CancellationToken cancellationToken)
        {
            var menu = await _menuRepository.GetMenuByIdAsync(request.MenuId);
            var dish = await _dishRepository.GetDishByIdAsync(request.DishId);

            if (menu == null || dish == null) return null;

            var menuDish = menu.MenuDishes.FirstOrDefault(md => md.DishId == request.DishId);
            if (menuDish != null)
            {
                menu.MenuDishes.Remove(menuDish);
            }

            return await _menuRepository.UpdateMenuAsync(menu);
        }
    }
}
