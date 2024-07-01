using AutoMapper;
using Business.Models.PaymentView;
using Business.Services.Interfaces;
using Business.Vnpay;
using DataAccess.Entities;
using FLY.DataAccess.Repositories;
using FLY.DataAccess.Repositories.Implements;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.Implements
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PaymentService(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<List<PaymentResponse>> GetPaymentsByAccountId(int accountId)
        {
            var payments = await _unitOfWork.PaymentRepository.FindAsync(p => p.AccountId == accountId);
            return _mapper.Map<List<PaymentResponse>>(payments.ToList());
        }

        public async Task<(PaymentResponse, string)> CreatePayment(PaymentRequest paymentRequest)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    var exitedAccount = await _unitOfWork.AccountRepository.GetByIDAsync(paymentRequest.AccountId);
                    if (exitedAccount != null)
                    {
                        var payment = _mapper.Map<Payment>(paymentRequest);
                        payment.PaymentDate = DateTime.Now;
                        payment.ExpireDate = DateTime.Now.AddMinutes(30);
                        await _unitOfWork.PaymentRepository.InsertAsync(payment);
                        await _unitOfWork.SaveAsync();
                        await transaction.CommitAsync();
                        var paymentUrl = CreateVnpayLink(payment);
                        return (_mapper.Map<PaymentResponse>(payment), paymentUrl);
                    }
                    else
                    {
                        return (null,null);
                    }
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception(ex.Message);
                }
            }
        }
        public async Task<bool> isDeletePayment(int paymentId)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    var payment = await _unitOfWork.PaymentRepository.GetByIDAsync(paymentId);
                    if (payment != null)
                    {
                        await _unitOfWork.PaymentRepository.DeleteAsync(payment);
                        await _unitOfWork.SaveAsync();
                        await transaction.CommitAsync();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception(ex.Message);
                }
            }
        }
        public async Task<PaymentResponse> UpdatePayment(int paymentId, PaymentRequest paymentRequest)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    var payment = (await _unitOfWork.PaymentRepository
                        .FindAsync(p => p.PaymentId == paymentId && p.AccountId == paymentRequest.AccountId)).FirstOrDefault();
                    if (payment != null)
                    {
                        _mapper.Map(paymentRequest, payment);
                        await _unitOfWork.PaymentRepository.UpdateAsync(payment);
                        await _unitOfWork.SaveAsync();
                        await transaction.CommitAsync();
                        return _mapper.Map<PaymentResponse>(payment);
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception(ex.Message);
                }
            }
        }
        private string CreateVnpayLink(Payment payment)
        {
            var paymentUrl = string.Empty;
            var vpnRequest = new VnpayRequest(_configuration["VNpay:Version"], _configuration["VNpay:tmnCode"],
                payment.PaymentDate, "127.0.0.1", (decimal)payment.PaymentAmount, "VND", "other", 
                "Thanh toan don hang", _configuration["VNpay:ReturnUrl"], $"{payment.PaymentId}", payment.ExpireDate);
            paymentUrl = vpnRequest.GetLink(_configuration["VNpay:PaymentUrl"], _configuration["VNpay:HashSecret"]);
            return paymentUrl;
        }
    }
}
