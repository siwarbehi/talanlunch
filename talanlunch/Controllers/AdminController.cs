using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TalanLunch.Application.Dtos;
using TalanLunch.Application.Interfaces;

namespace talanlunch.Controllers
{
    [Route("api/admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }// Endpoint pour récupérer tous les traiteurs en attente
        [HttpGet("pending-caterers")]
        public async Task<IActionResult> GetPendingCaterers()
        {
            var result = await _adminService.GetPendingCaterersAsync();
            if (result == null || !result.Any())
            {
                return NotFound("Aucun traiteur en attente.");
            }

            return Ok(result);
        }
        // Approuve un traiteur
        
        [HttpPut("approve-caterer")]
        public async Task<IActionResult> ApproveCaterer([FromBody] ApproveCatererDto approveCatererDto)
        {
            bool success = await _adminService.ApproveCatererAsync(approveCatererDto.UserId);
            if (!success)
            {
                return BadRequest("Impossible d'approuver le traiteur.");
            }

            return Ok("Le traiteur a été approuvé.");
        }

       
    }
}