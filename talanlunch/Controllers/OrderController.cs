using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TalanLunch.Application.Orders.Commands.PlaceOrder;
using TalanLunch.Application.Orders.Commands.UpdateOrderStatus;
using TalanLunch.Application.Orders.Queries.GetAllOrders;

namespace Talanlunch.API.Controllers
{
    [Authorize]
    [Route("api/order")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> PlaceOrder([FromBody] PlaceOrderCommand command)
        {
            try
            {
                var order = await _mediator.Send(command).ConfigureAwait(false);
                return Ok(new { orderId = order.OrderId, message = "Commande confirmée !" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<OrderDay>>> GetAllOrders([FromQuery] GetAllOrdersQuery query)
        {
            var result = await _mediator.Send(query).ConfigureAwait(false);
            return Ok(result);
        }

        [HttpPatch("update-order-status")]
        public async Task<IActionResult> UpdateOrderStatus([FromBody] UpdateOrderStatusCommand command)
        {
            bool success = await _mediator.Send(command).ConfigureAwait(false);
            return success ? Ok("Notification envoyée avec succès.") : BadRequest("Échec de la mise à jour de la commande.");
        }
    }
}
