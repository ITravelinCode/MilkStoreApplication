using Business.Models.Auth;

namespace Business.Services.Interfaces
{
    public interface IAccountService
    {
        Task<string> Login(string email, string password);
        Task<string> SignUp(RegisterRequest registerRequest);
        Task<bool> UpdateAccount(UpdateAccountRequest request);
    }
}