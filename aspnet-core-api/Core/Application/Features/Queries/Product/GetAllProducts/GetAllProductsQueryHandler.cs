using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Repositories.Products;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Features.Queries.Product.GetAllProducts
{
    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQueryRequest, GetAllProductsQueryResponse>
    {
        readonly IProductReadRepository _productReadRepository;
        readonly ILogger<GetAllProductsQueryHandler> _logger;
        readonly IHttpContextAccessor _httpContextAccessor;

        public GetAllProductsQueryHandler(IProductReadRepository productReadRepository, ILogger<GetAllProductsQueryHandler> logger, IHttpContextAccessor httpContextAccessor)
        {
            _productReadRepository = productReadRepository;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public Task<GetAllProductsQueryResponse> Handle(GetAllProductsQueryRequest request, CancellationToken cancellationToken)
        {
            var httpRequest = _httpContextAccessor.HttpContext.Request;
            var baseUrl = $"{httpRequest.Scheme}://{httpRequest.Host}";

            _logger.LogInformation("GetAllProductsQueryHandler.Handle");
            var totalCount = _productReadRepository.GetAll(false).Count();
            var products = _productReadRepository.GetAll(false)
                .Skip((request.Page - 1) * request.Size).Take(request.Size)
                .Include(p => p.ProductImageFiles)
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Price,
                    p.Stock,
                    p.CreatedDate,
                    p.UpdatedDate,
                    ProductImageFiles = p.ProductImageFiles.Select(pif => new
                    {
                        pif.Id,
                        pif.FileName,
                        pif.Showcase,
                        Path = $"{baseUrl}/{pif.Path}"
                    }).ToList()
                }).ToList();

            var response = new GetAllProductsQueryResponse { Products = products, TotalCount = totalCount };

            return Task.FromResult(response);
        }
    }
}
