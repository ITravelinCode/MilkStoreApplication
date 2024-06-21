﻿using AutoMapper;
using Business.Models.Auth;
using Business.Models.CartView;
using Business.Models.ProductView;
using DataAccess.Entities;
using Repositories.Entities;

namespace Business.Models
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<RegisterRequest, Account>();
            CreateMap<UpdateAccountRequest, Account>();
            CreateMap<Product, ProductResponse>();
            CreateMap<Cart, CartResponse>();
            CreateMap<CartRequest, Cart>();
        }
    }
}