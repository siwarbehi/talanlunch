using TalanLunch.Application.Dtos;
using TalanLunch.Domain.Entities;

namespace TalanLunch.Application.Interfaces
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(OrderRequestDto request);
        Task<List<OrderDayDto>> GetAllOrdersAsync();

        Task<bool> UpdateOrderStatusAsync(UpdateOrderStatusDto dto);
        Task<PagedResult<OrderDayDto>> GetPaginatedOrdersAsync(PaginationQuery query);


    }
}
