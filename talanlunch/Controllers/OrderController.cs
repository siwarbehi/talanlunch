using MediatR;
using Microsoft.AspNetCore.Mvc;
using TalanLunch.Application.Commands.Order;
using TalanLunch.Application.Dtos.Order;


namespace talanlunch.API.Controllers
{
    [Route("api/order")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;


        public OrderController(IMediator mediator) 
        {
            _mediator = mediator;

        }

        // POST api/order
        [HttpPost]
        public async Task<IActionResult> PlaceOrder([FromBody] OrderRequestDto request)
        {
            try
            {
                var order = await _mediator.Send(new PlaceOrderCommand(request));
                return Ok(new { orderId = order.OrderId, message = "Commande confirmée !" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

      /*  [HttpGet]
        public async Task<ActionResult<PagedResult<OrderDayDto>>> GetAllOrders([FromQuery] GetAllOrdersQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }*/

        // POST api/order/update-order-status
        [HttpPost("update-order-status")]
        public async Task<IActionResult> UpdateOrderStatus([FromBody] UpdateOrderStatusDto updateDto)
        {
            bool success = await _mediator.Send(new UpdateOrderStatusCommand(updateDto));
            if (success)
                return Ok("Notification envoyée avec succès.");
            return BadRequest("Échec de la mise à jour de la commande.");
        }


        /*[HttpGet("unpaid")]
        public async Task<IActionResult> UnpaidOrders([FromQuery] UnpaidOrdersQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }*/

    }
}
