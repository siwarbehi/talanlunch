using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalanLunch.Infrastructure.Data;
using TalanLunch.Domain.Entites;
using TalanLunch.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

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

      

        public async Task<Dish> GetDishByIdAsync(int id)
        {
            return await _context.Dishes.FindAsync(id);
        }

        public async Task UpdateDishAsync(Dish dish)
        {
            _context.Dishes.Update(dish);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteDishAsync(int id)
        {
            var dish = await _context.Dishes.FindAsync(id);
            if (dish != null)
            {
                _context.Dishes.Remove(dish);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<IEnumerable<Dish>> GetAllDishesAsync()
        {
            return await _context.Dishes.ToListAsync();
        }
    }
}