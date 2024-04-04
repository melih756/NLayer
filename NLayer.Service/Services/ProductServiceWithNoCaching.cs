using AutoMapper;
using NLayer.Core;
using NLayer.Core.DTO;
using NLayer.Core.Repositories;
using NLayer.Core.Services;
using NLayer.Core.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Service.Services
{
    public class ProductServiceWithNoCaching:Service<Product>,IProductService
    {
        private readonly IProductRepository _productrepository;
        private readonly IMapper _mapper;

        public ProductServiceWithNoCaching(IGenericRepository<Product> repository,IUnitOfWork unitOfWork,IMapper mapper,IProductRepository productRepository)
        {
            _mapper = mapper;
            _productrepository = productRepository;
        }

        public async Task<CustomResponseDTO<List<ProductWithCategoryDTO>> GetProductWithCategory()
        {
            var products = await _productrepository.GetProductWithCategory();

            var productsDTO = _mapper.Map<List<ProductWithCategoryDTO>>(products);
            return CustomResponseDTO<List<ProductWithCategoryDTO>>.Success(200,productsDTO);
        } 
    }
