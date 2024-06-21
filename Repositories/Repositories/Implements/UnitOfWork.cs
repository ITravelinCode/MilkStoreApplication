using DataAccess.Entities;
using Microsoft.EntityFrameworkCore.Storage;
using Repositories.Entities;

namespace FLY.DataAccess.Repositories.Implements
{
    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork(MilkStoreContext context)
        {
            this.context = context;
        }

        private readonly MilkStoreContext context;
        private GenericRepository<Account> _accountRepository;
        private GenericRepository<Cart> _cartRepository;
        private GenericRepository<Order> _orderRepository;
        private GenericRepository<OrderDetail> _orderDetailRepository;
        private GenericRepository<Product> _productRepository;
        private GenericRepository<ProductCategory> _productCategoryRepository;
        private GenericRepository<Payment> _paymentRepository;

        public IGenericRepository<Account> AccountRepository => _accountRepository ??= new GenericRepository<Account>(context);
        public IGenericRepository<Cart> CartRepository => _cartRepository ??= new GenericRepository<Cart>(context);
        public IGenericRepository<Order> OrderRepository => _orderRepository ??= new GenericRepository<Order>(context);
        public IGenericRepository<OrderDetail> OrderDetailRepository => _orderDetailRepository ??= new GenericRepository<OrderDetail>(context);
        public IGenericRepository<Payment> PaymentRepository => _paymentRepository ??= new GenericRepository<Payment>(context);
        public IGenericRepository<Product> ProductRepository => _productRepository ??= new GenericRepository<Product>(context);
        public IGenericRepository<ProductCategory> ProductCategoryRepository => _productCategoryRepository ??= new GenericRepository<ProductCategory>(context);

        public IDbContextTransaction BeginTransaction()
        {
            return context.Database.BeginTransaction();
        }

        public async Task SaveAsync()
        {
            await context.SaveChangesAsync();
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
