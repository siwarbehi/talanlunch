using MediatR;
using TalanLunch.Application.Dtos.Menu;

namespace TalanLunch.Application.Commands.Menu
{
    public class AddDishToMenuCommand : IRequest<AddDishToMenuResultDto>
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
