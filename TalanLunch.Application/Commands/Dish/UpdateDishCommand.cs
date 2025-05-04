using MediatR;
using Microsoft.AspNetCore.Http;
using TalanLunch.Application.Dtos.Dish;

namespace TalanLunch.Application.Commands.Dish
{
    
    public class UpdateDishCommand : IRequest<Domain.Entities.Dish>
    {
        public int DishId { get; }
        public DishUpdateDto UpdateDto { get; }
        public IFormFile? Photo { get; }

        public UpdateDishCommand(int dishId, DishUpdateDto updateDto, IFormFile? photo)
        {
            DishId = dishId;
            UpdateDto = updateDto;
            Photo = photo;
        }
    }
}
