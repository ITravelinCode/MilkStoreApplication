using Business.Models.PaymentView;

namespace Business.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<PaymentResponse> CreatePayment(PaymentRequest paymentRequest);
        Task<List<PaymentResponse>> GetPaymentsByOrderId(int orderId);
        Task<bool> isDeletePayment(int paymentId);
        Task<PaymentResponse> UpdatePayment(int paymentId, PaymentRequest paymentRequest);
    }
}