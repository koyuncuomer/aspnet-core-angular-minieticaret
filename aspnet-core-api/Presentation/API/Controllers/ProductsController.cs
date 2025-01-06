using Application.Repositories.Products;
using Application.RequestParameters;
using Application.ViewModels.Products;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        readonly private IProductReadRepository _productReadRepository;
        readonly private IProductWriteRepository _productWriteRepository;
        readonly private IWebHostEnvironment _webHostEnviroment;

        public ProductsController(IProductReadRepository productReadRepository, IProductWriteRepository productWriteRepository, IWebHostEnvironment webHostEnviroment)
        {
            _productReadRepository = productReadRepository;
            _productWriteRepository = productWriteRepository;
            _webHostEnviroment = webHostEnviroment;
        }

        [HttpGet]
        public IActionResult Get([FromQuery] Pagination pagination)
        {
            var totalCount = _productReadRepository.GetAll(false).Count();
            var products = _productReadRepository.GetAll(false).Select(p => new
            {
                p.Id,
                p.Name,
                p.Price,
                p.Stock,
                p.CreatedDate,
                p.UpdatedDate
            }).Skip((pagination.Page - 1) * pagination.Size).Take(pagination.Size);

            return Ok(new { totalCount, products });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute(Name = "id")] string id)
        {
            var product = await _productReadRepository.GetByIdAsync(id, false);
            if (product == null)
                return NotFound();
            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] VM_Create_Product vm)
        {
            await _productWriteRepository.AddAsync(new()
            {
                Name = vm.Name,
                Price = vm.Price,
                Stock = vm.Stock
            });
            await _productWriteRepository.SaveAsync();
            return StatusCode(201);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] VM_Update_Product vm)
        {
            var product = await _productReadRepository.GetByIdAsync(vm.Id);
            if (product == null)
                return NotFound();
            product.Name = vm.Name;
            product.Price = vm.Price;
            product.Stock = vm.Stock;
            await _productWriteRepository.SaveAsync();
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute(Name = "id")] string id)
        {
            var product = await _productReadRepository.GetByIdAsync(id);
            if (product == null)
                return NotFound();
            await _productWriteRepository.RemoveAsync(id);
            await _productWriteRepository.SaveAsync();
            return Ok();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Upload()
        {
            string uploadPath = Path.Combine(_webHostEnviroment.WebRootPath, "resource/product-images");

            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            Random r = new();
            foreach (IFormFile file in Request.Form.Files)
            {
                string fullPath = Path.Combine(uploadPath, $"{r.Next()}{Path.GetExtension(file.FileName)}");

                using FileStream fileStream = new(fullPath, FileMode.Create, FileAccess.Write, FileShare.None, 1024 * 1024, useAsync: false);
                await file.CopyToAsync(fileStream);
                await fileStream.FlushAsync();
            }

            return Ok();
        }
    }
}
