using MediatR;
using Microsoft.AspNetCore.Mvc;
using TalanLunch.Application.Orders.Commands.PlaceOrder;
using TalanLunch.Application.Orders.Commands.UpdateOrderStatus;
using TalanLunch.Application.Orders.Queries.GetAllOrders;




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
        public async Task<IActionResult> PlaceOrder([FromBody] PlaceOrderCommand command)
        {
            try
            {
                var order = await _mediator.Send(command);
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
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPatch("update-order-status")]
        public async Task<IActionResult> UpdateOrderStatus([FromBody] UpdateOrderStatusCommand command)
        {
            bool success = await _mediator.Send(command);
            if (success)
                return Ok("Notification envoyée avec succès.");
            return BadRequest("Échec de la mise à jour de la commande.");
        }


    

    }
}
