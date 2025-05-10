using MediatR;
using Microsoft.AspNetCore.Http;
using TalanLunch.Domain.Entities;


namespace TalanLunch.Application.Dishes.Commands.AddDish
{
    public class AddDishCommand : IRequest<Dish>
    {
        public string DishName { get; set; } = string.Empty;
        public string DishDescription { get; set; } = string.Empty;
        public decimal DishPrice { get; set; }public class DeleteDishCommand : IRequest<Unit>;

        public IFormFile? DishPhoto { get; set; }

        public AddDishCommand() { }

        public AddDishCommand(string dishName, string dishDescription, decimal dishPrice, IFormFile? dishPhoto)
        {
            DishName = dishName;
            DishDescription = dishDescription;
            DishPrice = dishPrice;
            DishPhoto = dishPhoto;
        }
    }
}
