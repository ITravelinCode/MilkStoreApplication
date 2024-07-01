using Business;
using Business.Models.CartView;
using Business.Services.Interfaces;
using DataAccess.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace MilkStoreApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet("/api/v1/cart")]
        [Authorize(Policy = "CustomerPolicy")]
        public async Task<IActionResult> GetCartOfAccount()
        {
            try
            {
                var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                if (token != null)
                {
                    var accountId = Util.GetInstance().ValidateJwtToken(token);
                    if (accountId.HasValue)
                    {
                        var result = await _cartService.GetCartsByAccountId(accountId.Value);
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

        [HttpPost("/api/v1/cart")]
        [Authorize(Policy = "CustomerPolicy")]
        public async Task<IActionResult> AddProductToCart([FromBody] CartRequest cartRequest)
        {
            try
            {
                var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                if(token != null)
                {
                    var accountId = Util.GetInstance().ValidateJwtToken(token);
                    if(accountId.HasValue)
                    {
                        cartRequest.AccountId = accountId.Value;
                        var result = await _cartService.isAddProductIntoCart(cartRequest);
                        return result ? Ok("Add success") : BadRequest("Request fail");
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

        [HttpDelete("/api/v1/cart")]
        [Authorize(Policy = "CustomerPolicy")]
        public async Task<IActionResult> RemoveProductFromCart([FromQuery] int productId)
        {
            try
            {
                var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                if(token != null)
                {
                    var accountId = Util.GetInstance().ValidateJwtToken(token);
                    if(accountId.HasValue)
                    {
                        var result = await _cartService.isRemoveProductFromCart(productId, accountId.Value);
                        return result ? Ok("Remove success") : BadRequest("Request fail");
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

        [HttpPut("/api/v1/cart")]
        [Authorize(Policy = "CustomerPolicy")]
        public async Task<IActionResult> UpdateCart([FromQuery] int cartId, [FromBody] CartRequest cartRequest)
        {
            try
            {
                var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                if (token != null)
                {
                    var accountId = Util.GetInstance().ValidateJwtToken(token);
                    if (accountId.HasValue)
                    {
                        cartRequest.AccountId = (int)accountId;
                        var result = await _cartService.isUpdateCart(cartId, cartRequest);
                        return result ? Ok("Update success") : BadRequest("Request fail");
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
