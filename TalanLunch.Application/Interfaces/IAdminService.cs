using TalanLunch.Domain.Entities;

namespace TalanLunch.Application.Interfaces
{
    public interface IAdminService
    {
        Task<List<User>> GetPendingCaterersAsync();
        Task<bool> ApproveCatererAsync(int id);
      


    }
}
