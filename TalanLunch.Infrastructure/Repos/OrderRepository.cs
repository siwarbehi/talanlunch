using Microsoft.EntityFrameworkCore;
using TalanLunch.Application.Interfaces;
using TalanLunch.Domain.Entities;
using TalanLunch.Infrastructure.Data;
using TalanLunch.Application.Orders.Queries.GetAllOrders;


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


       
        public async Task<PagedResult<Order>> GetAllOrdersAsync(GetAllOrdersQuery query,CancellationToken cancellationToken)
        {
            var ordersQuery = _context.Orders
                .AsNoTracking()
                .Include(o => o.User)
                .Include(o => o.OrderDishes)
                    .ThenInclude(od => od.Dish)
                .AsQueryable();

            bool hasFirstName = !string.IsNullOrEmpty(query.FirstName);
            bool hasLastName = !string.IsNullOrEmpty(query.LastName);

            // 👉 Filtrage par FirstName si fourni
            if (hasFirstName)
            {
                var firstNameLower = query.FirstName.ToLower();
                ordersQuery = ordersQuery.Where(o =>
                    EF.Functions.Like(o.User.FirstName.ToLower(), $"{firstNameLower}%"));
            }

            // 👉 Filtrage par LastName si fourni
            if (hasLastName)
            {
                var lastNameLower = query.LastName.ToLower();
                ordersQuery = ordersQuery.Where(o =>
                    EF.Functions.Like(o.User.LastName.ToLower(), $"{lastNameLower}%"));
            }

            // 🧠 Si aucun nom/prénom fourni ET aucun filtre paid/served => commandes impayées non servies
            if (!hasFirstName && !hasLastName && !query.IsPaid.HasValue && !query.IsServed.HasValue)
            {
                ordersQuery = ordersQuery.Where(o => !o.Paid);
            }
            else
            {
                // ✅ Sinon, logique normale Paid + Served
                if (!query.IsPaid.HasValue && !query.IsServed.HasValue)
                {
                    ordersQuery = ordersQuery.Where(o => !o.Paid && !o.Served);
                }
                else if (!query.IsPaid.HasValue && query.IsServed == false)
                {
                    ordersQuery = ordersQuery.Where(o => !o.Served);
                }
                else if (query.IsPaid == false && query.IsServed == true)
                {
                    ordersQuery = ordersQuery.Where(o => !o.Paid && o.Served);
                }
                else if (query.IsPaid == true && query.IsServed == true)
                {
                    ordersQuery = ordersQuery.Where(o => o.Paid && o.Served);
                }
                else
                {
                    if (query.IsPaid.HasValue)
                        ordersQuery = ordersQuery.Where(o => o.Paid == query.IsPaid.Value);
                    if (query.IsServed.HasValue)
                        ordersQuery = ordersQuery.Where(o => o.Served == query.IsServed.Value);
                }
            }

            // 🔃 Tri par date décroissante
            ordersQuery = ordersQuery.OrderByDescending(o => o.OrderDate);

            // 📊 Résultat paginé
            var totalItems = await CountOrdersAsync(ordersQuery, cancellationToken);
            var pagedItems = await GetPagedOrdersAsync(ordersQuery, query.PageNumber, query.PageSize, cancellationToken);

            return new PagedResult<Order>
            {
                Items = pagedItems,
                TotalItems = totalItems,
                PageNumber = query.PageNumber,
                PageSize = query.PageSize
            };
        }


        public async Task<int> CountOrdersAsync(IQueryable<Order> ordersQuery, CancellationToken cancellationToken)
        {
            return await ordersQuery.CountAsync(cancellationToken);
        }

        public async Task<List<Order>> GetPagedOrdersAsync(IQueryable<Order> ordersQuery, int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            return await ordersQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);
        }


    }
}
