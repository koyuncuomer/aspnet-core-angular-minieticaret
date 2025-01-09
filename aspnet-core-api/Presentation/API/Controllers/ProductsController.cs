using Application.Abstractions.Storage;
using Application.Repositories;
using Application.Repositories.Files;
using Application.Repositories.InvoiceFiles;
using Application.Repositories.ProductImageFiles;
using Application.Repositories.Products;
using Application.RequestParameters;
using Application.ViewModels.Products;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        readonly private IProductReadRepository _productReadRepository;
        readonly private IProductWriteRepository _productWriteRepository;
        readonly private IWebHostEnvironment _webHostEnviroment;
        readonly IFileReadRepository _fileReadRepository;
        readonly IFileWriteRepository _fileWriteRepository;
        readonly IProductImageFileReadRepository _productImageFileReadRepository;
        readonly IProductImageFileWriteRepository _productImageFileWriteRepository;
        readonly IInvoiceFileReadRepository _invoiceFileReadRepository;
        readonly IInvoiceFileWriteRepository _invoiceFileWriteRepository;
        readonly IStorageService _storageService;

        public ProductsController(
            IProductReadRepository productReadRepository,
            IProductWriteRepository productWriteRepository,
            IWebHostEnvironment webHostEnviroment,
            IFileReadRepository fileReadRepository,
            IFileWriteRepository fileWriteRepository,
            IProductImageFileReadRepository productImageFileReadRepository,
            IProductImageFileWriteRepository productImageFileWriteRepository,
            IInvoiceFileReadRepository invoiceFileReadRepository,
            IInvoiceFileWriteRepository invoiceFileWriteRepository,
            IStorageService storageService)
        {
            _productReadRepository = productReadRepository;
            _productWriteRepository = productWriteRepository;
            _webHostEnviroment = webHostEnviroment;
            _fileReadRepository = fileReadRepository;
            _fileWriteRepository = fileWriteRepository;
            _productImageFileReadRepository = productImageFileReadRepository;
            _productImageFileWriteRepository = productImageFileWriteRepository;
            _invoiceFileReadRepository = invoiceFileReadRepository;
            _invoiceFileWriteRepository = invoiceFileWriteRepository;
            _storageService = storageService;
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
        public async Task<IActionResult> Upload([FromQuery] string id, [FromForm] IFormFileCollection files)
        {
            var datas = await _storageService.UploadAsync("resource/product-images", files);

            var product = await _productReadRepository.GetByIdAsync(id);

            await _productImageFileWriteRepository.AddRangeAsync(datas.Select(d => new ProductImageFile()
            {
                FileName = d.fileName,
                Path = d.pathOrContainerName,
                Storage = _storageService.StorageName,
                Products = new List<Product>() { product }
            }).ToList());
            await _productImageFileWriteRepository.SaveAsync();


            return Ok();
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetProductImage([FromRoute] string id)
        {
            var product = await _productReadRepository.Table.Include(p => p.ProductImageFiles).FirstOrDefaultAsync(p => p.Id == Guid.Parse(id));

            var baseUrl = $"{Request.Scheme}://{Request.Host}";

            return Ok(product.ProductImageFiles.Select(p => new
            {
                Path = $"{baseUrl}/{p.Path}",
                p.FileName,
                p.Id
            }));
        }

        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> DeleteProductImage([FromRoute] string id, [FromQuery] string imageId)
        {
            var product = await _productReadRepository.Table.Include(p => p.ProductImageFiles).FirstOrDefaultAsync(p => p.Id == Guid.Parse(id));

            var productImageFile = product.ProductImageFiles.FirstOrDefault(p => p.Id == Guid.Parse(imageId));
            product.ProductImageFiles.Remove(productImageFile);
            await _productWriteRepository.SaveAsync();
            return Ok();
        }
    }
}
