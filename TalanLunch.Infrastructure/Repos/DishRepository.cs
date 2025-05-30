﻿using Microsoft.EntityFrameworkCore;
using TalanLunch.Application.Interfaces;
using TalanLunch.Domain.Entities;
using TalanLunch.Infrastructure.Data;

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

        public async Task<Dish?> GetDishByIdAsync(int? dishId)
        {
            if (dishId == null) return null;

            var dish = await _context.Dishes.FindAsync(dishId);
           
            return dish;
        }
        public async Task<Dish> UpdateDishAsync(Dish updatedDish)
        {
            _context.Dishes.Update(updatedDish);
            await _context.SaveChangesAsync();
            return updatedDish;
        }
        public async Task<IEnumerable<Dish>> GetAllDishesAsync()
        {
            return await _context.Dishes.ToListAsync();
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

        public async Task<List<Dish>> GetDishesByIdsAsync(IEnumerable<int> dishIds)
        {
            return await _context.Dishes
                .Where(d => dishIds.Contains(d.DishId))
                .ToListAsync();
        }
    }
}