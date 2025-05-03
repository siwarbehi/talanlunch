using MediatR;
using TalanLunch.Application.Dtos.Menu;

namespace TalanLunch.Application.Commands.Menu 
{
   public class AddMenuCommand : IRequest<Domain.Entities.Menu>

    {
        public MenuDto MenuDto { get; }

        public AddMenuCommand(MenuDto menuDto)
        {
            MenuDto = menuDto ?? throw new ArgumentNullException(nameof(menuDto));
        }
    }
}
