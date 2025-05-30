using Quartz;
using TalanLunch.Application.Interfaces;

namespace TalanLunch.Infrastructure.Jobs
{
    public class ResetMenuOfTheDayJob : IJob
    {
        private readonly IMenuRepository _menuRepository;

        public ResetMenuOfTheDayJob(IMenuRepository menuRepository)
        {
            _menuRepository = menuRepository;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var previousMenu = await _menuRepository.GetMenuOfTheDayAsync()
                .ConfigureAwait(false);

            if (previousMenu != null)
            {
                previousMenu.IsMenuOfTheDay = false;
                await _menuRepository.UpdateMenuAsync(previousMenu)
                    .ConfigureAwait(false);
            }
        }
    }
}
