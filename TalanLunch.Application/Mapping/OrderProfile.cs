using AutoMapper;
using TalanLunch.Application.Dtos.Order;
using TalanLunch.Domain.Entities;

namespace TalanLunch.Application.Mapping
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            // Mapping principal Order -> OrderDayDto
            CreateMap<Order, OrderDayDto>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.User.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.User.LastName))
                .ForMember(dest => dest.ProfilePicture, opt => opt.MapFrom(src => src.User.ProfilePicture))
                .ForMember(dest => dest.Dishes, opt => opt.MapFrom(src => src.OrderDishes))
                .ForMember(dest => dest.Served, opt => opt.MapFrom(src => src.Served)) // facultatif si utilisé
                .ForMember(dest => dest.OrderRemark, opt => opt.MapFrom(src => src.OrderRemark)); // idem

            // Mapping OrderDish -> DishOrderDto
            CreateMap<OrderDish, DishOrderDto>()
                .ForMember(dest => dest.DishName, opt => opt.MapFrom(src => src.Dish.DishName))
                .ForMember(dest => dest.DishId, opt => opt.MapFrom(src => src.Dish.DishId))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity));
        }
    }
}
