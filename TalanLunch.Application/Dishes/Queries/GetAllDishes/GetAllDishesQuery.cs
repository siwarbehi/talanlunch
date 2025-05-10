using MediatR;
using TalanLunch.Domain.Entities;

namespace TalanLunch.Application.Dishes.Queries.GetAllDishes
{
    public class GetAllDishesQuery : IRequest<IEnumerable<Dish>>
    {
    }
}
