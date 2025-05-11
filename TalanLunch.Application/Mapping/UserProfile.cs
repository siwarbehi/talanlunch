using AutoMapper;
using TalanLunch.Domain.Entities;
using TalanLunch.Application.Users.Commands;

namespace TalanLunch.Application.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UpdateUserProfileCommandResult>();
        }
    }

}
