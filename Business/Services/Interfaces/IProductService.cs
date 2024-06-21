using Business.Models.ProductView;

namespace Business.Services.Interfaces
{
    public interface IProductService
    {
        Task<List<ProductResponse>> GetPaginationProductsAsync(int pageIndex, int pageSize);
        Task<ProductResponse> GetProductById(int productId);
        Task<List<ProductResponse>> GetProductsAsync();
    }
}