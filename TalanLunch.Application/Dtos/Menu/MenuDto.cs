namespace TalanLunch.Application.Dtos.Menu
{
    
    public class MenuDto
    {
        public string? MenuDescription { get; set; }
       public List<DishCreationDto> Dishes { get; set; } = new List<DishCreationDto>();

    }
}