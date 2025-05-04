using MediatR;
using TalanLunch.Application.Dtos.Dish;

namespace TalanLunch.Application.Commands.Dish
{
    public class AddDishCommand : IRequest<Domain.Entities.Dish>
    {
        public DishDto DishDto { get; }

        public AddDishCommand(DishDto dishDto)
        {
            DishDto = dishDto;
        }
    }
}
