﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalanLunch.Domain.Entites;

namespace TalanLunch.Application.Interfaces
{
    public interface IDishRepository
    {
        Task<Dish> AddDishAsync(Dish dish);
        Task<IEnumerable<Dish>> GetAllDishesAsync();
        Task<Dish> GetDishByIdAsync(int id);
        Task UpdateDishAsync(Dish dish);
        Task DeleteDishAsync(int id);
       
    }
}