using TalanLunch.Domain.Entities;
using TalanLunch.Application.Orders.Queries.GetAllOrders;



namespace TalanLunch.Application.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order> AddOrderAsync(Order order);
        Task<List<Dish>> GetDishesByIdsAsync(List<int> dishIds);
       
        Task<Order?> GetOrderByIdAsync(int orderId);
        Task UpdateOrderAsync(Order order);
        Task<MenuDish?> GetMenuDishAsync(int menuId, int dishId);
       Task<PagedResult<Order>> GetAllOrdersAsync(GetAllOrdersQuery query,CancellationToken cancellationToken);
       }


}



