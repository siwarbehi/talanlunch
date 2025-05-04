using MediatR;

namespace TalanLunch.Application.Queries.Dish
{

    public class GetAllDishesQuery : IRequest<IEnumerable<Domain.Entities.Dish>>
    {
    }
}
