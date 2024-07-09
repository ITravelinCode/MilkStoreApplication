using Business;
using Business.Models.OrderView;
using Business.Models.PaymentView;
using Business.Services.Implements;
using Business.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System.IdentityModel.Tokens.Jwt;

namespace MilkStoreApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpGet("/api/v1/payment/vpn-return")]
        public async Task<IActionResult> CreatePayment([FromQuery] PaymentRequest parameters)
        {
            try
            {
                if(parameters.vnp_BankTranNo == null)
                {
                    return Ok("Cancel Transaction");
                }
                var result = await _paymentService.CreatePayment(parameters);
                return result != null ? Ok(result) : NotFound("Order does not created");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        //[HttpPut("/api/v1/payment")]
        //[Authorize(Policy = "AdminPolicy")]
        //public async Task<IActionResult> UpdatePayment([FromQuery] int paymentId, [FromBody] PaymentRequest paymentRequest)
        //{
        //    try
        //    {
        //        var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        //        if (token != null)
        //        {
        //            var accountId = Util.GetInstance().ValidateJwtToken(token);
        //            if (accountId.HasValue)
        //            {
        //                paymentRequest.AccountId = accountId.Value;
        //                var result = await _paymentService.UpdatePayment(paymentId, paymentRequest);
        //                return result != null ? Ok(result) : BadRequest("Update fail");
        //            }
        //            return Unauthorized("Not found account's information in token");
        //        }
        //        else
        //        {
        //            return Unauthorized("Please login account");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Inner Error: {ex}");
        //    }
        //}

        //[HttpDelete("/api/v1/payment")]
        //[Authorize(Policy = "AdminPolicy")]
        //public async Task<IActionResult> DeletePayment([FromQuery] int paymentId)
        //{
        //    try
        //    {
        //        var result = await _paymentService.isDeletePayment(paymentId);
        //        return result ? Ok("Delete success") : BadRequest("Delete fail");
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Inner Error: {ex}");
        //    }
        //}
    }
}
