using MediatR;

namespace TalanLunch.Application.Admin.Commands.DeleteUser
{
    public class DeleteUserCommand(int userId) : IRequest<Unit>
    {
        public int UserId { get; set; } = userId;
    }
}
