using MediatR;
using TalanLunch.Application.Interfaces;
using TalanLunch.Domain.Entities;

namespace TalanLunch.Application.Dishes.Queries.GetDishById
{
   
    public class GetDishByIdQueryHandler : IRequestHandler<GetDishByIdQuery, Dish?>
    {
        private readonly IDishRepository _dishRepository;

        public GetDishByIdQueryHandler(IDishRepository dishRepository)
        {
            _dishRepository = dishRepository;
        }

        public async Task<Dish?> Handle(GetDishByIdQuery request, CancellationToken cancellationToken)
        {
            return await _dishRepository.GetDishByIdAsync(request.DishId);
        }

    }
}
