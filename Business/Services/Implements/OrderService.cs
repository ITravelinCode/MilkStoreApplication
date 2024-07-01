using AutoMapper;
using Business.Models.OrderView;
using Business.Services.Interfaces;
using DataAccess.Entities;
using FLY.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.Implements
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<OrderResponse>> GetOrderByAccount(int accountId)
        {
            try
            {
                var orders = await _unitOfWork.OrderRepository.GetAsync(o => o.AccountId == accountId);
                return _mapper.Map<List<OrderResponse>>(orders);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<OrderResponse> CreateOrder(int accountId, OrderRequest orderRequest)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    var exitedAccount = await _unitOfWork.AccountRepository.GetByIDAsync(accountId);
                    var exitedPayment = await  _unitOfWork.PaymentRepository.GetByIDAsync(orderRequest.PaymentId);
                    if(exitedAccount != null && exitedAccount !=null)
                    {
                        var order = _mapper.Map<Order>(orderRequest);
                        order.AccountId = accountId;
                        order.Status = 1;
                        order.TotalPrice = order.orderDetails.Sum(od => od.OrderQuantity * od.ProductPrice);
                        await _unitOfWork.OrderRepository.InsertAsync(order);
                        await _unitOfWork.SaveAsync();
                        await transaction.CommitAsync();
                        foreach (var orderDetail in order.orderDetails)
                        {
                            orderDetail.OrderId = order.OrderId;
                            orderDetail.Status = order.Status;
                        }
                        await _unitOfWork.OrderDetailRepository.AddRangeAsync(order.orderDetails);
                        await _unitOfWork.SaveAsync();
                        await transaction.CommitAsync();
                        var orderResponse = _mapper.Map<OrderResponse>(order);
                        return orderResponse;
                    }
                    throw new ArgumentNullException($"1 or both parameters(Account/Payment) not found");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception(ex.Message);
                }
            }
        }

        public async Task<bool> isDeleteOrder(int orderId)
        {
            try
            {
                var order = await _unitOfWork.OrderRepository.GetByIDAsync(orderId);
                if (order == null) throw new Exception("Order not found");
                var orderDetails = await _unitOfWork.OrderDetailRepository.FindAsync(o => o.OrderId == orderId);
                await _unitOfWork.OrderRepository.DeleteAsync(order);
                await _unitOfWork.OrderDetailRepository.DeleteRangeAsync(orderDetails);
                await _unitOfWork.SaveAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
