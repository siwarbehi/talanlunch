using TalanLunch.Domain.Entities;

namespace TalanLunch.Application.Interfaces
{
    public interface IAdminService
    {
        Task<List<Domain.Entities.User>> GetPendingCaterersAsync();
        Task<bool> ApproveCatererAsync(int id);
      


    }
}
