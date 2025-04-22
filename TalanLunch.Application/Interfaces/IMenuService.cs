using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalanLunch.Application.Dtos;
using TalanLunch.Domain.Entities;

namespace TalanLunch.Application.Interfaces
{
    public interface IMenuService
    {
        Task<Menu> AddMenuAsync(MenuDto menuDto);
        Task<bool> UpdateMenuDescriptionAsync(int id, string newDescription);
        Task<(Menu?, bool)> AddDishToMenuAsync(int menuId, int dishId, int quantity);
        Task<Menu?> RemoveDishFromMenuAsync(int menuId, int dishId);
        Task DeleteMenuAsync(int id);
        Task<Menu?> GetMenuByIdAsync(int id);
        Task<IEnumerable<GetAllMenusDto>> GetAllMenusAsync();
        List<int> GetDishIdsForMenu(int menuId);
        Task<bool> SetMenuOfTheDayAsync(int menuId);
    



    }
}
