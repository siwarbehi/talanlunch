using AutoMapper;
using TalanLunch.Application.Auth.Commands.LoginUser;
using TalanLunch.Application.Auth.Commands.RegisterUser;
using TalanLunch.Application.Auth.Common;
using TalanLunch.Domain.Entities;

public class AuthProfile : Profile
{
    public AuthProfile()
    {
        CreateMap<RegisterUserCommand, User>()
              .ForMember(dest => dest.EmailAddress, opt => opt.MapFrom(src => src.EmailAddress))
              .ForMember(dest => dest.UserRole, opt => opt.Ignore())
              .ForMember(dest => dest.IsApproved, opt => opt.Ignore())
              .ForMember(dest => dest.HashedPassword, opt => opt.Ignore());


       
        CreateMap<User, TokenResponseDto>()
            .ForMember(dest => dest.IsApproved, opt => opt.MapFrom(src => src.IsApproved))
            .ForMember(dest => dest.AccessToken, opt => opt.Ignore())
            .ForMember(dest => dest.RefreshToken, opt => opt.Ignore());
    }
}
