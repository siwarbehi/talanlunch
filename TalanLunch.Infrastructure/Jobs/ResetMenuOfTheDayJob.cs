using Microsoft.EntityFrameworkCore;
using Quartz;
using System.Threading.Tasks;
using TalanLunch.Infrastructure.Data; 

namespace TalanLunch.Application.Jobs
{
    public class ResetMenuOfTheDayJob : IJob
    {
        private readonly TalanLunchDbContext  _context; // Utilise le bon DbContext

        public ResetMenuOfTheDayJob(TalanLunchDbContext context)
        {
            _context = context;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var menus = await _context.Menus.ToListAsync();
            foreach (var menu in menus)
            {
                menu.IsMenuOfTheDay = false;
            }

            await _context.SaveChangesAsync();
        }
    }
}
