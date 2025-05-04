// Application/Handlers/DishHandlers/AddDishCommandHandler.cs
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using TalanLunch.Application.Commands.Dish;
using TalanLunch.Application.Interfaces;
using TalanLunch.Domain.Entities;

namespace TalanLunch.Application.Handlers.DishHandlers
{
   
    public class AddDishCommandHandler : IRequestHandler<AddDishCommand, Dish>
    {
        private readonly IDishRepository _dishRepository;
        private readonly IMapper _mapper;

        public AddDishCommandHandler(IDishRepository dishRepository, IMapper mapper)
        {
            _dishRepository = dishRepository;
            _mapper = mapper;
        }

        public async Task<Dish> Handle(AddDishCommand request, CancellationToken cancellationToken)
        {
            var dto = request.DishDto ?? throw new ArgumentNullException(nameof(request.DishDto));

            // Mappe DTO → entité
            var dish = _mapper.Map<Dish>(dto);

            // Sauvegarde de l’image si fournie
            if (dto.DishPhoto is IFormFile photo && photo.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "dishes");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                string uniqueFileName = $"{Guid.NewGuid()}_{photo.FileName}";
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using var stream = new FileStream(filePath, FileMode.Create);
                await photo.CopyToAsync(stream, cancellationToken);

                dish.DishPhoto = uniqueFileName;
            }

            // Valeurs par défaut
            dish.OrderDate = DateTime.UtcNow;
            dish.ReviewCount = 0;
            dish.CurrentRating = 0;

            // Persistance
            return await _dishRepository.AddDishAsync(dish);
        }
    }
}
