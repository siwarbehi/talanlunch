using MediatR;
using TalanLunch.Application.Interfaces;
using TalanLunch.Domain.Entities;

namespace TalanLunch.Application.Menus.Queries.GetMenuById
{
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
