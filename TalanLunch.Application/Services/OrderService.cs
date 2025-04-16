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

            decimal totalAmount = 0;
            var orderDishes = new List<OrderDish>();

            foreach (var item in request.Dishes)
            {
                var dish = dishes.FirstOrDefault(d => d.DishId == item.DishId);
                if (dish == null)
                    throw new Exception($"Plat avec ID {item.DishId} non trouvé");

                totalAmount += dish.DishPrice * item.Quantity;

                orderDishes.Add(new OrderDish
                {
                    DishId = item.DishId,
                    Dish = dish,
                    Quantity = item.Quantity,
                    Order = null!
                });
            }

            var newOrder = new Order
            {
                User = user,
                TotalAmount = totalAmount,
                OrderDate = DateTime.Now,
                Paid = false,
                Served = false,
                OrderRemark = request.OrderRemark,
                OrderDishes = orderDishes

            };

            return await _orderRepository.AddOrderAsync(newOrder);
        }


    }
}
