using MediatR;

namespace TalanLunch.Application.Menus.Commands.SetMenuOfTheDayCommand
{

    public class SetMenuOfTheDayCommand : IRequest<bool>
    {
        public int MenuId { get; set; }

        public SetMenuOfTheDayCommand() { }

        public SetMenuOfTheDayCommand(int menuId)
        {
            MenuId = menuId;
        }
    }
}
