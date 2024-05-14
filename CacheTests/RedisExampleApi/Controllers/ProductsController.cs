using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RedisExample.Cache;
using RedisExampleApi.Models;
using RedisExampleApi.Repository;
using RedisExampleApi.Services;
using StackExchange.Redis;

namespace RedisExampleApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        //private readonly IProductRepository _productRepository;
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            //_productRepository = productRepository;
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _productService.GetAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _productService.GetByIdAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> Create (Product product)
        {
            return Created(string.Empty,await _productService.CreateAsync(product));
        }
    }
}
