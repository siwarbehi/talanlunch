namespace TalanLunch.Application.Orders.Queries.GetAllOrders
{
    public class PagedResult<T>
    {
        public List<T> Items { get; set; } = new();
        public int TotalItems { get; set; } // total dans la BDD
        public int PageNumber { get; set; } // numéro de page actuelle
        public int PageSize { get; set; }
    }

}
