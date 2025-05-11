using MediatR;
using TalanLunch.Domain.Entities;

public class RemoveDishFromMenuCommand : IRequest<Menu>
{
    public int MenuId { get; set; }
    public int DishId { get; set; }

    public RemoveDishFromMenuCommand() { }

    public RemoveDishFromMenuCommand(int menuId, int dishId)
    {
        MenuId = menuId;
        DishId = dishId;
    }
}
