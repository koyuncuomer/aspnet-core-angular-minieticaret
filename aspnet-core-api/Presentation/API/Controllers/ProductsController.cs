using Application.Repositories.Products;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        readonly private IProductReadRepository _productReadRepository;
        readonly private IProductWriteRepository _productWriteRepository;

        public ProductsController(IProductReadRepository productReadRepository, IProductWriteRepository productWriteRepository)
        {
            _productReadRepository = productReadRepository;
            _productWriteRepository = productWriteRepository;
        }

        //[HttpGet]
        //public async Task Get()
        //{
        //    //await _productWriteRepository.AddRangeAsync(new()
        //    //{
        //    //    new() { Name = "Product 11", Price = 100, Stock = 10 },
        //    //    new() { Name = "Product 22", Price = 200, Stock = 20 }
        //    //});
        //    //await _productWriteRepository.SaveAsync();

        //    var p = await _productReadRepository.GetByIdAsync("c188131a-772c-4204-ac5d-c9c1575c3a26", false);
        //    p.Name = "Product 22 Updated";
        //    await _productWriteRepository.SaveAsync();
        //}

        //[HttpGet("{id}")]
        //public async Task<IActionResult> Get(string id)
        //{
        //    var product = await _productReadRepository.GetByIdAsync(id);
        //    if (product == null)
        //        return NotFound();
        //    return Ok(product);
        //}

        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Merhaba");
        }
    }
}
