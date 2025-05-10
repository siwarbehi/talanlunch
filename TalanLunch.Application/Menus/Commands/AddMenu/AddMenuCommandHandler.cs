using AutoMapper;
using MediatR;
using TalanLunch.Application.Interfaces;
using TalanLunch.Domain.Entities;

namespace TalanLunch.Application.Menus.Commands.AddMenu
{
    public class AddMenuCommandHandler : IRequestHandler<AddMenuCommand, Menu>
    {
        private readonly IMenuRepository _menuRepository;
        private readonly IDishRepository _dishRepository;
        private readonly IMapper _mapper;

        public AddMenuCommandHandler(IMenuRepository menuRepository, IDishRepository dishRepository, IMapper mapper)
        {
            _menuRepository = menuRepository;
            _dishRepository = dishRepository;
            _mapper = mapper;
        }

        public async Task<Menu> Handle(AddMenuCommand request, CancellationToken cancellationToken)
        {
            if (request.Dishes == null || !request.Dishes.Any())
                throw new ArgumentException("Le menu doit contenir au moins un plat.");

            var dishIds = request.Dishes.Select(d => d.DishId).ToList();
            var dishes = await _dishRepository.GetDishesByIdsAsync(dishIds);

            var dishesById = dishes.ToDictionary(d => d.DishId);
            var invalidDishIds = dishIds.Except(dishesById.Keys).ToList();

            if (invalidDishIds.Any())
                throw new ArgumentException($"Plats invalides : {string.Join(", ", invalidDishIds)}");

            // Map basic menu (sans les MenuDishes)
            var newMenu = _mapper.Map<Menu>(request);
            newMenu.MenuDate = DateTime.Now;

            // Ajouter les MenuDishes après validation des Dish
            newMenu.MenuDishes = request.Dishes
                .Where(d => dishesById.ContainsKey(d.DishId))
                .Select(d =>
                {
                    var menuDish = _mapper.Map<MenuDish>(d);
                    menuDish.Dish = dishesById[d.DishId];
                    return menuDish;
                }).ToList();

            return await _menuRepository.AddMenuAsync(newMenu);
        }
    }
}
