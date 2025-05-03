using MediatR;
using TalanLunch.Application.Queries.Menu;
using TalanLunch.Application.Interfaces;
using TalanLunch.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace TalanLunch.Application.Handlers.MenuHandlers
{
    // Gère la Query et renvoie l'entité Menu (ou null si introuvable)
    public class GetMenuByIdQueryHandler : IRequestHandler<GetMenuByIdQuery, Menu?>
    {
        private readonly IMenuRepository _menuRepository;

        public GetMenuByIdQueryHandler(IMenuRepository menuRepository)
        {
            _menuRepository = menuRepository;
        }

        public async Task<Menu?> Handle(GetMenuByIdQuery request, CancellationToken cancellationToken)
        {
            return await _menuRepository.GetMenuByIdAsync(request.MenuId);
        }
    }
}
