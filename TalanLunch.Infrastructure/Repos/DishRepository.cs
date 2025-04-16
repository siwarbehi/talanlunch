using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalanLunch.Infrastructure.Data;
using TalanLunch.Domain.Entities;
using TalanLunch.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace TalanLunch.Infrastructure.Repos
{
    public class DishRepository : IDishRepository
    {
        private readonly TalanLunchDbContext _context;      

        public DishRepository(TalanLunchDbContext context)
        {
            _context = context;
        }

        public async Task<Dish> AddDishAsync(Dish dish)
        {
            _context.Dishes.Add(dish);
            await _context.SaveChangesAsync();
            return dish;
        }

        public async Task<Dish?> GetDishByIdAsync(int dishId)
        {
            var dish = await _context.Dishes.FindAsync(dishId);
           
            return dish;
        }
        public async Task<Dish> UpdateDishAsync(Dish updatedDish)
        {
            _context.Dishes.Update(updatedDish);
            await _context.SaveChangesAsync();
            return updatedDish;
        }

        public async Task<List<int>> GetExistingDishIdsAsync()
        {
            return await _context.Dishes
                .Select(d => d.DishId)
                .ToListAsync();
        }
        public async Task DeleteDishAsync(int id)
        {
            var dish = await _context.Dishes
                .Include(d => d.MenuDishes) 
                .FirstOrDefaultAsync(d => d.DishId == id);

            if (dish != null)
            {
                // Supprimer les liaisons avec les menus
                if (dish.MenuDishes != null && dish.MenuDishes.Any())
                {
                    _context.MenuDishes.RemoveRange(dish.MenuDishes);
                }

                // Supprimer le plat lui-même
                _context.Dishes.Remove(dish);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Dish>> GetAllDishesAsync()
        {
            return await _context.Dishes.ToListAsync();
        }
        public async Task<IEnumerable<Dish>> GetAllDishesWithRelationsAsync()
        {
            return await _context.Dishes
                .Include(d => d.MenuDishes)  
                .Include(d => d.OrderDishes) 
                .ToListAsync();
        }

    }
}