﻿using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using TalanLunch.Application.Auth.Commands.RegisterUser;
using TalanLunch.Application.Interfaces;
using TalanLunch.Domain.Entities;
using TalanLunch.Domain.Enums;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, string>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public RegisterUserCommandHandler(
        IUserRepository userRepository,
        IMapper mapper)
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
        user.UserRole = request.UserRole;
        user.IsApproved = request.UserRole != UserRole.CATERER; // Seuls les traiteurs doivent être approuvés manuellement

        var passwordHasher = new PasswordHasher<User>();
        user.HashedPassword = passwordHasher.HashPassword(null, request.Password);

        var success = await _userRepository.AddUserAsync(user);

        return success
            ? request.UserRole == UserRole.CATERER
                ? "Votre demande d'inscription en tant que traiteur a été enregistrée. Un administrateur doit approuver votre compte."
                : "Inscription réussie."
            : "Une erreur s'est produite lors de l'enregistrement de l'utilisateur. Veuillez réessayer.";
    }


}
