namespace TalanLunch.Application.Dtos.Order
{
    public class PaginationQuery
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; } // nbr d elements par page

        // Ajout pour filtrer par utilisateur
        public string? FirstName { get; set; }
        public string? LastName { get; set; } 
    }


}
