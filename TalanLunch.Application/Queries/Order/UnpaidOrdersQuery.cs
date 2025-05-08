using MediatR;
using TalanLunch.Application.Dtos.Order;
using TalanLunch.Application.Dtos.Menu;

namespace TalanLunch.Application.Orders.Queries
{
    public class UnpaidOrdersQuery : IRequest<PagedResult<OrderDayDto>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}
