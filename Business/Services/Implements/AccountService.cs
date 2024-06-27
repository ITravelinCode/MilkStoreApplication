using AutoMapper;
using Business.Models.Auth;
using Business.Services.Interfaces;
using FLY.DataAccess.Repositories;
using FLY.DataAccess.Repositories.Implements;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Repositories.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.Implements
{
    public class AccountService : IAccountService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public AccountService(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        public async Task<string> Login(string email, string password)
        {
            try
            {
                var hashedPassword = await HashedPassword(password);
                var accounts = await _unitOfWork.AccountRepository.FindAsync(a => a.Email == email && a.Password == hashedPassword);
                if (accounts.Any())
                {
                    var account = accounts.FirstOrDefault();
                    if (account != null && account.Status == 1)
                    {
                        return GenerateJwtToken(account);
                    }
                    else
                    {
                        throw new Exception("Account is blocked");
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<string> SignUp(RegisterRequest registerRequest)
        {
            var accounts = await _unitOfWork.AccountRepository.FindAsync(a => a.Email == registerRequest.Email);
            if (accounts.Any())
            {
                throw new Exception("Account already exist");
            }
            else
            {
                using (var transaction = _unitOfWork.BeginTransaction())
                {
                    try
                    {
                        var account = _mapper.Map<Account>(registerRequest);
                        account.Status = 1;
                        account.RoleId = 2;
                        account.Password = await HashedPassword(registerRequest.Password);
                        await _unitOfWork.AccountRepository.InsertAsync(account);
                        await _unitOfWork.SaveAsync();
                        await transaction.CommitAsync();
                        return GenerateJwtToken(account);
                    }
                    catch
                    {
                        transaction.Rollback();
                        return null;
                    }
                }
            }
        }
        public async Task<bool> UpdateAccount(UpdateAccountRequest request)
        {
            try
            {
                var accounts = await _unitOfWork.AccountRepository.FindAsync(a => a.Email == request.Email);
                if (accounts.Any())
                {
                    using (var transaction = _unitOfWork.BeginTransaction())
                    {
                        try
                        {
                            var account = accounts.FirstOrDefault();
                            if (account.Status == 0) throw new Exception("Account is blocked");
                            _mapper.Map(request, account);
                            await _unitOfWork.AccountRepository.UpdateAsync(account);
                            await _unitOfWork.SaveAsync();
                            return true;
                        }
                        catch
                        {
                            transaction.Rollback();
                            return false;
                        }
                    }
                }
                else
                {
                    throw new Exception("Account not found");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private async Task<string> HashedPassword(string password)
        {
            try
            {
                using (SHA512 sha512 = SHA512.Create())
                {
                    byte[] hashBytes = sha512.ComputeHash(Encoding.UTF8.GetBytes(password));

                    StringBuilder stringBuilder = new StringBuilder();
                    for (int i = 0; i < hashBytes.Length; i++)
                    {
                        stringBuilder.Append(hashBytes[i].ToString("x2"));
                    }
                    return await Task.FromResult<string?>(stringBuilder.ToString());
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private string GenerateJwtToken(Account account)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new Claim[]
                {
                                new Claim(ClaimTypes.Email, account.Email),
                                new Claim(ClaimTypes.Role, account.RoleId.ToString()),
                                new Claim("accountId", account.AccountId.ToString())
                }),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
