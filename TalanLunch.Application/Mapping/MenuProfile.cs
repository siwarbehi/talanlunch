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

        
        CreateMap<Menu, GetAllMenusDto>()
            .ForMember(dest => dest.Dishes, opt => opt.MapFrom(src => src.MenuDishes));

        CreateMap<MenuDish, DishMenuAllDto>()
            .ForMember(dest => dest.DishId, opt => opt.MapFrom(src => src.Dish.DishId))
            .ForMember(dest => dest.DishQuantity, opt => opt.MapFrom(src => src.DishQuantity))
            .ForMember(dest => dest.DishName, opt => opt.MapFrom(src => src.Dish.DishName))
            .ForMember(dest => dest.DishPrice, opt => opt.MapFrom(src => src.Dish.DishPrice))
            .ForMember(dest => dest.DishPhoto, opt => opt.MapFrom(src => src.Dish.DishPhoto))
            .ForMember(dest => dest.DishDescription, opt => opt.MapFrom(src => src.Dish.DishDescription));
    }
}

