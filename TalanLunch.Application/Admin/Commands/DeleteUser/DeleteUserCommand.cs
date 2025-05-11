using MediatR;

namespace TalanLunch.Application.Admin.Commands.DeleteUser
{
    public class DeleteUserCommand : IRequest<Unit>
    {
        public int UserId { get; set; }

        public DeleteUserCommand(int userId)
        {
            UserId = userId;
        }
    }
}
