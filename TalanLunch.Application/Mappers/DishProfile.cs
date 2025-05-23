using AutoMapper;
using TalanLunch.Application.Dishes.Commands.AddDish;
using TalanLunch.Application.Dishes.Commands.UpdateDish;
using TalanLunch.Domain.Entities;

namespace TalanLunch.Application.Mapping
{
    public class DishProfile : Profile
    {
        public DishProfile()
        {
            CreateMap<AddDishCommand, Dish>()
                .ForMember(dest => dest.DishPhoto, opt => opt.Ignore())
                .ForMember(dest => dest.OrderDate, opt => opt.Ignore())
                .ForMember(dest => dest.ReviewCount, opt => opt.Ignore())
                .ForMember(dest => dest.CurrentRating, opt => opt.Ignore());


            CreateMap<UpdateDishCommand, Dish>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        }
    }
}
