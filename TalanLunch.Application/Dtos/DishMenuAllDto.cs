namespace TalanLunch.Application.Dtos
{
    public class DishMenuAllDto
    {
        public int DishId { get; set; }

        public int DishQuantity { get; set; }
        public string DishName { get; set; }
        public decimal DishPrice { get; set; }

        public string? DishPhoto { get; set; }
        public string? DishDescription { get; set; }




    }
}
