namespace TalanLunch.Application.Orders.Queries.GetAllOrders
{
    public class OrderDay
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? ProfilePicture { get; set; }
        public int OrderId { get; set; }

        public string? OrderRemark { get; set; }
        public decimal TotalAmount { get; set; }
        public bool Paid { get; set; }
        public bool Served { get; set; }
        public DateTime OrderDate { get; set; }
        public List<DishOrderQuantity> Dishes { get; set; } = new();
    }
}
