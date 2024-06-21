using AutoMapper;
using Business.Models.CartView;
using Business.Services.Interfaces;
using DataAccess.Entities;
using FLY.DataAccess.Repositories;
using FLY.DataAccess.Repositories.Implements;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.Implements
{
    public class CartService : ICartService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CartService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<CartResponse>> GetCartsByAccountId(int accountId)
        {
            try
            {
                var account = await _unitOfWork.AccountRepository.GetByIDAsync(accountId);
                if (account != null)
                {
                    var carts = await _unitOfWork.CartRepository.GetAsync(c => c.AccountId == accountId);
                    var result = _mapper.Map<List<CartResponse>>(carts.ToList());
                    return result;
                }
                else
                {
                    throw new Exception("Please log in");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> isAddProductIntoCart(CartRequest cartRequest)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    var cart = _mapper.Map<Cart>(cartRequest);
                    await _unitOfWork.CartRepository.InsertAsync(cart);
                    await _unitOfWork.SaveAsync();
                    await transaction.CommitAsync();
                    return true;
                }
                catch
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }

        public async Task<bool> isRemoveProductFromCart(int productId, int accountId)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    var carts = await _unitOfWork.CartRepository
                        .FindAsync(c => c.ProductId == productId && c.AccountId == accountId);
                    await _unitOfWork.CartRepository.DeleteRangeAsync(carts);
                    await _unitOfWork.SaveAsync();
                    await transaction.CommitAsync();
                    return true;
                }
                catch
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }
    }
}
