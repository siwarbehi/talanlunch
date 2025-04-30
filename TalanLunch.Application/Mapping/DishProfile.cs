using AutoMapper;
using TalanLunch.Application.Dtos.Dish;
using TalanLunch.Domain.Entities;

namespace TalanLunch.Application.Mapping
{

    public class DishProfile : Profile
    {
        public DishProfile()
        {
      
            CreateMap<DishDto, Dish>()
                .ForMember(dest => dest.OrderDate, opt => opt.Ignore())
                .ForMember(dest => dest.ReviewCount, opt => opt.Ignore())
                .ForMember(dest => dest.CurrentRating, opt => opt.Ignore())
                .ForMember(dest => dest.DishPhoto, opt => opt.Ignore());

            
            CreateMap<DishUpdateDto, Dish>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            
           
        }
    }

}

