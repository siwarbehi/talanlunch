using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using TalanLunch.Application.Interfaces;
using TalanLunch.Domain.Entities;
using TalanLunch.Application.Dishes.Commands.UpdateDish;

namespace TalanLunch.Application.Handlers.DishHandlers
{

    public class UpdateDishCommandHandler : IRequestHandler<UpdateDishCommand, Dish?>
    {
        private readonly IDishRepository _dishRepository;
        private readonly IMapper _mapper;

        public UpdateDishCommandHandler(IDishRepository dishRepository, IMapper mapper)
        {
            _dishRepository = dishRepository;
            _mapper = mapper;
        }

        public async Task<Dish?> Handle(UpdateDishCommand request, CancellationToken cancellationToken)
        {
            var existing = await _dishRepository.GetDishByIdAsync(request.DishId);
            if (existing == null) return null;

            _mapper.Map(request, existing);

            if (request.Rating.HasValue)
            {
                float oldRating = existing.CurrentRating;
                int oldCount = existing.ReviewCount;
                float newRating = ((oldRating * oldCount) + request.Rating.Value)
                                   / (oldCount + 1);
                existing.CurrentRating = newRating;
                existing.ReviewCount++;
            }

            if (request.Photo is IFormFile photo && photo.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "dishes");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                string uniqueFileName = $"{Guid.NewGuid()}_{photo.FileName}";
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                await using var stream = new FileStream(filePath, FileMode.Create);
                await photo.CopyToAsync(stream, cancellationToken);

                existing.DishPhoto = uniqueFileName;
            }

            return await _dishRepository.UpdateDishAsync(existing);
        }

    }
}
