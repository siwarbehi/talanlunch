/*using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TalanLunch.Application.Dtos;
using TalanLunch.Application.Dtos.Order;
using TalanLunch.Application.Interfaces;
using TalanLunch.Application.Orders.Queries;

namespace TalanLunch.Application.Orders.Handlers
{
    public class UnpaidOrdersHandler : IRequestHandler<UnpaidOrdersQuery, PagedResult<OrderDayDto>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public UnpaidOrdersHandler(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task<PagedResult<OrderDayDto>> Handle(UnpaidOrdersQuery query, CancellationToken cancellationToken)
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
            if (!string.IsNullOrWhiteSpace(query.FirstName))
            {
                var firstNameLower = query.FirstName.ToLower();
                ordersQuery = ordersQuery.Where(o => o.User.FirstName.ToLower().StartsWith(firstNameLower));
            }

            if (!string.IsNullOrWhiteSpace(query.LastName))
            {
                var lastNameLower = query.LastName.ToLower();
                ordersQuery = ordersQuery.Where(o => o.User.LastName.ToLower().StartsWith(lastNameLower));
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
*/