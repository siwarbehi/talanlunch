using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TalanLunch.Domain.Entities
{
    public class Menu
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MenuId { get; set; }
        public string? MenuDescription { get; set; }
        public DateTime MenuDate { get; set; } = DateTime.Now;
        public bool IsMenuOfTheDay { get; set; } = false;

        [Required]
        public ICollection<MenuDish> MenuDishes { get; set; } = [];
    }
}
