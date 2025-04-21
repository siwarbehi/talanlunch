using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalanLunch.Application.Dtos;
using TalanLunch.Domain.Entities;
namespace TalanLunch.Application.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order> AddOrderAsync(Order order);
        Task<List<Dish>> GetDishesByIdsAsync(List<int> dishIds);
        Task<List<Order>> GetOrdersByDateAsync(DateTime date);
        Task<Order?> GetOrderByIdAsync(int orderId);
        Task UpdateOrderAsync(Order order);
        Task<MenuDish?> GetMenuDishAsync(int menuId, int dishId);


    }
}


