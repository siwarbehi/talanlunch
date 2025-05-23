using MediatR;
using TalanLunch.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace TalanLunch.Application.Users.Queries.GetUserById
{
    public class GetUserByIdQuery : IRequest<User>
    {
        [Required]
        public int UserId { get; set; }

        public GetUserByIdQuery() { }

        public GetUserByIdQuery(int userId)
        {
            UserId = userId;
        }
    }
}
