// Fichier : Application/Handlers/MenuHandlers/GetAllMenusQueryHandler.cs
using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TalanLunch.Application.Dtos.Menu;
using TalanLunch.Application.Interfaces;
using TalanLunch.Application.Queries.Menu;

namespace TalanLunch.Application.Handlers.MenuHandlers
{

    public class GetAllMenusQueryHandler
        : IRequestHandler<GetAllMenusQuery, IEnumerable<GetAllMenusDto>>
    {
        private readonly IMenuRepository _menuRepository;
        private readonly IMapper _mapper;

        public GetAllMenusQueryHandler(IMenuRepository menuRepository, IMapper mapper)
        {
            _menuRepository = menuRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetAllMenusDto>> Handle(
            GetAllMenusQuery request,
            CancellationToken cancellationToken)
        {
            // Récupère les entités
            var menus = await _menuRepository.GetAllMenusAsync();
            // Mappe en DTOs
            return _mapper.Map<IEnumerable<GetAllMenusDto>>(menus);
        }
    }
}
