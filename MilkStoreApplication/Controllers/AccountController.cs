using Business.Models.Auth;
using Business.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MilkStoreApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("/api/v1/login")]
        public async Task<IActionResult> Login([FromBody] AuthRequest authRequest)
        {
            try
            {
                var result = await _accountService.Login(authRequest.Email, authRequest.Password);
                if(string.IsNullOrEmpty(result)) return NotFound("Not found account");
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("/api/v1/register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
        {
            try
            {
                var result = await _accountService.SignUp(registerRequest);
                if (string.IsNullOrEmpty(result)) return BadRequest("Something wrong check your request");
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("/api/v1/update-account")]
        public async Task<IActionResult> UpdateAccount([FromBody] UpdateAccountRequest updateAccountRequest)
        {
            try
            {
                var result = await _accountService.UpdateAccount(updateAccountRequest);
                if (result) return Ok("Update success");
                else return BadRequest("Update fail");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
