using AutoMapper;
using Business.Models.PaymentView;
using Business.Services.Interfaces;
using Business.Vnpay;
using DataAccess.Entities;
using FLY.DataAccess.Repositories;
using System.Globalization;

namespace Business.Services.Implements
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PaymentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<PaymentResponse>> GetPaymentsByOrderId(int orderId)
        {
            var payments = await _unitOfWork.PaymentRepository.FindAsync(p => p.OrderId == orderId);
            return _mapper.Map<List<PaymentResponse>>(payments.ToList());
        }

        public async Task<PaymentResponse> CreatePayment(PaymentRequest paymentRequest)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    var existedOrder = await _unitOfWork.OrderRepository.GetByIDAsync(int.Parse(paymentRequest.vnp_TxnRef));
                    if(existedOrder != null)
                    {
                        var payment = new Payment()
                        {
                            PaymentMethod = "VPN",
                            BankCode = paymentRequest.vnp_BankCode,
                            BankTranNo = paymentRequest.vnp_BankTranNo,
                            CardType = paymentRequest.vnp_CardType,
                            PaymentInfo = paymentRequest.vnp_OrderInfo,
                            PayDate = DateTime.ParseExact(paymentRequest.vnp_PayDate, "yyyyMMddHHmmss", CultureInfo.InvariantCulture),
                            TransactionNo = paymentRequest.vnp_TransactionNo,
                            TransactionStatus = int.Parse(paymentRequest.vnp_TransactionStatus),
                            PaymentAmount = double.Parse(paymentRequest.vnp_Amount),
                            OrderId = int.Parse(paymentRequest.vnp_TxnRef)
                        };
                        await _unitOfWork.PaymentRepository.InsertAsync(payment);
                        //Update Order's status is Paid
                        existedOrder.Status = 2;
                        await _unitOfWork.OrderRepository.UpdateAsync(existedOrder);
                        await _unitOfWork.SaveAsync();
                        await transaction.CommitAsync();
                        return _mapper.Map<PaymentResponse>(payment);
                    } else
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
                        .FindAsync(p => p.PaymentId == paymentId)).FirstOrDefault();
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
        
    }
}
