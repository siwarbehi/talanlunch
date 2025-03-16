﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalanLunch.Application.DTOs;
using TalanLunch.Application.Interfaces;
using TalanLunch.Domain.Entities;
using TalanLunch.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TalanLunch.Application.Dtos;
using System.Security.Cryptography;



namespace TalanLunch.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;


        public AuthService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }
        public async Task<string> RegisterUserAsync(RegisterUserDto registerUserDto, bool isCaterer)
        {
            // Vérifier si l'email existe déjà
            var existingUser = await _userRepository.GetUserByEmailAsync(registerUserDto.EmailAddress);
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
            bool isAdded = await _userRepository.AddUserAsync(user); 

            // Vérifier que l'utilisateur a bien été ajouté
            if (!isAdded)
            {
                return "Une erreur s'est produite lors de l'enregistrement de l'utilisateur. Veuillez réessayer.";
            }

            // Retour d'un message basé sur le rôle de l'utilisateur
            if (isCaterer)
            {
                return "Votre demande d'inscription en tant que traiteur a été enregistrée. Un administrateur doit approuver votre compte.";
            }

            return "Inscription réussie.";
        }

        public async Task<TokenResponseDto?> LoginAsync(LoginDto request)
        {
            var user = await _userRepository.GetUserByEmailAsync(request.EmailAddress);
            if (user is null)
            {
                return null;
            }
            if (new PasswordHasher<User>().VerifyHashedPassword(user, user.HashedPassword, request.Password)
                == PasswordVerificationResult.Failed)
            {
                return null;
            }

            return await CreateTokenResponse(user);
        }
        // Méthode pour créer un TokenResponse contenant le token d'accès et le refresh token

        private async Task<TokenResponseDto> CreateTokenResponse(User? user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "L'utilisateur ne peut pas être nul.");
            }

            return new TokenResponseDto
            {
                AccessToken = CreateToken(user),
                RefreshToken = await GenerateAndSaveRefreshTokenAsync(user)
            };
        }

        // Méthode pour générer et sauvegarder un refresh token

        private async Task<string> GenerateAndSaveRefreshTokenAsync(User user)
        {
            var refreshToken = GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _userRepository.UpdateUserAsync(user);
            return refreshToken;
        }
        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
        // Méthode pour créer un token JWT d'accès

        private string CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.EmailAddress),
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Role, user.UserRole.ToString())
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration.GetValue<string>("AppSettings:Token")!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var tokenDescriptor = new JwtSecurityToken(
                issuer: _configuration.GetValue<string>("AppSettings:Issuer"),
                audience: _configuration.GetValue<string>("AppSettings:Audience"),
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
        // Méthode pour rafraîchir les tokens (access token et refresh token)
        public async Task<TokenResponseDto?> RefreshTokensAsync(RefreshTokenRequestDto request)
        {
            var user = await ValidateRefreshTokenAsync(request.UserId, request.RefreshToken);
            if (user is null)
                return null;

            return await CreateTokenResponse(user);
        }

        // Méthode pour valider un refresh token
        private async Task<User?> ValidateRefreshTokenAsync(int userId, string refreshToken)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user is null || user.RefreshToken != refreshToken
                || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return null;
            }

            return user;
        }
    }
}
