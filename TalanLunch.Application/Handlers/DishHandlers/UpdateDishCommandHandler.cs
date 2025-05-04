// Application/Handlers/DishHandlers/UpdateDishCommandHandler.cs
using AutoMapper;
using MediatR;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using TalanLunch.Application.Commands.Dish;
using TalanLunch.Application.Interfaces;
using TalanLunch.Domain.Entities;
using Microsoft.AspNetCore.Http;

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
            // 1. Récupération de l’entité existante
            var existing = await _dishRepository.GetDishByIdAsync(request.DishId);
            if (existing == null) return null;

            // 2. Mapping des champs non nuls du DTO vers l’entité
            _mapper.Map(request.UpdateDto, existing);

            // 3. Recalcul de la note si fourni
            if (request.UpdateDto.Rating.HasValue)
            {
                float oldRating = existing.CurrentRating;
                int oldCount = existing.ReviewCount;
                float newRating = ((oldRating * oldCount) + request.UpdateDto.Rating.Value)
                                   / (oldCount + 1);
                existing.CurrentRating = newRating;
                existing.ReviewCount++;
            }

            // 4. Sauvegarde de l’image si fournie
            if (request.Photo is IFormFile photo && photo.Length > 0)
            {
                var uploadsFolder = Path.Combine(
                    Directory.GetCurrentDirectory(), "wwwroot", "dishes");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                string uniqueFileName = $"{Guid.NewGuid()}_{photo.FileName}";
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                await using var stream = new FileStream(filePath, FileMode.Create);
                await photo.CopyToAsync(stream, cancellationToken);

                existing.DishPhoto = uniqueFileName;
            }

            // 5. Persistance
            return await _dishRepository.UpdateDishAsync(existing);
        }
    }
}
