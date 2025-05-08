using AutoMapper;
using TalanLunch.Domain.Entities;
using TalanLunch.Application.Dtos.User;

namespace TalanLunch.Application.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            // Mapper de User vers UserDto
            CreateMap<Domain.Entities.User, UserDto>()
                .ForMember(dest => dest.ProfilePicture, opt => opt.Ignore()) // Ignore le champ ProfilePicture si nécessaire
                .ForMember(dest => dest.UpdatedPassword, opt => opt.Ignore());
        }
    }
}
