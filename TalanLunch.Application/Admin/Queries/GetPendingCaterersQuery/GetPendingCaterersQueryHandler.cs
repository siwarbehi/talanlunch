using AutoMapper;
using MediatR;
using TalanLunch.Application.Interfaces;

namespace TalanLunch.Application.Admin.Queries.GetPendingCaterersQuery
{
    public class GetPendingCaterersHandler : IRequestHandler<GetPendingCaterersQuery, List<GetCatererQueryResult>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetPendingCaterersHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<List<GetCatererQueryResult>> Handle(GetPendingCaterersQuery request, CancellationToken cancellationToken)
        {
            var users = await _userRepository.GetPendingCaterersAsync();
            return _mapper.Map<List<GetCatererQueryResult>>(users);
        }
    }
}
