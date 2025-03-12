
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TalanLunch.Domain.Entities;

namespace TalanLunch.Domain.Entities
{
    public class MenuDish
    {
        public int MenuId { get; set; }
        
        [JsonInclude] 

        public required Menu Menu { get; set; }
        public int DishId { get; set; }
        public required Dish Dish { get; set; }
    }
} //on doit toujours avoir Dish et Menu sinon pas d'association valide
