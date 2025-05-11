using MediatR;
using TalanLunch.Application.Interfaces;

namespace TalanLunch.Application.Menus.Commands.SetMenuOfTheDayCommand
{
    public class SetMenuOfTheDayCommandHandler : IRequestHandler<SetMenuOfTheDayCommand, bool>
    {
        private readonly IMenuRepository _menuRepository;

        public SetMenuOfTheDayCommandHandler(IMenuRepository menuRepository)
        {
            _menuRepository = menuRepository;
        }

        public async Task<bool> Handle(SetMenuOfTheDayCommand request, CancellationToken cancellationToken)
        {
            // 1. Vérifie si le menu demandé existe
            var menu = await _menuRepository.GetMenuByIdAsync(request.MenuId);
            if (menu == null)
                return false;

            // 2. Désactive le menu précédemment défini comme "du jour"
            var currentMenuOfTheDay = (await _menuRepository.GetAllMenusAsync())
                .FirstOrDefault(m => m.IsMenuOfTheDay && m.MenuId != request.MenuId);

            if (currentMenuOfTheDay != null)
            {
                currentMenuOfTheDay.IsMenuOfTheDay = false;
                await _menuRepository.UpdateMenuAsync(currentMenuOfTheDay);
            }

            // 3. Active le nouveau menu du jour
            if (!menu.IsMenuOfTheDay)
            {
                menu.IsMenuOfTheDay = true;
                await _menuRepository.UpdateMenuAsync(menu);
            }

            return true;
        }
    }
}
