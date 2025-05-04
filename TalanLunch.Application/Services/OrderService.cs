using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using TalanLunch.Application.Interfaces;
using TalanLunch.Domain.Entities;
using talanlunch.Application.Hubs;
using TalanLunch.Application.Dtos.Order;
using TalanLunch.Application.Dtos.Menu;
using AutoMapper;

namespace TalanLunch.Application.Services
{
    public class OrderService : IOrderService

    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUserRepository _userRepository;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IMapper _mapper;

        public OrderService(IOrderRepository orderRepository, IUserRepository userRepository, IHubContext<NotificationHub> hubContext  ,IMapper mapper)
        {
            _orderRepository = orderRepository;
            _userRepository = userRepository;
            _hubContext = hubContext;
            _mapper = mapper;

        }
        /*public async Task<Order> CreateOrderAsync(OrderRequestDto request)
        {
            var user = await _userRepository.GetUserByIdAsync(request.UserId);
            if (user == null)
                throw new Exception("Utilisateur non trouvé");

            var dishIds = request.Dishes.Select(d => d.DishId).ToList();
            var dishes = await _orderRepository.GetDishesByIdsAsync(dishIds);
            if (!dishes.Any())
                throw new Exception("Aucun plat valide sélectionné");

            var newOrder = new Order
            {
                User = user,
                TotalAmount = 0, 
                OrderDate = DateTime.Now,
                Paid = false,
                Served = false,
                OrderRemark = request.OrderRemark,
                OrderDishes = new List<OrderDish>() 
            };

            foreach (var item in request.Dishes)
            {
                var dish = dishes.FirstOrDefault(d => d.DishId == item.DishId);
                if (dish == null)
                    throw new Exception($"Plat avec ID {item.DishId} non trouvé");

                var menuDish = await _orderRepository.GetMenuDishAsync(request.MenuId, item.DishId);
                if (menuDish == null)
                    throw new Exception($"Le plat '{dish.DishName}' n'est pas disponible dans ce menu.");

                if (menuDish.DishQuantity < item.Quantity)
                    throw new Exception($"Stock insuffisant pour le plat '{dish.DishName}'. Disponible : {menuDish.DishQuantity}");

                // Décrémenter la quantité
                menuDish.DishQuantity -= item.Quantity;

                var orderDish = new OrderDish
                {
                    DishId = item.DishId,
                    Quantity = item.Quantity,
                };

                newOrder.OrderDishes.Add(orderDish);
                newOrder.TotalAmount += (dish.DishPrice ?? 0) * item.Quantity;
            }


            return await _orderRepository.AddOrderAsync(newOrder);
        }*/


      /*  public async Task<List<OrderDayDto>> GetAllOrdersAsync()
        {
            var orders = await _orderRepository.GetAllOrdersAsync();
            var orderDtos = _mapper.Map<List<OrderDayDto>>(orders); 
            return orderDtos;
        }
*/


       /* public async Task<bool> UpdateOrderStatusAsync(UpdateOrderStatusDto dto)
        {
            // Récupération de la commande par ID
            var order = await _orderRepository.GetOrderByIdAsync(dto.OrderId);
            if (order == null) return false; // Si la commande n'existe pas

            bool paidChangedToTrue = false;
            bool servedChangedToTrue = false;

            // Vérification et mise à jour de l'état "Paid"
            if (dto.Paid.HasValue && dto.Paid.Value && !order.Paid)
            {
                order.Paid = true;
                paidChangedToTrue = true;
            }

            // Vérification et mise à jour de l'état "Served"
            if (dto.Served.HasValue && dto.Served.Value && !order.Served)
            {
                order.Served = true;
                servedChangedToTrue = true;
            }

            // Mise à jour de la commande dans la base de données
            await _orderRepository.UpdateOrderAsync(order);

            // Si l'état de la commande a changé, envoyer une notification à l'utilisateur
            if ((paidChangedToTrue || servedChangedToTrue) && order?.UserId != null)
            {
                var notificationMessage = "";

                // Construction du message de notification
                if(paidChangedToTrue)
    notificationMessage += $"Félicitations ! Votre commande a été payée avec succès. Merci de votre achat.\n";

                if (servedChangedToTrue)
                    notificationMessage += $"Votre commande a été servie ! Bon appétit !\n";


                // Envoi de la notification via SignalR
                await _hubContext.Clients.Group(order.UserId.ToString())
                    .SendAsync("ReceiveNotification", new
                    {
                        OrderId = order.OrderId,
                        Message = notificationMessage.Trim(),
                        Type = "status-update",
                        Timestamp = DateTime.UtcNow
                    });
            }

            return true; // Retourne true si la mise à jour a réussi
        }
    
    */

        public async Task<PagedResult<OrderDayDto>> GetPaginatedOrdersAsync(PaginationQuery query)
        {
            var ordersQuery = _orderRepository.GetAllOrdersQuery();

            ordersQuery = ordersQuery.Include(o => o.User);
            ordersQuery = ordersQuery.Include(o => o.OrderDishes)
                                     .ThenInclude(od => od.Dish);

            // Filtrer par prénom et/ou nom si fournis
            if (!string.IsNullOrEmpty(query.FirstName))
            {
                ordersQuery = ordersQuery.Where(o => o.User.FirstName.Contains(query.FirstName));
            }

            if (!string.IsNullOrEmpty(query.LastName))
            {
                ordersQuery = ordersQuery.Where(o => o.User.LastName.Contains(query.LastName));
            }

            // Tri décroissant par date
            var orderedQuery = ordersQuery.OrderByDescending(o => o.OrderDate);

            var totalItems = await orderedQuery.CountAsync();

            var pagedOrders = await orderedQuery
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();

            var result = pagedOrders.Select(o => new OrderDayDto
            {
                FirstName = o.User?.FirstName ?? "",
                LastName = o.User?.LastName ?? "",
                ProfilePicture = o.User?.ProfilePicture,
                OrderId = o.OrderId,
                TotalAmount = o.TotalAmount,
                Paid = o.Paid,
                OrderDate = o.OrderDate,
                Dishes = o.OrderDishes.Select(od => new DishOrderDto
                {
                    DishName = od.Dish?.DishName ?? "",
                    DishId = od.Dish?.DishId ?? 0,
                    Quantity = od.Quantity
                }).ToList()
            }).ToList();

            return new PagedResult<OrderDayDto>
            {
                Items = result,
                TotalItems = totalItems,
                PageNumber = query.PageNumber,
                PageSize = query.PageSize
            };
        }



    }
}
