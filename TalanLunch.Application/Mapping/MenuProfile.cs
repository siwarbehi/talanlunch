using AutoMapper;
using TalanLunch.Application.Dtos.Menu;
using TalanLunch.Domain.Entities;

public class MenuProfile : Profile
{
    public MenuProfile()
    {
        CreateMap<MenuDto, Menu>()
            .ForMember(dest => dest.MenuDate, opt => opt.MapFrom(_ => DateTime.Now))
            .ForMember(dest => dest.MenuDishes, opt => opt.Ignore()); 

        CreateMap<DishCreationDto, MenuDish>()
            .ForMember(dest => dest.DishId, opt => opt.MapFrom(src => src.DishId))
            .ForMember(dest => dest.DishQuantity, opt => opt.MapFrom(src => src.DishQuantity))
            .ForMember(dest => dest.Dish, opt => opt.Ignore())
            .ForMember(dest => dest.Menu, opt => opt.Ignore());
    }
}
