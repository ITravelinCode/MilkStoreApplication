using Business;
using Business.Models.OrderView;
using Business.Services.Implements;
using Business.Services.Interfaces;
using DataAccess.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MilkStoreApplication.Controllers
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
        [HttpGet("/api/v1/order")]
        public async Task<IActionResult> CreateOrder()
        {
            try
            {
                var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                if (token != null)
                {
                    var accountId = Util.GetInstance().ValidateJwtToken(token);
                    if (accountId.HasValue)
                    {
                        var result = await _orderService.CreateOrder(accountId.Value);
                        return Ok(result);
                    }
                    return Unauthorized("Not found account's information in token");
                }
                else
                {
                    return Unauthorized("Please login account");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Inner error: {ex.Message}");
            }
        }
    }
}
