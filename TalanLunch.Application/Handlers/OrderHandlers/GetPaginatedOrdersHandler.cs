using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TalanLunch.Application.Dtos.Menu;
using TalanLunch.Application.Dtos.Order;
using TalanLunch.Application.Interfaces;
using TalanLunch.Application.Orders.Queries;

namespace TalanLunch.Application.Orders.Handlers
{
    public class GetPaginatedOrdersHandler : IRequestHandler<GetPaginatedOrdersQuery, PagedResult<OrderDayDto>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public GetPaginatedOrdersHandler(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task<PagedResult<OrderDayDto>> Handle(GetPaginatedOrdersQuery query, CancellationToken cancellationToken)
        {
            // Étape 1 : Récupération de la requête de base
            var ordersQuery = _orderRepository.GetAllOrdersQuery();

            // Étape 2 : Ajout des Includes séparément pour éviter les erreurs CS0266
            ordersQuery = ordersQuery
                .Include(o => o.User);

            ordersQuery = ordersQuery
                .Include(o => o.OrderDishes)
                .ThenInclude(od => od.Dish);

            // Étape 3 : Application des filtres de recherche
            if (!string.IsNullOrEmpty(query.FirstName))
            {
                ordersQuery = ordersQuery.Where(o => o.User.FirstName.Contains(query.FirstName));
            }

            if (!string.IsNullOrEmpty(query.LastName))
            {
                ordersQuery = ordersQuery.Where(o => o.User.LastName.Contains(query.LastName));
            }

            // Étape 4 : Tri par date
            var orderedQuery = ordersQuery.OrderByDescending(o => o.OrderDate);

            // Étape 5 : Pagination
            var totalItems = await orderedQuery.CountAsync(cancellationToken);

            var pagedOrders = await orderedQuery
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ProjectTo<OrderDayDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            // Étape 6 : Retour des résultats paginés
            return new PagedResult<OrderDayDto>
            {
                Items = pagedOrders,
                TotalItems = totalItems,
                PageNumber = query.PageNumber,
                PageSize = query.PageSize
            };
        }
    }
}
