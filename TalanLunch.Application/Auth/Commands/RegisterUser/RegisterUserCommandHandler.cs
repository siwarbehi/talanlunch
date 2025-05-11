using MediatR;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using TalanLunch.Domain.Entities;
using TalanLunch.Domain.Enums;
using TalanLunch.Application.Interfaces;
using TalanLunch.Application.Auth.Commands.RegisterUser;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, string>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public RegisterUserCommandHandler(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<string> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _userRepository.GetUserByEmailAsync(request.EmailAddress);
        if (existingUser != null)
        {
            return "Cet email est déjà utilisé.";
        }

        var user = _mapper.Map<User>(request);
        user.UserRole = request.IsCaterer ? UserRole.CATERER : UserRole.COLLABORATOR;
        user.IsApproved = !request.IsCaterer;

        var passwordHasher = new PasswordHasher<User>();
        user.HashedPassword = passwordHasher.HashPassword(null, request.Password);

        var success = await _userRepository.AddUserAsync(user);

        return success
            ? request.IsCaterer
                ? "Votre demande d'inscription en tant que traiteur a été enregistrée. Un administrateur doit approuver votre compte."
                : "Inscription réussie."
            : "Une erreur s'est produite lors de l'enregistrement de l'utilisateur. Veuillez réessayer.";
    }
}
