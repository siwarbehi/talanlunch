using TalanLunch.Application.Dtos.Menu;
using TalanLunch.Domain.Entities;

namespace TalanLunch.Application.Interfaces
{
    public interface IMenuService
    {
        Task<Menu> AddMenuAsync(MenuDto menuDto);
      
        Task<Menu?> RemoveDishFromMenuAsync(int menuId, int dishId);
        Task DeleteMenuAsync(int id);
        Task<Menu?> GetMenuByIdAsync(int id);
        Task<IEnumerable<GetAllMenusDto>> GetAllMenusAsync();
       // List<int> GetDishIdsForMenu(int menuId);
        Task<bool> SetMenuOfTheDayAsync(int menuId);
        Task<AddDishToMenuResult?> AddDishToMenuAsync(int menuId, int dishId, int quantity, string? newDescription = null);





    }
}
