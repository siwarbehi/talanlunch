using AutoMapper;
using TalanLunch.Application.Admin.Queries.GetPendingCaterersQuery;
using TalanLunch.Domain.Entities;

namespace TalanLunch.Application.Mapping
{
    public class AdminProfile : Profile
    { public AdminProfile()
        {
        CreateMap<User, GetCatererQueryResult>();
            }
    }
}
