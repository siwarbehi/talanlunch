using MediatR;

namespace TalanLunch.Application.Commands.Menu
{
    
    public class SetMenuOfTheDayCommand : IRequest<bool>
    {
        public int MenuId { get; }

        public SetMenuOfTheDayCommand(int menuId)
        {
            MenuId = menuId;
        }
    }
}
