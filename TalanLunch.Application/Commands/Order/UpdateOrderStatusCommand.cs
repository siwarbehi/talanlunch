using MediatR;
using TalanLunch.Application.Dtos.Order;

namespace TalanLunch.Application.Commands.Order
{
   
    public class UpdateOrderStatusCommand : IRequest<bool>
    {
        public UpdateOrderStatusDto Dto { get; }

        public UpdateOrderStatusCommand(UpdateOrderStatusDto dto)
        {
            Dto = dto;
        }
    }
}
