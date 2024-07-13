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
                string appScheme = "milkstore";

                if (parameters.vnp_BankTranNo == null)
                {
                    string redirectUrl = $"{appScheme}://android-app?orderId={parameters.vnp_TxnRef}";

                    return Redirect(redirectUrl);
                }
                var result = await _paymentService.CreatePayment(parameters);

                if (result != null)
                {
                    string redirectUrl = $"{appScheme}://android-app?status={result.TransactionStatus}&orderId={result.OrderId}";

                    return Redirect(redirectUrl);
                }
                else
                {
                    return NotFound("Order does not created");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        //[HttpGet("/api/v1/payment/vpn-return")]
        //public async Task<IActionResult> CreatePayment([FromQuery] PaymentRequest parameters)
        //{
        //    try
        //    {
        //        if(parameters.vnp_BankTranNo == null)
        //        {
        //            return Ok("Cancel Transaction");
        //        }
        //        var result = await _paymentService.CreatePayment(parameters);
        //        return result != null ? Ok(result) : NotFound("Order does not created");
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, ex.Message);
        //    }
        //}
    }
}
