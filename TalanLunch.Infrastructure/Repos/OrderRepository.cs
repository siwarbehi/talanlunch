using Microsoft.EntityFrameworkCore;
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
       

        public async Task<(List<Order> Orders, int TotalItems)> GetAllOrdersAsync(int pageNumber, int pageSize, bool? isPaid, bool? isServed)
        {
            var query = _context.Orders
                .AsNoTracking()
                .Include(o => o.User)
                .Include(o => o.OrderDishes)
                    .ThenInclude(od => od.Dish)
                .AsQueryable();

            if (!isPaid.HasValue && !isServed.HasValue)
            {
                query = query.Where(o => !o.Paid && !o.Served);
            }
            else if (!isPaid.HasValue && isServed == false)
            {
                query = query.Where(o => !o.Served);
            }
            else if (isPaid == false && isServed == true)
            {
                query = query.Where(o => !o.Paid && o.Served);
            }
            else if (isPaid == true && isServed == true)
            {
                query = query.Where(o => o.Paid && o.Served);
            }
            else
            {
                if (isPaid.HasValue)
                    query = query.Where(o => o.Paid == isPaid.Value);
                if (isServed.HasValue)
                    query = query.Where(o => o.Served == isServed.Value);
            }

            var totalItems = await query.CountAsync();

            var orders = await query
                .OrderByDescending(o => o.OrderDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (orders, totalItems);
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


        public IQueryable<Order> GetAllOrdersQuery()
        {
            return _context.Orders.AsQueryable(); // on retourne juste la requete
        }


    }
}
