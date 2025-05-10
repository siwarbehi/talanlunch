using MediatR;
using TalanLunch.Application.Interfaces;
using TalanLunch.Application.Menus.Commands.DeleteMenu;


namespace TalanLunch.Application.Menus.MenuHandlers
{
    public class DeleteMenuCommandHandler : IRequestHandler<DeleteMenuCommand, Unit>
    {
        private readonly IMenuRepository _menuRepository;

        public DeleteMenuCommandHandler(IMenuRepository menuRepository)
        {
            _menuRepository = menuRepository;
        }

        public async Task<Unit> Handle(DeleteMenuCommand request, CancellationToken cancellationToken)
        {
            var menu = await _menuRepository.GetMenuByIdAsync(request.MenuId);

            if (menu != null)
            {
                await _menuRepository.DeleteMenuAsync(request.MenuId);
            }

            return Unit.Value;
        }
    }
}
