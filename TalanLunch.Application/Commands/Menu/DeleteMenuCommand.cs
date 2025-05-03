using MediatR;

namespace TalanLunch.Application.Commands.Menu
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
