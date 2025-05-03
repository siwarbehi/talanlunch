using MediatR;
using TalanLunch.Application.Commands.Menu;
using TalanLunch.Application.Interfaces;

namespace TalanLunch.Application.Handlers.MenuHandlers
{
    
    public class SetMenuOfTheDayCommandHandler
        : IRequestHandler<SetMenuOfTheDayCommand, bool>
    {
        private readonly IMenuRepository _menuRepository;

        public SetMenuOfTheDayCommandHandler(IMenuRepository menuRepository)
        {
            _menuRepository = menuRepository;
        }

        public async Task<bool> Handle(
            SetMenuOfTheDayCommand request,
            CancellationToken cancellationToken)
        {
            // 1. Récupère le menu ciblé
            var menu = await _menuRepository.GetMenuByIdAsync(request.MenuId);
            if (menu == null)
                return false;

            // 2. Trouve l'ancien menu du jour et le désactive
            var allMenus = await _menuRepository.GetAllMenusAsync();
            var previous = allMenus.FirstOrDefault(m => m.IsMenuOfTheDay);
            if (previous != null && previous.MenuId != request.MenuId)
            {
                previous.IsMenuOfTheDay = false;
                await _menuRepository.UpdateMenuAsync(previous);
            }

            // 3. Active le nouveau menu du jour
            menu.IsMenuOfTheDay = true;
            await _menuRepository.UpdateMenuAsync(menu);

            return true;
        }
    }
}
