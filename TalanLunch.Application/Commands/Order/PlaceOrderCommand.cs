using MediatR;
using TalanLunch.Application.Dtos.Order;


namespace TalanLunch.Application.Commands.Order
{
    
    public class PlaceOrderCommand : IRequest<Domain.Entities.Order>
    {
        public OrderRequestDto Request { get; }

        public PlaceOrderCommand(OrderRequestDto request)
        {
            Request = request;
        }
    }
}
