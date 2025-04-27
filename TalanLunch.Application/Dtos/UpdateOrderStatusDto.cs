namespace TalanLunch.Application.Dtos
{
    public class UpdateOrderStatusDto
    {
        public int OrderId { get; set; }
        public bool? Paid { get; set; }
        public bool? Served { get; set; }
    }
}
