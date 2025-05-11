using AutoMapper;
using MediatR;
using TalanLunch.Application.Interfaces;
using TalanLunch.Domain.Entities;

namespace TalanLunch.Application.Orders.Commands.PlaceOrder
{
    public class PlaceOrderCommandHandler : IRequestHandler<PlaceOrderCommand, Order>
    {
        private readonly IUserRepository _userRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IMenuRepository _menuRepository;
        private readonly IMapper _mapper;

        public PlaceOrderCommandHandler(
            IUserRepository userRepository,
            IOrderRepository orderRepository,
            IMenuRepository menuRepository,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _orderRepository = orderRepository;
            _menuRepository = menuRepository;
            _mapper = mapper;
        }

        public async Task<Order> Handle(PlaceOrderCommand request, CancellationToken cancellationToken)
        {
            // 1. Validation de base
            if (request.Dishes == null || !request.Dishes.Any())
                throw new Exception("La commande ne contient aucun plat.");

            var user = await _userRepository.GetUserByIdAsync(request.UserId)
                       ?? throw new Exception("Utilisateur non trouvé.");

            var dishIds = request.Dishes.Select(d => d.DishId).Distinct().ToList();
            var dishes = await _orderRepository.GetDishesByIdsAsync(dishIds);

            var menu = await _menuRepository.GetMenuByIdAsync(request.MenuId)
                       ?? throw new Exception("Menu non trouvé.");

            var dishDict = dishes.ToDictionary(d => d.DishId);
            var menuDishDict = menu.MenuDishes.ToDictionary(md => md.DishId);

            var newOrder = _mapper.Map<Order>(request); 

            newOrder.User = user; 
            newOrder.OrderDate = DateTime.UtcNow;
            newOrder.Paid = false;
            newOrder.Served = false;
            newOrder.TotalAmount = 0;
            newOrder.OrderDishes = new List<OrderDish>();

            // 3. Traitement de chaque plat
            foreach (var item in request.Dishes)
            {
                if (!dishDict.TryGetValue(item.DishId, out var dish))
                    throw new Exception($"Plat ID {item.DishId} non trouvé.");

                if (!menuDishDict.TryGetValue(item.DishId, out var menuDish))
                    throw new Exception($"Le plat '{dish.DishName}' n'est pas dans le menu sélectionné.");

                if (menuDish.DishQuantity < item.Quantity)
                    throw new Exception($"Stock insuffisant pour '{dish.DishName}' (stock : {menuDish.DishQuantity}, demandé : {item.Quantity}).");

                // Mise à jour du stock
                menuDish.DishQuantity -= item.Quantity;

                newOrder.OrderDishes.Add(new OrderDish
                {
                    DishId = item.DishId,
                    Quantity = item.Quantity
                });

                decimal price = dish.DishPrice ?? 0m;
                newOrder.TotalAmount += price * item.Quantity;
            }

            // 4. Sauvegarde
            return await _orderRepository.AddOrderAsync(newOrder);
        }
    }
}
