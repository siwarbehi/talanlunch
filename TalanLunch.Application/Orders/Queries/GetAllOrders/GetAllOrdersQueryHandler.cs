using AutoMapper;
using MediatR;
using TalanLunch.Application.Interfaces;

namespace TalanLunch.Application.Orders.Queries.GetAllOrders;

public class GetAllOrdersQueryHandler : IRequestHandler<GetAllOrdersQuery, PagedResult<OrderDay>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;

    public GetAllOrdersQueryHandler(IOrderRepository orderRepository, IMapper mapper)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
    }
    public async Task<PagedResult<OrderDay>> Handle(GetAllOrdersQuery query, CancellationToken cancellationToken)
    {
        var pagedOrders = await _orderRepository.GetAllOrdersAsync(
   query,cancellationToken);

        var mappedItems = _mapper.Map<List<OrderDay>>(pagedOrders.Items);

        return new PagedResult<OrderDay>
        {
            Items = mappedItems,
            TotalItems = pagedOrders.TotalItems,
            PageNumber = pagedOrders.PageNumber,
            PageSize = pagedOrders.PageSize
        };
    }

}
