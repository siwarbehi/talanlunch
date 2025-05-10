using MediatR;
using TalanLunch.Domain.Entities;

namespace TalanLunch.Application.Menus.Queries.GetMenuById
{
    public class GetMenuByIdQuery : IRequest<Menu>
    {
        public int MenuId { get; set; }

       
    }
}
