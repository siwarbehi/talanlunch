﻿using Microsoft.EntityFrameworkCore;
using TalanLunch.Application.Interfaces;
using TalanLunch.Domain.Entities;
using TalanLunch.Infrastructure.Data;
namespace TalanLunch.Infrastructure.Repos
{
    public class MenuRepository : IMenuRepository
    {
        private readonly TalanLunchDbContext _context;
        public MenuRepository(TalanLunchDbContext context)
        {
            _context = context;
        }
        public async Task<Menu> AddMenuAsync(Menu menu)
        {
            try
            {
                _context.Menus.Add(menu);
                await _context.SaveChangesAsync();
                return menu;
            }
            catch (Exception ex)
            {
                throw new Exception("Une erreur s'est produite lors de l'ajout du menu.", ex);
            }
        }
        public async Task<Menu> UpdateMenuAsync(Menu menu)
        {
            _context.Menus.Update(menu);
            await _context.SaveChangesAsync();
            return menu;
        }
        public async Task DeleteMenuAsync(int id)
        {
            var menu = await _context.Menus
                .Include(m => m.MenuDishes)
                .FirstOrDefaultAsync(m => m.MenuId == id);
            if (menu != null)
            {
                // Supprimer les liaisons MenuDish
                _context.MenuDishes.RemoveRange(menu.MenuDishes);
                // Puis supprimer le menu
                _context.Menus.Remove(menu);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<Menu?> GetMenuByIdAsync(int id)
        {
            return await _context.Menus
                .Include(m => m.MenuDishes)
                .ThenInclude(md => md.Dish)
                .FirstOrDefaultAsync(m => m.MenuId == id);
        }
        public async Task<IEnumerable<Menu>> GetAllMenusAsync()
        {
            return await _context.Menus
               .Include(m => m.MenuDishes)
               .ThenInclude(md => md.Dish)
               .ToListAsync();
        }
        public async Task<IEnumerable<Menu>> GetAllMenus()
        {
            return await _context.Menus.ToListAsync();
        }
        public async Task<Menu> GetMenuOfTheDayAsync()
        {
            return await _context.Menus.FirstOrDefaultAsync(menu => menu.IsMenuOfTheDay);
        }

    }
}