using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using TalanLunch.Application.Interfaces;
using TalanLunch.Domain.Entities;
using System.Security.Claims;
using TalanLunch.Application.Dtos;

namespace TalanLunch.Application.Services
{
    public class DishService : IDishService
    {
        private readonly IDishRepository _dishRepository;

        private readonly string _uploadsFolder;



        public DishService(IDishRepository dishRepository)
        {
            _dishRepository = dishRepository;
            _uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");

        }

        // Ajouter un plat

        public async Task<Dish> AddDishAsync(DishDto dishDto)
        {
            if (dishDto == null)
                throw new ArgumentNullException(nameof(dishDto), "Dish data is null.");

            var newDish = new Dish
            {
                DishName = dishDto.DishName,
                DishDescription = dishDto.DishDescription,
                DishQuantity = dishDto.DishQuantity,
                DishPrice = dishDto.DishPrice,
                IsSalad = dishDto.IsSalad,
                OrderDate = DateTime.UtcNow, // Date actuelle
                ReviewCount = 0,
                CurrentRating = 0,
            };

            if (dishDto.DishPhoto != null && dishDto.DishPhoto.Length > 0)
            {
                string uniqueFileName = await SaveDishImageAsync(dishDto.DishPhoto);
                newDish.DishPhoto = uniqueFileName; 
            }

            return await _dishRepository.AddDishAsync(newDish);
        }

        // Méthode pour sauvegarder l'image du plat
        private async Task<string> SaveDishImageAsync(IFormFile dishImage)
        {
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "dishes");

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            string uniqueFileName = $"{Guid.NewGuid()}_{dishImage.FileName}";
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await dishImage.CopyToAsync(stream);
            }

            return uniqueFileName;
        }



        // Obtenir tous les plats
        public async Task<IEnumerable<Dish>> GetAllDishesAsync()
        {
            return await _dishRepository.GetAllDishesAsync();
        }
        // Obtenir tous les plats avec relations avec commandes et menus
        public async Task<IEnumerable<Dish>> GetAllDishesWithRelationsAsync()
        {
            return await _dishRepository.GetAllDishesWithRelationsAsync();
        }
        // Obtenir un plat par ID
        public async Task<Dish?> GetDishByIdAsync(int id)
        {
            return await _dishRepository.GetDishByIdAsync(id);
        }

        // Mettre à jour un plat


        // Mise à jour d'un plat avec gestion de la photo et des propriétés
        public async Task<Dish> UpdateDishAsync(Dish existingDish, DishUpdateDto updatedDish, IFormFile? dishPhoto)
        {
            // Mettre à jour les propriétés du plat seulement si elles sont spécifiées
            if (!string.IsNullOrEmpty(updatedDish.DishName))
                existingDish.DishName = updatedDish.DishName;

            if (!string.IsNullOrEmpty(updatedDish.DishDescription))
                existingDish.DishDescription = updatedDish.DishDescription;

            if (updatedDish.DishQuantity.HasValue && updatedDish.DishQuantity >= 0)
                existingDish.DishQuantity = updatedDish.DishQuantity.Value;

            if (updatedDish.DishPrice.HasValue && updatedDish.DishPrice >= 0)
                existingDish.DishPrice = (decimal)updatedDish.DishPrice.Value;

            if (updatedDish.IsSalad.HasValue)
                existingDish.IsSalad = updatedDish.IsSalad.Value;

            // Gestion de la photo du plat
            if (dishPhoto != null && dishPhoto.Length > 0)
            {
                var fileName = await SaveDishImageAsync(dishPhoto);
                existingDish.DishPhoto = fileName;
            }
          
            // Mise à jour du plat dans la base de données via le repository
            return await _dishRepository.UpdateDishAsync(existingDish);
        }
        // Supprimer un plat
        public async Task DeleteDishAsync(int id)
        {
            // Vérification si le plat existe
            var dish = await _dishRepository.GetDishByIdAsync(id);
            if (dish == null)
            {
                throw new KeyNotFoundException($"Dish with ID {id} not found.");
            }

            // Suppression du plat dans le repository
            await _dishRepository.DeleteDishAsync(id);
        }
    }
}