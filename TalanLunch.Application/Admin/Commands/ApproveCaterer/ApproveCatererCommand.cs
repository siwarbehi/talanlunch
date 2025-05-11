using MediatR;

namespace TalanLunch.Application.Admin.Commands.ApproveCaterer
{
    public class ApproveCatererCommand : IRequest<bool>
    {
        public int UserId { get; set; }
    }
}
