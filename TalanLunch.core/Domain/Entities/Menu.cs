namespace TalanLunch.Core.Domain.Entities
{
    public class Menu
    {
        public int MenuId { get; set; }
        public string MenuName { get; set; } = string.Empty;
        public string MenuDescription { get; set; } = string.Empty;
        public int MenuRating { get; set; }
        public DateTime MenuDate { get; set; } = DateTime.Now;
        public ICollection<MenuDish> MenuDishes { get; set; } = new List<MenuDish>();
    }
}
