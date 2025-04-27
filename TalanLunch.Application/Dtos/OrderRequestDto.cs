namespace TalanLunch.Application.Dtos
{
    public class OrderRequestDto
    {
        public int UserId { get; set; }
        public int MenuId { get; set; }
        public List<DishOrderDto> Dishes { get; set; } = new();
        public string? OrderRemark { get; set; }             
    }
}
