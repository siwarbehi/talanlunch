using AutoMapper;
using TalanLunch.Application.Menus;
using TalanLunch.Application.Menus.Commands.AddMenu;
using TalanLunch.Application.Menus.Queries.GetAllMenus;
using TalanLunch.Domain.Entities;


public class MenuProfile : Profile
{
    public MenuProfile()
    {
        CreateMap<AddMenuCommand, Menu>()
                .ForMember(dest => dest.MenuDate, opt => opt.Ignore());

        CreateMap<AddMenuCommand.DishCreation, MenuDish>()
            .ForMember(dest => dest.Dish, opt => opt.Ignore());

        CreateMap<Menu, GetAllMenusQueryResult>()
                    .ForMember(dest => dest.Dishes, opt => opt.MapFrom(src =>
                        src.MenuDishes.Select(md => new DishMenuAll
                        {
                            DishId = md.DishId,
                            DishQuantity = md.DishQuantity,
                            DishName = md.Dish!.DishName,
                            DishPrice = md.Dish!.DishPrice ?? 0,
                            DishPhoto = md.Dish.DishPhoto,
                            DishDescription = md.Dish.DishDescription
                        }).ToList()
                  ));
        CreateMap<AddDishToMenuDto, MenuDish>()
        .ForMember(dest => dest.DishId, opt => opt.MapFrom(src => src.DishId))
        .ForMember(dest => dest.DishQuantity, opt => opt.MapFrom(src => src.Quantity))
        .ForMember(dest => dest.Menu, opt => opt.Ignore())
        .ForMember(dest => dest.Dish, opt => opt.Ignore())
        .ForMember(dest => dest.MenuId, opt => opt.Ignore());
        /*  CreateMap<AddDishToMenuCommand, MenuDish>()
     .ForMember(dest => dest.DishQuantity, opt => opt.MapFrom(src => src.Quantity))
     .ForMember(dest => dest.DishId, opt => opt.MapFrom(src => src.DishId));*/


    }
}


