using AutoMapper;
using Business.Models.ProductView;
using Business.Services.Interfaces;
using FLY.DataAccess.Repositories;
using FLY.DataAccess.Repositories.Implements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.Implements
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<ProductResponse>> GetProductsAsync()
        {
            try
            {
                var products = await _unitOfWork.ProductRepository.GetAsync();
                var result = _mapper.Map<List<ProductResponse>>(products.ToList());
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<ProductResponse>> GetPaginationProductsAsync(int? pageIndex, int? pageSize)
        {
            try
            {
                var products = await _unitOfWork.ProductRepository
                    .GetAsync(pageIndex: pageIndex, pageSize: pageSize);
                var result = _mapper.Map<List<ProductResponse>>(products.ToList());
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ProductResponse> GetProductById(int productId)
        {
            try
            {
                var product = await _unitOfWork.ProductRepository.GetByIDAsync(productId);
                return _mapper.Map<ProductResponse>(product);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<ProductResponse>> SearchProductsByName(string productName)
        {
            try
            {
                var products = await _unitOfWork.ProductRepository.FindAsync(p => p.ProductName.Contains(productName));
                return _mapper.Map<List<ProductResponse>>(products);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
