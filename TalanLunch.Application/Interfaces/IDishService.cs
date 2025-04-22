using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalanLunch.Domain.Entities;
using Microsoft.AspNetCore.Http;
using TalanLunch.Application.Dtos;




namespace TalanLunch.Application.Interfaces
{
    public interface IDishService
    {
        Task<Dish> AddDishAsync(DishDto dishDto);
        Task<IEnumerable<Dish>> GetAllDishesAsync();
        Task<Dish> UpdateDishAsync(Dish existingDish, DishUpdateDto updatedDish, IFormFile? dishPhoto);

        Task<Dish?> GetDishByIdAsync(int id);
        Task DeleteDishAsync(int id);
        Task RateDishAsync(RateDishDto dto);
    }
}
