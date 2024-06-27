using Business.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MilkStoreApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        //[HttpGet("/api/v1/products")]
        //public async Task<IActionResult> GetAllProducts()
        //{
        //    try
        //    {
        //        var result = await _productService.GetProductsAsync();
        //        if (result.Any()) return Ok(result);
        //        else return NotFound("Products data is not exist");
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, ex.Message);
        //    }
        //}

        [HttpGet("/api/v1/products")]
        public async Task<IActionResult> GetProductsWithPagination([FromQuery] int? pageIndex = null, [FromQuery] int? pageSize = null)
        {
            try 
            { 
                var result = await _productService.GetPaginationProductsAsync(pageIndex, pageSize);
                if (result.Any()) return Ok(result);
                else return NotFound("Products data is not exist");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("/api/v1/product/{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            try
            {
                var result = await _productService.GetProductById(id);
                if (result != null) return Ok(result);
                else return NotFound("Products data is not exist");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
