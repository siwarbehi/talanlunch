using AutoMapper;
using Microsoft.AspNetCore.Http;
using TalanLunch.Application.Dtos.Dish;
using TalanLunch.Application.Interfaces;
using TalanLunch.Domain.Entities;

namespace TalanLunch.Application.Services
{
    public class DishService : IDishService
    {
        private readonly IDishRepository _dishRepository;

        private readonly string _uploadsFolder;

        private readonly IMapper _mapper;


        public DishService(IDishRepository dishRepository, IMapper mapper)
        {
            _dishRepository = dishRepository;
            _uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
            _mapper = mapper;
        }
         //creation plat
        public async Task<Dish> AddDishAsync(DishDto dishDto)
        {
            if (dishDto == null)
                throw new ArgumentNullException(nameof(dishDto), "Dish data is null.");

            var dish = _mapper.Map<Dish>(dishDto);

            if (dishDto.DishPhoto != null && dishDto.DishPhoto.Length > 0)
            {
                string uniqueFileName = await SaveDishImageAsync(dishDto.DishPhoto);
                dish.DishPhoto = uniqueFileName;
            }

            // Données par défaut
            dish.OrderDate = DateTime.UtcNow;
            dish.ReviewCount = 0;
            dish.CurrentRating = 0;

            return await _dishRepository.AddDishAsync(dish);
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
    
        // Obtenir un plat par ID
        public async Task<Dish?> GetDishByIdAsync(int id)
        {
            return await _dishRepository.GetDishByIdAsync(id);
        }

        public async Task<Dish> UpdateDishAsync(Dish existingDish, DishUpdateDto updatedDish, IFormFile? dishPhoto)
        {
            _mapper.Map(updatedDish, existingDish);

            // Si une nouvelle note est fournie, recalculer la moyenne
            if (updatedDish.Rating.HasValue)
            {
                float oldRating = existingDish.CurrentRating;
                int oldCount = existingDish.ReviewCount;

                float newRating = ((oldRating * oldCount) + updatedDish.Rating.Value) / (oldCount + 1);
                existingDish.CurrentRating = newRating;
                existingDish.ReviewCount++;
            }

            if (dishPhoto != null && dishPhoto.Length > 0)
            {
                var fileName = await SaveDishImageAsync(dishPhoto);
                existingDish.DishPhoto = fileName;
            }

            return await _dishRepository.UpdateDishAsync(existingDish);
        }

        // Supprimer un plat
        public async Task DeleteDishAsync(int id)
        {
            var dish = await _dishRepository.GetDishByIdAsync(id);
            if (dish == null)
                throw new KeyNotFoundException($"Dish with ID {id} not found.");

            await _dishRepository.DeleteDishAsync(id);
        }
       

    }
}