using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalanLunch.Application.Interfaces;
using TalanLunch.Domain.Entities;

namespace TalanLunch.Application.Services
{
    public class AdminService : IAdminService
    {
        private readonly IUserRepository _userRepository; 
        private readonly IMailService _mailService; 

        public AdminService(IUserRepository userRepository, IMailService mailService)
        {
            _userRepository = userRepository;
            _mailService = mailService;
        }

        // Récupérer tous les traiteurs en attente
        public async Task<List<User>> GetPendingCaterersAsync()
        {
            return await _userRepository.GetPendingCaterersAsync();
        }

        public async Task<bool> ApproveCatererAsync(int id)
        {
            var caterer = await _userRepository.GetCatererByIdAsync(id);
            if (caterer == null || caterer.IsApproved) return false;

            caterer.IsApproved = true;

            // Appeler le service pour mettre à jour le statut du traiteur
            bool updateSuccess = await _userRepository.ApproveUserAsync(caterer);

            if (!updateSuccess)
            {
                return false;
            }

            // Créer les données de l'email via le service de mail
            var mailData = _mailService.CreateMailDataForApproval(caterer);

            // Appeler le service de mail pour envoyer l'email
            await _mailService.SendEmailAsync(mailData);

            return true;
        }

        public async Task<bool> ApproveMultipleCaterersAsync(List<int> catererIds)
        {
            var caterers = await _userRepository.GetCaterersByIdsAsync(catererIds);
            if (caterers == null || caterers.Count == 0) return false;

            foreach (var caterer in caterers)
            {
                caterer.IsApproved = true;
            }

            return await _userRepository.UpdateMultipleCaterersAsync(caterers);
        }

         
        
    }
}