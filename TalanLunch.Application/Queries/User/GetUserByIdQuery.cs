using MediatR;

namespace TalanLunch.Application.User.Queries
{
    public class GetUserByIdQuery : IRequest<Domain.Entities.User>
    {
        public int UserId { get; set; }

        public GetUserByIdQuery(int userId)
        {
            UserId = userId;
        }
    }
}
