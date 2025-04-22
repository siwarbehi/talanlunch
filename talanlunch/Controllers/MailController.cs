using Microsoft.AspNetCore.Mvc;
using TalanLunch.Application.Interfaces;
using TalanLunch.Application.Dtos;
using System.Threading.Tasks;

namespace talanlunch.Controllers
{
    [Route("api/mail")]
    [ApiController]
    public class MailController : ControllerBase
    {
        private readonly IMailService _mailService;

        public MailController(IMailService mailService)
        {
            _mailService = mailService;
        }

        [HttpPost]
        public async Task<IActionResult> SendEmail([FromBody] MailDataDto mailData)
        {
            if (mailData == null)
            {
                return BadRequest("Les données du mail sont manquantes.");
            }

            try
            {
                await _mailService.SendEmailAsync(mailData);
                return Ok("E-mail envoyé avec succès.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erreur lors de l'envoi de l'e-mail : {ex.Message}");
            }
        }
    }
}



