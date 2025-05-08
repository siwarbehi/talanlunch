using MediatR;
using TalanLunch.Application.Dtos.Auth;

public class RegisterUserCommand : IRequest<string>
{
    public RegisterUserDto RegisterUserDto { get; set; }
    public bool IsCaterer { get; set; }

    public RegisterUserCommand(RegisterUserDto dto, bool isCaterer)
    {
        RegisterUserDto = dto;
        IsCaterer = isCaterer;
    }
}
