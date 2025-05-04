// Application/Handlers/DishHandlers/GetAllDishesQueryHandler.cs
using MediatR;

using TalanLunch.Application.Interfaces;
using TalanLunch.Application.Queries.Dish;
using TalanLunch.Domain.Entities;

namespace TalanLunch.Application.Handlers.DishHandlers
{
    
    public class GetAllDishesQueryHandler
        : IRequestHandler<GetAllDishesQuery, IEnumerable<Dish>>
    {
        private readonly IDishRepository _dishRepository;

        public GetAllDishesQueryHandler(IDishRepository dishRepository)
        {
            _dishRepository = dishRepository;
        }

        public async Task<IEnumerable<Dish>> Handle(
            GetAllDishesQuery request,
            CancellationToken cancellationToken)
        {
            return await _dishRepository.GetAllDishesAsync();
        }
    }
}
