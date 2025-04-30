namespace TalanLunch.Application.Dtos.Menu { 
    public class GetAllMenusDto { 
        public int MenuId { get; set; } 
        public string MenuDescription { get; set; }
        public List<DishMenuAllDto> Dishes { get; set; }

         public bool IsMenuOfTheDay { get; set; } = false;

    }
} 
