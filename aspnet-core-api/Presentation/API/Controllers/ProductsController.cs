using Application.Features.Commands.Product.CreateProduct;
using Application.Features.Commands.Product.RemoveProduct;
using Application.Features.Commands.Product.UpdateProduct;
using Application.Features.Commands.ProductImageFile.RemoveProductImage;
using Application.Features.Commands.ProductImageFile.UploadProductImage;
using Application.Features.Queries.Product.GetAllProducts;
using Application.Features.Queries.Product.GetByIdProduct;
using Application.Features.Queries.ProductImageFile.GetAllProductImages;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Admin")]
    public class ProductsController : ControllerBase
    {
        readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetAllProductsQueryRequest getAllProductsQueryRequest)
        {
            var response = await _mediator.Send(getAllProductsQueryRequest);
            return Ok(response);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> Get([FromRoute] GetByIdProductQueryRequest getByIdProductQueryRequest)
        {
            var response = await _mediator.Send(getByIdProductQueryRequest);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateProductCommandRequest createProductCommandRequest)
        {
            var response = await _mediator.Send(createProductCommandRequest);
            return StatusCode(201);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] UpdateProductCommandRequest updateProductCommandRequest)
        {
            await _mediator.Send(updateProductCommandRequest);
            return Ok();
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete([FromRoute] RemoveProductCommandRequest removeProductCommandRequest)
        {
            await _mediator.Send(removeProductCommandRequest);
            return Ok();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> UploadProductImages([FromQuery] string productId, [FromForm] IFormFileCollection files)
        {
            var uploadProductImageCommandRequest = new UploadProductImageCommandRequest
            {
                ProductId = productId,
                Files = files
            };

            await _mediator.Send(uploadProductImageCommandRequest);
            return Ok();
        }

        [HttpGet("[action]/{Id}")]
        public async Task<IActionResult> GetProductImages([FromRoute] GetAllProductImagesQueryRequest getAllProductImagesQueryRequest)
        {
            var response = await _mediator.Send(getAllProductImagesQueryRequest);
            return Ok(response);
        }

        [HttpDelete("[action]/{Id}")]
        public async Task<IActionResult> DeleteProductImage([FromRoute] string Id, [FromQuery] string imageId)
        {
            var removeProductImageCommandRequest = new RemoveProductImageCommandRequest
            {
                Id = Id,
                ImageId = imageId
            };
            await _mediator.Send(removeProductImageCommandRequest);
            return Ok();
        }
    }
}
