﻿using MediatR;
using TalanLunch.Application.Interfaces;

namespace TalanLunch.Application.Auth.Commands.Logout
{
    public class LogoutCommandHandler : IRequestHandler<LogoutCommand, bool>
    {
        private readonly IUserRepository _userRepository;

        public LogoutCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByRefreshTokenAsync(request.RefreshToken);
            if (user == null) return false;

            await _userRepository.DeleteRefreshTokenAsync(user);
            return true;
        }
    }
}
