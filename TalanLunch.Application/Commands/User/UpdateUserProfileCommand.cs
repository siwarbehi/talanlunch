using MediatR;
using TalanLunch.Application.Dtos.User;

namespace TalanLunch.Application.User.Commands
{
    public class UpdateUserProfileCommand : IRequest<UserDto>
    {
        public int UserId { get; set; }
        public UserDto UserDto { get; set; }

        public UpdateUserProfileCommand(int userId, UserDto userDto)
        {
            UserId = userId;
            UserDto = userDto;
        }
    }
}
