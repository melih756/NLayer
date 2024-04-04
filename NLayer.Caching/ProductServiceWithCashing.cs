using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using NLayer.Core;
using NLayer.Core.DTO;
using NLayer.Core.Repositories;
using NLayer.Core.Services;
using NLayer.Core.UnitOfWorks;
using NLayer.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Caching
{
    public class ProductServiceWithCashing : IProductService
    {
        private const string CashProductsKeys = "productsCashe";
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCashe;
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ProductServiceWithCashing (IMapper mapper,IMemoryCache memoryCashe, IProductRepository productRepository,IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _memoryCashe = memoryCashe;
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;


            if (_memoryCashe.TryGetValue(CashProductsKeys,out _))
            {
                _memoryCashe.Set(CashProductsKeys, _productRepository.GetAll()).ToString();
            }
        }

        public async Task<Product> AddAsync(Product entity)
        {
            await _productRepository.AddAsync(entity);
            await _unitOfWork.CommitAsync();
            await CacheAllProducts();
            return entity;
        }

        public async Task<IEnumerable<Product>> AddRangeAsync(IEnumerable<Product> entities)
        {
            await _productRepository.AddRangeAsync(entities);
            await _unitOfWork.CommitAsync();
            await CacheAllProducts();
            return entities;
        }

        public Task<bool> Any(Expression<Func<Product, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Product>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Product> GetByIdAsync(int id)
        {
            return Task.FromResult(_memoryCashe.Get<List<Product>>(CashProductsKeys).FirstOrDefault(x => x.Id == id));
        }

        public async Task<CustomResponseDTO<List<ProductWithCategoryDTO>>> GetProductWithCategory()
        {
            var products =  await _productRepository.GetProductWithCategory();

            var productsWithCategoryDto = _mapper.Map<List<ProductWithCategoryDTO>>(products);

            return CustomResponseDTO<List<ProductWithCategoryDTO>>.Success(200, productsWithCategoryDto);
        }

        public async Task RemoveAsync(Product entity)
        {
            _productRepository.Remove(entity);
            await _unitOfWork.CommitAsync();
            await CacheAllProducts();
        }

        public async Task RemoveRangeAsync(IEnumerable<Product> entities)
        {
            _productRepository.RemoveRange(entities);
            await _unitOfWork.CommitAsync();
            await CacheAllProducts();
        }

        public async Task UpdateAsync(Product entity)
        {
            _productRepository.Update(entity);
            await _unitOfWork.CommitAsync();
            await CacheAllProducts();
            
        }

        public IQueryable<Product> Where(Expression<Func<Product, bool>> expression)
        {
            return _memoryCashe.Get<List<Product>>(CashProductsKeys).Where(expression.Compile()).AsQueryable();
        }

        public async Task CacheAllProducts()
        {
            _memoryCashe.Set(CashProductsKeys, await _productRepository.GetAll().ToListAsync());
        }
    }
}
