namespace TalanLunch.Application.Dtos
{
    
    public class MenuDto
    {
        public string? MenuDescription { get; set; }
       public List<DishCreationDto> Dishes { get; set; } = new List<DishCreationDto>();

    }
}