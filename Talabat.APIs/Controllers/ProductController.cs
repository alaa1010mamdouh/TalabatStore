using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.WindowsAzure.Storage;
using Talabat.APIs.DTOs;
using Talabat.APIs.Errors;
using Talabat.APIs.Helper;
using Talabat.Core.Entities;
using Talabat.Core.Repositores;
using Talabat.Core.Specification;

namespace Talabat.APIs.Controllers
{


    public class ProductController : ApiBaseController
    {
        private readonly IGenericRepository<Product> _productRepo;
        private readonly IMapper _mapper;
        private readonly IGenericRepository<ProductType> _typeRepo;
        private readonly IGenericRepository<ProductBrand> _brandRepo;

        public ProductController(IGenericRepository<Product> ProductRepo,IMapper mapper
            ,IGenericRepository<ProductType > TypeRepo,IGenericRepository<ProductBrand> BrandRepo)
        {
            _productRepo = ProductRepo;
            _mapper = mapper;
            _typeRepo = TypeRepo;
            _brandRepo = BrandRepo;
        }

        //BaseURL/api/Product -> Get
        [CachedAttribute(300)]
        [HttpGet]
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts([FromQuery] ProductSpecParam Params)
        {
            var Spec =new ProductWithBrandSpecification(Params);
            var products = await _productRepo.GetAllWithSpecification(Spec);
            if (products is null)
                return NotFound(new ApiResponse(400));
            var MappedProduct=  _mapper.Map<IReadOnlyList< Product>,IReadOnlyList< ProductToReturnDto>>(products);
            //var ReturnObject = new Pagination<ProductToReturnDto>()
            //{
            //   PageIndex = Params.PageIndex,
            //    PageSize = Params.PageSize,
            //    Count = products.Count,
            //    Data = MappedProduct

            //};
            var countspec= new ProductWithBrandSpecification(Params);
            var count=await _productRepo.GetCountWithSpecification(countspec);
            return Ok(new Pagination<ProductToReturnDto>(Params.PageIndex, Params.PageSize, MappedProduct,count));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductToReturnDto),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductToReturnDto>> GetProductBy(int id)
        {
            var Spec = new ProductWithBrandSpecification(id);
            var product = await _productRepo.GetByEntityWithSpecification(Spec);
            if (product is null)
                return NotFound(new ApiResponse(400));
            var MappedProduct = _mapper.Map<Product, ProductToReturnDto>(product);
            return Ok(MappedProduct);
        }

        [HttpGet("Types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetTypes()
        {
            var Types = await _typeRepo.GetAllAsync();
            return Ok(Types);
        }
        //BaseURL/api/Product/Types
        [HttpGet("Brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
        {
            var Brands = await _brandRepo.GetAllAsync();
            return Ok(Brands);
        }




    }
}
