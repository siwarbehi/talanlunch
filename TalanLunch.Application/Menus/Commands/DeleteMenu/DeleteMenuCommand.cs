using MediatR;

namespace TalanLunch.Application.Menus.Commands.DeleteMenu
{
    public class DeleteMenuCommand : IRequest<Unit>
    {
        public int MenuId { get; }

        public DeleteMenuCommand(int menuId)
        {
            MenuId = menuId;
        }
    }
}
