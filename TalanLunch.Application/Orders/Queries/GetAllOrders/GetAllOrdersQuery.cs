using MediatR;

namespace TalanLunch.Application.Orders.Queries.GetAllOrders
{
    public class GetAllOrdersQuery : IRequest<PagedResult<OrderDay>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
   
        public bool? IsPaid { get; set; }
        public bool? IsServed { get; set; }
       
    }
}
