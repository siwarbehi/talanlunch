using MediatR;

namespace TalanLunch.Application.Menus.Commands.AddMenu
{
    public class AddMenuCommand : IRequest<Domain.Entities.Menu>
    {
        public string? MenuDescription { get; set; }
        public List<DishCreation> Dishes { get; set; } = new List<DishCreation>();

        public class DishCreation
        {
            public int DishId { get; set; }
            public int DishQuantity { get; set; }
        }
    }
}
