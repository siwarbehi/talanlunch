using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalanLunch.Application.Interfaces;
using TalanLunch.Domain.Entites;

namespace TalanLunch.Application.Services
{
    public class DishService
    {
        private readonly IDishRepository _dishRepository;

        public DishService(IDishRepository dishRepository)
        {
            _dishRepository = dishRepository;
        }

        // Ajouter un plat
        public async Task<Dish> AddDishAsync(Dish dish)
        {
            return await _dishRepository.AddDishAsync(dish);
        }

        // Obtenir tous les plats
        public async Task<IEnumerable<Dish>> GetAllDishesAsync()
        {
            return await _dishRepository.GetAllDishesAsync();
        }

        // Obtenir un plat par ID
        public async Task<Dish> GetDishByIdAsync(int id)
        {
            return await _dishRepository.GetDishByIdAsync(id);
        }

        // Mettre à jour un plat
        public async Task UpdateDishAsync(Dish dish)
        {
            await _dishRepository.UpdateDishAsync(dish);
        }

        // Supprimer un plat
        public async Task DeleteDishAsync(int id)
        {
            await _dishRepository.DeleteDishAsync(id);
        }
    }
}