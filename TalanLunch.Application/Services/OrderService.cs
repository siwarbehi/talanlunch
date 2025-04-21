using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalanLunch.Application.Dtos;
using TalanLunch.Application.Interfaces;
using TalanLunch.Domain.Entities;


namespace TalanLunch.Application.Services
{
    public class OrderService : IOrderService

    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUserRepository _userRepository;
        public OrderService(IOrderRepository orderRepository, IUserRepository userRepository)
        {
            _orderRepository = orderRepository;
            _userRepository = userRepository;
        }
        public async Task<Order> CreateOrderAsync(OrderRequestDto request)
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
                newOrder.TotalAmount += dish.DishPrice * item.Quantity;
            }


            return await _orderRepository.AddOrderAsync(newOrder);
        }

        public async Task<List<OrderDayDto>> GetOrdersByDateAsync(DateTime date)
        {
            var orders = await _orderRepository.GetOrdersByDateAsync(date);

            return orders.Select(o => new OrderDayDto
            {
                FirstName = o.User.FirstName,
                LastName = o.User.LastName,
                ProfilePicture = o.User.ProfilePicture,
                OrderId = o.OrderId,
                OrderRemark = o.OrderRemark,
                TotalAmount = o.TotalAmount,
                Paid = o.Paid,
                Served = o.Served,
                OrderDate = o.OrderDate,
                Dishes = o.OrderDishes.Select(od => new DishOrderDto
                {
                    DishName = od.Dish.DishName,
                    Quantity = od.Quantity
                }).ToList()
            }).ToList();
        }

        public async Task<bool> UpdateOrderStatusAsync(UpdateOrderStatusDto dto)
        {
            var order = await _orderRepository.GetOrderByIdAsync(dto.OrderId);
            if (order == null) return false;

            if (dto.Paid.HasValue)
                order.Paid = dto.Paid.Value;

            if (dto.Served.HasValue)
                order.Served = dto.Served.Value;

            await _orderRepository.UpdateOrderAsync(order);
            return true;
        }



    }
}
