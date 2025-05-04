using AutoMapper;
using MediatR;
using TalanLunch.Application.Dtos.Order;
using TalanLunch.Application.Interfaces;
using TalanLunch.Application.Queries.Order;

namespace TalanLunch.Application.Handlers.OrderHandlers
{
    
    public class GetAllOrdersQueryHandler
        : IRequestHandler<GetAllOrdersQuery, IEnumerable<OrderDayDto>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public GetAllOrdersQueryHandler(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OrderDayDto>> Handle(
            GetAllOrdersQuery request,
            CancellationToken cancellationToken)
        {
            // 1. Récupère toutes les entités Order
            var orders = await _orderRepository.GetAllOrdersAsync();
            // 2. Mappe en DTOs
            return _mapper.Map<IEnumerable<OrderDayDto>>(orders);
        }
    }
}
