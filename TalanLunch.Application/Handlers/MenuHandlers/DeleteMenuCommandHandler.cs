using MediatR;
using TalanLunch.Application.Commands.Menu;
using TalanLunch.Application.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace TalanLunch.Application.Handlers.MenuHandlers
{
    // On précise Unit comme type de retour
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
