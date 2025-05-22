using MediatR;
using System.Text.Json.Serialization;

namespace TalanLunch.Application.Menus.Commands.AddDishToMenu
{
    public class AddDishToMenuCommand : IRequest<AddDishToMenuCommandResult>
    {
        [JsonIgnore]
        public int MenuId { get; set; } 
        public int? DishId { get; set; }
        public string Description { get; set; } = string.Empty;
        public int Quantity { get; set; }
    }
}
