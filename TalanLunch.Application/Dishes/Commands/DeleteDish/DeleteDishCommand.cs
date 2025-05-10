using System.Diagnostics.CodeAnalysis;
using MediatR;

namespace TalanLunch.Application.Dishes.Commands
{
    [ExcludeFromCodeCoverage]
    public class DeleteDishCommand : IRequest<Unit>
    {
        public int DishId { get; }

        public DeleteDishCommand(int dishId)
        {
            DishId = dishId;
        }
    }
}
