using Microsoft.EntityFrameworkCore;
using Quartz;
using TalanLunch.Application.Interfaces;
using TalanLunch.Infrastructure.Data;

namespace TalanLunch.Infrastructure.Jobs
{
    public class ResetMenuOfTheDayJob : IJob
    {
        private readonly TalanLunchDbContext  _context;
        private readonly IMenuRepository _menuRepository;


        public ResetMenuOfTheDayJob(TalanLunchDbContext context , IMenuRepository menuRepository)
        {
            _context = context;
            _menuRepository = menuRepository;

        }

        public async Task Execute(IJobExecutionContext context)
        {
            var menus = await _context.Menus.ToListAsync();
            var previousMenu = menus.FirstOrDefault(menu => menu.IsMenuOfTheDay == true);

            if (previousMenu != null)
            {
                previousMenu.IsMenuOfTheDay = false;
                await _menuRepository.UpdateMenuAsync(previousMenu);
            }

        }
    }
}
