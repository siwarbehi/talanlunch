using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TalanLunch.Application.Dtos;
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

        //public async Task<IEnumerable<Menu>> GetAllMenusAsync()
        // {
        public async Task<IEnumerable<GetAllMenusDto>> GetAllMenusAsync()
        {
            return await _context.Menus
                .Include(m => m.MenuDishes)
                    .ThenInclude(md => md.Dish)
                .Select(menu => new GetAllMenusDto
                {
                    MenuId = menu.MenuId,
                    MenuDescription = menu.MenuDescription,
                    IsMenuOfTheDay = menu.IsMenuOfTheDay,
                    Dishes = menu.MenuDishes.Select(md => new DishMenuAllDto
                    {
                        DishId = md.Dish.DishId,
                        DishName = md.Dish.DishName,
                        DishQuantity = md.DishQuantity,
                        DishPrice = md.Dish.DishPrice,
                        DishPhoto = md.Dish.DishPhoto,
                        DishDescription = md.Dish.DishDescription
                    }).ToList()
                })
                .ToListAsync();
        }


        //.Include(m => m.MenuDishes)
        //.ThenInclude(md => md.Dish)
        //.ToListAsync();
        //}
        public List<int> GetAllMenuIds()
        {
            return _context.Menus
                .Select(menu => menu.MenuId) 
                .ToList();
        }
        // Méthode pour récupérer tous les DishId associés à un MenuId
        public List<int> GetDishIdsByMenuId(int menuId)
        {
            return _context.MenuDishes
                .Where(md => md.MenuId == menuId)
                .Select(md => md.DishId)
                .ToList();
        }
    }

    }