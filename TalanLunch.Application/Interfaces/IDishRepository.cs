using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalanLunch.Domain.Entities;

namespace TalanLunch.Application.Interfaces
{
    public interface IDishRepository
    {
        Task<Dish> AddDishAsync(Dish dish);
        Task<IEnumerable<Dish>> GetAllDishesAsync();
        Task<Dish?> GetDishByIdAsync(int id); 
        Task<Dish> UpdateDishAsync(Dish updatedDish);
        Task<List<int>> GetExistingDishIdsAsync();
        Task DeleteDishAsync(int id);


    }
}