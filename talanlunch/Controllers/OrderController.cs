using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TalanLunch.Application.Interfaces;
using TalanLunch.Application.Dtos;


namespace talanlunch.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService) 
        {
            _orderService = orderService;
        }

        // Passer un ordre
        [HttpPost("place")]
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
        [HttpGet("orders-by-date")]
        public async Task<IActionResult> GetOrdersByDate([FromQuery] OrderDateRequest request)
        {
            var orders = await _orderService.GetOrdersByDateAsync(request.Date);
            return Ok(orders);

        }
        //paid & served
        [HttpPut("update-status")]
        public async Task<IActionResult> UpdateOrderStatus([FromBody] UpdateOrderStatusDto dto)
        {
            var result = await _orderService.UpdateOrderStatusAsync(dto);
            if (!result) return NotFound("Order not found");

            return Ok("Order status updated successfully.");
        }



    }
}
