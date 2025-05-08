using MediatR;
using Microsoft.AspNetCore.Mvc;
using TalanLunch.Application.Commands.Order;
using TalanLunch.Application.Dtos.Menu;
using TalanLunch.Application.Dtos.Order;
using TalanLunch.Application.Interfaces;
using TalanLunch.Application.Orders.Queries;


namespace talanlunch.API.Controllers
{
    [Route("api/order")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IMediator _mediator;


        public OrderController(IOrderService orderService, IMediator mediator) 
        {
            _orderService = orderService;
            _mediator = mediator;

        }

        /*  // Passer un ordre
          [HttpPost]
          public async Task<IActionResult> PlaceOrder([FromBody] OrderRequestDto request)
          {
              try
              {
                  var order = await _orderService.CreateOrderAsync(request);
                  return Ok(new { orderId = order.OrderId, message = "Commande confirmée !" });
              }
              catch (Exception ex)
              {
                  return BadRequest(new { message = ex.Message });
              }
          }*/
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


        /*  // GET: api/order
          [HttpGet]
          public async Task<IActionResult> GetAllOrders()
          {
              var orders = await _orderService.GetAllOrdersAsync();
              return Ok(orders);
          }*/

        // GET api/order
        /*        [HttpGet]
                public async Task<ActionResult<IEnumerable<OrderDayDto>>> GetAllOrders()
                {
                    var orders = await _mediator.Send(new GetAllOrdersQuery());
                    return Ok(orders);
                }*/
        /*
                [HttpGet]
                public async Task<ActionResult<IEnumerable<OrderDayDto>>> GetAllOrders(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] bool onlyUnpaid = false,
            [FromQuery] bool onlyPaidAndServed = false)
                {
                    var query = new GetAllOrdersQuery(pageNumber, pageSize, onlyUnpaid, onlyPaidAndServed);
                    var result = await _mediator.Send(query);
                    return Ok(result);
                }
        */
        [HttpGet]
        public async Task<ActionResult<PagedResult<OrderDayDto>>> GetAllOrders([FromQuery] GetAllOrdersQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }



        /*//paid & served
        [HttpPost("update-order-status")]
        public async Task<IActionResult> UpdateOrderStatus([FromBody] UpdateOrderStatusDto updateDto)
        {
            var result = await _orderService.UpdateOrderStatusAsync(updateDto);
            if (result)
            {
                return Ok("Notification envoyée avec succès.");
            }
            return BadRequest("Échec de la mise à jour de la commande.");
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

        /*  [HttpGet("unpaid")]
          public async Task<IActionResult> GetPaginatedOrders([FromQuery] PaginationQuery query)
          {
              var result = await _orderService.GetPaginatedOrdersAsync(query);
              return Ok(result);
          }*/

        [HttpGet("unpaid")]
        public async Task<IActionResult> UnpaidOrders([FromQuery] UnpaidOrdersQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

    }
}
