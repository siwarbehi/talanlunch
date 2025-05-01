
namespace TalanLunch.Application.Dtos.Menu
{
    public class AddDishToMenuResult
    {
        public TalanLunch.Domain.Entities.Menu? Menu { get; set; }
        public bool DishAlreadyExists { get; set; }
    }
}