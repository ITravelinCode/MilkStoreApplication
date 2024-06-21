using DataAccess.Entities;
using Microsoft.EntityFrameworkCore.Storage;
using Repositories.Entities;

namespace FLY.DataAccess.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IDbContextTransaction BeginTransaction();
        Task SaveAsync();
        IGenericRepository<Account> AccountRepository { get; }
        IGenericRepository<Cart> CartRepository { get; }
        IGenericRepository<Order> OrderRepository { get; }
        IGenericRepository<OrderDetail> OrderDetailRepository { get; }
        IGenericRepository<Payment> PaymentRepository { get; }
        IGenericRepository<Product> ProductRepository { get; }
        IGenericRepository<ProductCategory> ProductCategoryRepository { get; }

    }
}
