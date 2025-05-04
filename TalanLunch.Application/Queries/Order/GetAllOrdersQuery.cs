// Application/Queries/Order/GetAllOrdersQuery.cs
using MediatR;
using TalanLunch.Application.Dtos.Order;

namespace TalanLunch.Application.Queries.Order
{
   
    public class GetAllOrdersQuery : IRequest<IEnumerable<OrderDayDto>>
    {
    }
}
