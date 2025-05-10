namespace TalanLunch.Application.Dtos
{
    public class AddDishToMenuDto
    {
        public int DishId { get; set; }
        public int Quantity { get; set; } = 1;
        public string? NewDescription { get; set; }  // facultatif

    }
}
