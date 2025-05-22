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
        private readonly IBlobStorageService _blobStorageService;

        public UpdateDishCommandHandler(IDishRepository dishRepository, IMapper mapper, IBlobStorageService blobStorageService)
        {
            _dishRepository = dishRepository;
            _mapper = mapper;
            _blobStorageService = blobStorageService;
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
                float newRating = ((oldRating * oldCount) + request.Rating.Value) / (oldCount + 1);

                existing.CurrentRating = newRating;
                existing.ReviewCount++;
            }

            if (request.Photo is IFormFile photo && photo.Length > 0)
            {
                string containerName = "dishimages";
                string imageUrl = await _blobStorageService.UploadFileAsync(photo, containerName);
                existing.DishPhoto = imageUrl;
            }

            return await _dishRepository.UpdateDishAsync(existing);
        }
    }
}
