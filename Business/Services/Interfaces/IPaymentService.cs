using Business.Models.PaymentView;

namespace Business.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<(PaymentResponse, string)> CreatePayment(PaymentRequest paymentRequest);
        Task<List<PaymentResponse>> GetPaymentsByAccountId(int accountId);
        Task<bool> isDeletePayment(int paymentId);
        Task<PaymentResponse> UpdatePayment(int paymentId, PaymentRequest paymentRequest);
    }
}