using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using TalanLunch.Application.Interfaces;
using TalanLunch.Domain.Entities;


namespace TalanLunch.Application.Dishes.Commands.AddDish
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
            // Mapper la commande vers l'entité Dish
            var dish = _mapper.Map<Dish>(request);

            // Sauvegarde de l’image si fournie
            if (request.DishPhoto is IFormFile photo && photo.Length > 0)
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

            return await _dishRepository.AddDishAsync(dish);
        }
    }
}
