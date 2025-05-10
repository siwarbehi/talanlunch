using MediatR;

namespace TalanLunch.Application.Menus.Queries.GetAllMenus
{
    public class GetAllMenusQuery : IRequest<IEnumerable<GetAllMenusQueryResult>>
    {
    }
}
