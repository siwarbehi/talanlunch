using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalanLunch.Application.Interfaces;
using TalanLunch.Domain.Entities;
using TalanLunch.Infrastructure.Data;

namespace TalanLunch.Infrastructure.Repos
{
    public class OrderRepository : IOrderRepository
    {
        private readonly TalanLunchDbContext _context;

        public OrderRepository(TalanLunchDbContext context)
        {
            _context = context;
        }
        public async Task<Order> AddOrderAsync(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<List<Dish>> GetDishesByIdsAsync(List<int> dishIds)
        {
            return await _context.Dishes
                .Where(d => dishIds.Contains(d.DishId))
                .ToListAsync();
        }
        /* public async Task<List<Order>> GetOrdersByDateAsync(DateTime date)
         {
             return await _context.Orders
                 .Include(o => o.User)
                 .Include(o => o.OrderDishes)
                     .ThenInclude(od => od.Dish)
                 .Where(o => o.OrderDate.Date == date.Date)
                 .ToListAsync();
         }
        */
        public async Task<List<Order>> GetAllOrdersAsync()
        {
            return await _context.Orders
                .AsNoTracking()
                .Include(o => o.User)
                .Include(o => o.OrderDishes)
                    .ThenInclude(od => od.Dish)
                .ToListAsync();
        }

        public async Task<Order?> GetOrderByIdAsync(int orderId)
        {
            return await _context.Orders.FindAsync(orderId);
        }

        public async Task UpdateOrderAsync(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }
        public async Task<MenuDish?> GetMenuDishAsync(int menuId, int dishId)
        {
            return await _context.MenuDishes
                .FirstOrDefaultAsync(md => md.MenuId == menuId && md.DishId == dishId);
        }




    }
}
