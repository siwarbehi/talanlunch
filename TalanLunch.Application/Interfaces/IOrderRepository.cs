using TalanLunch.Domain.Entities;
namespace TalanLunch.Application.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order> AddOrderAsync(Order order);
        Task<List<Dish>> GetDishesByIdsAsync(List<int> dishIds);
       
        Task<(List<Order> Orders, int TotalItems)> GetAllOrdersAsync(int pageNumber, int pageSize, bool? isPaid, bool? isServed);
        Task<Order?> GetOrderByIdAsync(int orderId);
        Task UpdateOrderAsync(Order order);
        Task<MenuDish?> GetMenuDishAsync(int menuId, int dishId);
        IQueryable<Order> GetAllOrdersQuery();



    }
}


