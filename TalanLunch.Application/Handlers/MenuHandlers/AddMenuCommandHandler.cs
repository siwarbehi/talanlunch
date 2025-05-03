using AutoMapper;
using MediatR;
using TalanLunch.Application.Commands.Menu;
using TalanLunch.Application.Interfaces;
using TalanLunch.Domain.Entities;

namespace TalanLunch.Application.Handlers.MenuHandlers
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
            var menuDto = request.MenuDto;

            if (menuDto.Dishes == null || !menuDto.Dishes.Any())
                throw new ArgumentException("Le menu doit contenir au moins un plat.");

            var dishIds = menuDto.Dishes.Select(d => d.DishId).ToList();
            var dishes = await _dishRepository.GetDishesByIdsAsync(dishIds);

            var dishesById = dishes.ToDictionary(d => d.DishId);
            var invalidDishIds = dishIds.Except(dishes.Select(d => d.DishId)).ToList();

            if (invalidDishIds.Any())
            {
                throw new ArgumentException($"Plats invalides : {string.Join(", ", invalidDishIds)}");
            }

            var newMenu = _mapper.Map<Menu>(menuDto);
            newMenu.MenuDate = DateTime.Now;
            newMenu.MenuDishes = menuDto.Dishes
        .Where(d => dishesById.ContainsKey(d.DishId))
        .Select(d => new MenuDish
        {
            DishId = d.DishId,
            Dish = dishesById[d.DishId],
            DishQuantity = d.DishQuantity
        }).ToList();


            return await _menuRepository.AddMenuAsync(newMenu);
        }
    }

}
