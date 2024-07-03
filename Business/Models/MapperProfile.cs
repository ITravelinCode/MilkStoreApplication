using AutoMapper;
using Business.Models.Auth;
using Business.Models.CartView;
using Business.Models.OrderView;
using Business.Models.PaymentView;
using Business.Models.ProductView;
using DataAccess.Entities;
using Repositories.Entities;

namespace Business.Models
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            //Auth
            CreateMap<RegisterRequest, Account>();
            CreateMap<UpdateAccountRequest, Account>();
            //Product
            CreateMap<Product, ProductResponse>();
            //Cart
            CreateMap<Cart, CartResponse>();
            CreateMap<CartRequest, Cart>();
            //Order
            CreateMap<OrderRequest, Order>();
            CreateMap<Order, OrderResponse>();
            CreateMap<OrderDetailRequest, OrderDetail>();
            CreateMap<OrderDetail, OrderDetailResponse>();
            //Payment
            CreateMap<PaymentRequest, Payment>();
            CreateMap<Payment, PaymentResponse>();
        }
    }
}
