using MediatR;
using TalanLunch.Application.Dishes.Commands;
using TalanLunch.Application.Interfaces;

namespace TalanLunch.Application.Dishes.Handlers
{
    public class DeleteDishCommandHandler : IRequestHandler<DeleteDishCommand, Unit>
    {
        private readonly IDishRepository _dishRepository;

        public DeleteDishCommandHandler(IDishRepository dishRepository)
        {
            _dishRepository = dishRepository;
        }

        public async Task<Unit> Handle(DeleteDishCommand request, CancellationToken cancellationToken)
        {
            var dish = await _dishRepository.GetDishByIdAsync(request.DishId); 
            if (dish == null)
                throw new KeyNotFoundException($"Dish with ID {request.DishId} not found.");

            await _dishRepository.DeleteDishAsync(request.DishId);
            return Unit.Value;
        }
    }
}
