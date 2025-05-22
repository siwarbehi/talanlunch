namespace TalanLunch.Application.Menus.Queries.GetAllMenus
{
    public class GetAllMenusQueryResult // ce n’est PAS une requête, c’est le résultat
    {
        public int MenuId { get; set; }
        public string MenuDescription { get; set; }
        public bool IsMenuOfTheDay { get; set; } = false;
        public List<DishMenuAll> Dishes { get; set; }
    }

    public class DishMenuAll
    {
        public int? DishId { get; set; }
        public int DishQuantity { get; set; }
        public string DishName { get; set; }
        public decimal DishPrice { get; set; }
        public string? DishPhoto { get; set; }
        public string? DishDescription { get; set; }
    }

}
