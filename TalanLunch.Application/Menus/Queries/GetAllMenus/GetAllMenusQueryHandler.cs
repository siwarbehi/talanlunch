using AutoMapper;
using MediatR;
using TalanLunch.Application.Interfaces;
using TalanLunch.Application.Menus.Queries.GetAllMenus;

namespace TalanLunch.Application.Menus.MenuHandlers
{
    public class GetAllMenusQueryHandler: IRequestHandler<GetAllMenusQuery, IEnumerable<GetAllMenusQueryResult>>
    {
        private readonly IMenuRepository _menuRepository;
        private readonly IMapper _mapper;

        public GetAllMenusQueryHandler(IMenuRepository menuRepository, IMapper mapper)
        {
            _menuRepository = menuRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetAllMenusQueryResult>> Handle(
            GetAllMenusQuery request,
            CancellationToken cancellationToken)
        {
            var menus = await _menuRepository.GetAllMenusAsync();
            return _mapper.Map<IEnumerable<GetAllMenusQueryResult>>(menus);
        }
    }
}
