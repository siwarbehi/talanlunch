using MediatR;
using TalanLunch.Domain.Entities;

namespace TalanLunch.Application.Dishes.Queries.GetDishById
{

    public class GetDishByIdQuery : IRequest<Dish>
    {
        public int DishId { get; set; }  

    }

}
