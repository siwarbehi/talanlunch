using Microsoft.AspNetCore.Http;
using TalanLunch.Application.Dtos.Dish;
using TalanLunch.Domain.Entities;




namespace TalanLunch.Application.Interfaces
{
    public interface IDishService
    {
        Task<Dish> AddDishAsync(DishDto dishDto);
        Task<IEnumerable<Dish>> GetAllDishesAsync();
        Task<Dish> UpdateDishAsync(Dish existingDish, DishUpdateDto updatedDish, IFormFile? dishPhoto);

        Task<Dish?> GetDishByIdAsync(int id);
        Task DeleteDishAsync(int id);
    }
}
