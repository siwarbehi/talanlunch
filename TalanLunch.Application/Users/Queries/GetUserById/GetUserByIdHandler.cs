using MediatR;
using TalanLunch.Application.Interfaces;
using TalanLunch.Domain.Entities;

namespace TalanLunch.Application.Users.Queries.GetUserById
{
    public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, User>
    {
        private readonly IUserRepository _userRepository;

        public GetUserByIdHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<User> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentException("La requête ne peut pas être null.");
            }

            if (request.UserId <= 0)
            {
                throw new ArgumentException("L'ID utilisateur doit être supérieur à zéro.");
            }

            var user = await _userRepository.GetUserByIdAsync(request.UserId);

            if (user == null)
            {
                throw new ArgumentException($"Utilisateur avec l'ID {request.UserId} non trouvé.");
            }

            return user;
        }
    }
}
