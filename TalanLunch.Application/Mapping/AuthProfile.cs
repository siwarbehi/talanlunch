using AutoMapper;
using TalanLunch.Application.Dtos.Auth;
using TalanLunch.Domain.Entities;

public class AuthProfile : Profile
{
    public AuthProfile()
    {
        CreateMap<RegisterUserDto, User>()
            .ForMember(dest => dest.UserId, opt => opt.Ignore())
            .ForMember(dest => dest.HashedPassword, opt => opt.Ignore())
            .ForMember(dest => dest.UserRole, opt => opt.Ignore())
            .ForMember(dest => dest.IsApproved, opt => opt.Ignore());

        CreateMap<LoginDto, User>()
                .ForMember(dest => dest.EmailAddress, opt => opt.MapFrom(src => src.EmailAddress))
                .ForMember(dest => dest.HashedPassword, opt => opt.Ignore());

        CreateMap<User, TokenResponseDto>()
            .ForMember(dest => dest.IsApproved, opt => opt.MapFrom(src => src.IsApproved))
            .ForMember(dest => dest.AccessToken, opt => opt.Ignore())
            .ForMember(dest => dest.RefreshToken, opt => opt.Ignore());
    }
}
