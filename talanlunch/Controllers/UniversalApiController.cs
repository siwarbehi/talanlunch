/*using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace TalanLunch.API.Controllers
{
    // Cette classe doit également être publique
    [Route("api/universalapi")]
    [ApiController]
    public class UniversalApiController : BaseMediatorController
    {
        public UniversalApiController(IMediator mediator) : base(mediator) { }

        // Point d'entrée pour la gestion des requêtes via le corps de la requête
        [HttpPost]
        public async Task<IActionResult> HandleRequest([FromBody] ApiRequest request)
        {
            return await base.HandleMediatorRequest(request);
        }
    }
}
*/