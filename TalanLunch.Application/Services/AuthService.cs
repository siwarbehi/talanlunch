using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalanLunch.Application.DTOs;
using TalanLunch.Application.Interfaces;
using TalanLunch.Domain.Entities;
using TalanLunch.Domain.Enums;
using Microsoft.AspNetCore.Identity; 
namespace TalanLunch.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;

        public AuthService(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        public async Task<string> RegisterUserAsync(RegisterUserDto registerUserDto, bool isCaterer)
        {
            // Vérifier si l'email existe déjà
            var existingUser = await _authRepository.GetUserByEmailAsync(registerUserDto.EmailAddress);
            if (existingUser != null)
            {
                return "Cet email est déjà utilisé.";
            }

            // Création de l'utilisateur
            var user = new User
            {
                FirstName = registerUserDto.FirstName,
                LastName = registerUserDto.LastName,
                EmailAddress = registerUserDto.EmailAddress,
                PhoneNumber = registerUserDto.PhoneNumber,
                UserRole = isCaterer ? UserRole.CATERER : UserRole.COLLABORATOR,
                IsApproved = !isCaterer, // Si c'est un traiteur, il doit être approuvé manuellement
            };

            // Utilisation de PasswordHasher pour hacher le mot de passe
            var passwordHasher = new PasswordHasher<User>();
            var hashedPassword = passwordHasher.HashPassword(user, registerUserDto.Password); // Hachage du mot de passe

            user.HashedPassword = hashedPassword;

            // Enregistrement de l'utilisateur dans la base de données
            await _authRepository.AddUserAsync(user);
            await _authRepository.SaveChangesAsync();

            // Retour d'un message basé sur le rôle de l'utilisateur
            if (isCaterer)
            {
                return "Votre demande d'inscription en tant que traiteur a été enregistrée. Un administrateur doit approuver votre compte.";
            }

            return "Inscription réussie.";
        }
    }
}
