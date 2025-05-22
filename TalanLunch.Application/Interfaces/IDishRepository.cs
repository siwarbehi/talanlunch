using TalanLunch.Domain.Entities;

namespace TalanLunch.Application.Interfaces
{
    public interface IDishRepository
    {
        Task<Dish> AddDishAsync(Dish dish);
        Task<IEnumerable<Dish>> GetAllDishesAsync();
        Task<Dish?> GetDishByIdAsync(int? id); 
        Task<Dish> UpdateDishAsync(Dish updatedDish);
        Task DeleteDishAsync(int id);
        Task<List<Dish>> GetDishesByIdsAsync(IEnumerable<int> dishIds);
    }
}