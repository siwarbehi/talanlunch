using Microsoft.AspNetCore.Mvc;
using TalanLunch.Application.Dtos.Order;
using TalanLunch.Application.Interfaces;


namespace talanlunch.API.Controllers
{
    [Route("api/order")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService) 
        {
            _orderService = orderService;
        }

        // Passer un ordre
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
        }

        //get all orders by day 
        /* [HttpGet("orders-by-date")]
        public async Task<IActionResult> GetOrdersByDate([FromQuery] OrderDateRequest request)
        {
            var orders = await _orderService.GetOrdersByDateAsync(request.Date);
            return Ok(orders);

        } */
        // GET: api/order
        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return Ok(orders);
        }

        //paid & served
        [HttpPost("update-order-status")]
        public async Task<IActionResult> UpdateOrderStatus([FromBody] UpdateOrderStatusDto updateDto)
        {
            var result = await _orderService.UpdateOrderStatusAsync(updateDto);
            if (result)
            {
                return Ok("Notification envoyée avec succès.");
            }
            return BadRequest("Échec de la mise à jour de la commande.");
        }

        [HttpGet("unpaid")]
        public async Task<IActionResult> GetPaginatedOrders([FromQuery] PaginationQuery query)
        {
            var result = await _orderService.GetPaginatedOrdersAsync(query);
            return Ok(result);
        }

       
    }
}
