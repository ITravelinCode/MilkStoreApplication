using Business.Models.OrderView;

namespace Business.Services.Interfaces
{
    public interface IOrderService
    {
        Task<string> CreateOrder(int accountId);
        Task<List<OrderResponse>> GetOrderByAccount(int accountId);
        Task<bool> isDeleteOrder(int orderId);
        Task<bool> UpdateStatusOrder(int orderId, int accountId, int status);
    }
}