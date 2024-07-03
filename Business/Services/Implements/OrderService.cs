using AutoMapper;
using Business.Models.OrderView;
using Business.Services.Interfaces;
using Business.Vnpay;
using DataAccess.Entities;
using FLY.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
        private readonly IConfiguration _configuration;

        public OrderService(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configuration = configuration;
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

        public async Task<string> CreateOrder(int accountId)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    var existedAccount = await _unitOfWork.AccountRepository.GetByIDAsync(accountId);
                    var cartList = await _unitOfWork.CartRepository.FindAsync(c => c.AccountId == accountId);
                    if(existedAccount != null && cartList.Any())
                    {
                        var order = new Order()
                        {
                            AccountId = accountId,
                            Status = 0,
                            OrderDate = DateTime.Now,
                            ExpireDate = DateTime.Now.AddMinutes(30),
                            TotalPrice = cartList.Sum(c => c.Quantity * c.UnitPrice),
                            orderDetails = new List<OrderDetail>()
                        };
                        
                        await _unitOfWork.OrderRepository.InsertAsync(order);
                        await _unitOfWork.SaveAsync();
                        foreach (var item in cartList)
                        {
                            var orderDetail = new OrderDetail()
                            {
                                OrderId = order.OrderId,
                                ProductId = item.ProductId,
                                OrderQuantity = item.Quantity,
                                ProductPrice = item.UnitPrice
                            };
                            order.orderDetails.Add(orderDetail);
                        }
                        await _unitOfWork.OrderDetailRepository.AddRangeAsync(order.orderDetails);
                        await _unitOfWork.CartRepository.DeleteRangeAsync(cartList);
                        await _unitOfWork.SaveAsync();
                        await transaction.CommitAsync();
                        var paymentUrl = CreateVnpayLink(order);
                        return paymentUrl;
                    }
                    throw new ArgumentNullException($"Account is not correct");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception(ex.Message);
                }
            }
        }

        public string CreateVnpayLink(Order order)
        {
            var paymentUrl = string.Empty;

            var vpnRequest = new VnpayRequest(_configuration["VNpay:Version"], _configuration["VNpay:tmnCode"],
                order.OrderDate, "127.0.0.1", (decimal)order.TotalPrice, "VND", "other",
                $"Thanh toan don hang {order.OrderId}", _configuration["VNpay:ReturnUrl"], 
                $"{order.OrderId}", order.ExpireDate);

            paymentUrl = vpnRequest.GetLink(_configuration["VNpay:PaymentUrl"], 
                _configuration["VNpay:HashSecret"]);

            return paymentUrl;
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

        public async Task<OrderResponse> UpdateOrder(int orderId, int accountId, OrderRequest orderRequest)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    var existedAccount = await _unitOfWork.AccountRepository.GetByIDAsync(accountId);
                    var existedOrder = (await _unitOfWork.OrderRepository
                        .FindAsync(o => o.AccountId == accountId && o.OrderId == orderId)).FirstOrDefault();
                    if (existedOrder != null)
                    {
                        _mapper.Map(orderRequest, existedOrder);
                        foreach (var existedDetail in existedOrder.orderDetails.ToList())
                        {
                            var updatedDetail = orderRequest.OrderDetails?.FirstOrDefault(d => d.ProductId == existedDetail.ProductId);
                            if (updatedDetail != null)
                            {
                                _mapper.Map(updatedDetail, existedDetail);
                                await _unitOfWork.OrderDetailRepository.UpdateAsync(existedDetail);
                            }
                            else
                            {
                                await _unitOfWork.OrderDetailRepository.DeleteAsync(existedDetail);
                            }
                        }
                        existedOrder.TotalPrice = existedOrder.orderDetails.Sum(od => od.OrderQuantity * od.ProductPrice);
                        await _unitOfWork.OrderRepository.UpdateAsync(existedOrder);
                        await _unitOfWork.SaveAsync();
                        await transaction.CommitAsync();
                        return _mapper.Map<OrderResponse>(existedOrder);
                    }
                    throw new ArgumentNullException($"Not found");
                }
                catch(Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception(ex.Message);
                }
            }
        }

        public async Task<bool> UpdateStatusOrder(int orderId, int accountId, int status)
        {
            using(var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    var existedOrder = (await _unitOfWork.OrderRepository
                        .FindAsync(o => o.AccountId == accountId && o.OrderId == orderId))
                        .FirstOrDefault();
                    if (existedOrder != null)
                    {
                        existedOrder.Status = status;
                        await _unitOfWork.OrderRepository.UpdateAsync(existedOrder);
                        await _unitOfWork.SaveAsync();
                        await transaction.CommitAsync();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch(Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception(ex.Message);
                }
            }
        }
    }
}
