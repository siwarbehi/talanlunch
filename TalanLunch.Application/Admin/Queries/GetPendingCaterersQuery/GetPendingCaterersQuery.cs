using MediatR;

namespace TalanLunch.Application.Admin.Queries.GetPendingCaterersQuery
{
    public class GetPendingCaterersQuery : IRequest<List<GetCatererQueryResult>>
    {
    }
}
