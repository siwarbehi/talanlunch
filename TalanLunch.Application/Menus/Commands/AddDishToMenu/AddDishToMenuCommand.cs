/*using MediatR;
using TalanLunch.Application.Dtos;

namespace TalanLunch.Application.Menus.Commands.AddDishToMenu
{
    public class AddDishToMenuCommand : IRequest<AddDishToMenuResult>
    {
        public int MenuId { get; }
        public AddDishToMenuDto DishDto { get; }

        public AddDishToMenuCommand(int menuId, AddDishToMenuDto dto)
        {
            MenuId = menuId;
            DishDto = dto;
        }
    }
}

*//*using MediatR;


namespace TalanLunch.Application.Menus.Commands.AddDishToMenu
{
    public class AddDishToMenuCommand : IRequest<AddDishToMenuResult>
    {
        public int MenuId { get; set; }
        public int DishId { get; set; }
        public int Quantity { get; set; } = 1;
        public string? NewDescription { get; set; }
        public AddDishToMenuCommand() { }
    *//*    public AddDishToMenuCommand(int menuId, int dishId, int quantity, string? enwDescription)
        {
            MenuId = menuId;
            DishId = dishId;
            Quantity = quantity;
            NewDescription = enwDescription;
        }*//*
    }
}
*/