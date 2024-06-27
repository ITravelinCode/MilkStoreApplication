using Business.Models.OrderView;

namespace Business.Services.Interfaces
{
    public interface IOrderService
    {
        Task<OrderResponse> CreateOrder(int accountId, OrderRequest orderRequest);
        Task<List<OrderResponse>> GetOrderByAccount(int accountId);
        Task<bool> isDeleteOrder(int orderId);
    }
}