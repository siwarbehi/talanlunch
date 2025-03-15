using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalanLunch.Domain.Entities;

namespace TalanLunch.Application.Interfaces
{
    public interface IAdminService
    {
        Task<List<User>> GetPendingCaterersAsync();
        Task<bool> ApproveCatererAsync(int id);
        Task<bool> ApproveMultipleCaterersAsync(List<int> catererIds);



    }
}
