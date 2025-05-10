using MediatR;

namespace TalanLunch.Application.Menus
{
    public class RemoveDishFromMenuCommand : IRequest<Domain.Entities.Menu>
    {
        public int MenuId { get; }
        public int DishId { get; }

        public RemoveDishFromMenuCommand(int menuId, int dishId)
        {
            MenuId = menuId;
            DishId = dishId;
        }
    }
}