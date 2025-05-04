using MediatR;

namespace TalanLunch.Application.Queries.Dish
{
    /// <summary>
    /// Requête pour récupérer un plat par son ID.
    /// </summary>
    public class GetDishByIdQuery : IRequest<Domain.Entities.Dish>
    {
        public int DishId { get; }

        public GetDishByIdQuery(int dishId)
        {
            DishId = dishId;
        }
    }
}
