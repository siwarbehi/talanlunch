using MediatR;
using System.Collections.Generic;
using TalanLunch.Application.Dtos.Menu;

namespace TalanLunch.Application.Queries.Menu
{
   
    public class GetAllMenusQuery : IRequest<IEnumerable<GetAllMenusDto>>
    {
    }
}
