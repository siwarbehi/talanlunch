using MediatR;

namespace TalanLunch.Application.Commands.Dish
{
    public class DeleteDishCommand : IRequest<Unit>
    {
        public int DishId { get; }

        public DeleteDishCommand(int dishId)
        {
            DishId = dishId;
        }
    }
}
