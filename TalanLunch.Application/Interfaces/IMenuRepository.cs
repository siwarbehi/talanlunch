using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalanLunch.Application.Dtos;
using TalanLunch.Domain.Entities;

namespace TalanLunch.Application.Interfaces
{
    public interface IMenuRepository
    {

        Task<Menu> AddMenuAsync(Menu menu);
        Task<Menu> UpdateMenuAsync(Menu menu);
        Task DeleteMenuAsync(int id);
        Task<Menu?> GetMenuByIdAsync(int id);
        Task<IEnumerable<GetAllMenusDto>> GetAllMenusAsync();
        Task<IEnumerable<Menu>> GetAllMenus();
        List<int> GetDishIdsByMenuId(int menuId); // Méthode pour récupérer les DishId par MenuId


    }
}