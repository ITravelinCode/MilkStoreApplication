using Business.Models.CartView;

namespace Business.Services.Interfaces
{
    public interface ICartService
    {
        Task<List<CartResponse>> GetCartsByAccountId(int accountId);
        Task<bool> isAddProductIntoCart(CartRequest cartRequest);
        Task<bool> isRemoveProductFromCart(int productId, int accountId);
    }
}