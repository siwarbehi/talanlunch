/*using MediatR;
using TalanLunch.Application.Dtos.Order;

namespace TalanLunch.Application.Queries.Order
{
   
    public class GetAllOrdersQuery : IRequest<IEnumerable<OrderDayDto>>
    {
    }
}
*//*
using MediatR;
using TalanLunch.Application.Dtos.Order;

public class GetAllOrdersQuery : IRequest<IEnumerable<OrderDayDto>>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public bool OnlyUnpaid { get; set; }
    public bool OnlyPaidAndServed { get; set; }

    public GetAllOrdersQuery(int pageNumber, int pageSize, bool onlyUnpaid, bool onlyPaidAndServed)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        OnlyUnpaid = onlyUnpaid;
        OnlyPaidAndServed = onlyPaidAndServed;
    }
}

*//*
using MediatR;
using TalanLunch.Application.Dtos.Menu;
using TalanLunch.Application.Dtos.Order;

public class GetAllOrdersQuery : IRequest<PagedResult<OrderDayDto>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public bool? IsPaid { get; set; }
    public bool? IsServed { get; set; }

    public GetAllOrdersQuery() { }
}
*/