// Application/Handlers/OrderHandlers/PlaceOrderCommandHandler.cs
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TalanLunch.Application.Commands.Order;
using TalanLunch.Application.Dtos.Order;
using TalanLunch.Application.Interfaces;
using TalanLunch.Domain.Entities;

namespace TalanLunch.Application.Handlers.OrderHandlers
{
   
    public class PlaceOrderCommandHandler : IRequestHandler<PlaceOrderCommand, Order>
    {
        private readonly IUserRepository _userRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public PlaceOrderCommandHandler(
            IUserRepository userRepository,
            IOrderRepository orderRepository,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task<Order> Handle(
            PlaceOrderCommand request,
            CancellationToken cancellationToken)
        {
            var dto = request.Request;

            // 1. Valide l'utilisateur
            var user = await _userRepository.GetUserByIdAsync(dto.UserId)
                       ?? throw new Exception("Utilisateur non trouvé");

            // 2. Charge les plats
            var dishIds = dto.Dishes.Select(d => d.DishId).ToList();
            var dishes = await _orderRepository.GetDishesByIdsAsync(dishIds);
            if (!dishes.Any())
                throw new Exception("Aucun plat valide sélectionné");

            // 3. Initialise la commande
            var newOrder = new Order
            {
                User = user,
                OrderDate = DateTime.Now,
                Paid = false,
                Served = false,
                OrderRemark = dto.OrderRemark,
                TotalAmount = 0,
                OrderDishes = new List<OrderDish>()
            };

            // 4. Pour chaque plat, vérifie disponibilité puis ajoute

            foreach (var item in dto.Dishes)
            {
                var dish = dishes.FirstOrDefault(d => d.DishId == item.DishId)
                                ?? throw new Exception($"Plat ID {item.DishId} non trouvé");
                var menuDish = await _orderRepository.GetMenuDishAsync(dto.MenuId, item.DishId)
                               ?? throw new Exception($"Le plat '{dish.DishName}' n'est pas dans ce menu");
                if (menuDish.DishQuantity < item.Quantity)
                    throw new Exception($"Stock insuffisant pour '{dish.DishName}'");

                menuDish.DishQuantity -= item.Quantity;

                var orderDish = new OrderDish
                {
                    DishId = item.DishId,
                    Quantity = item.Quantity
                };
                newOrder.OrderDishes.Add(orderDish);

                // Correction : DishPrice est decimal? → on fournit 0m si null
                decimal price = dish.DishPrice ?? 0m;
                newOrder.TotalAmount += price * item.Quantity;
            }

            // 5. Persistance
            return await _orderRepository.AddOrderAsync(newOrder);
        }
    }
}
