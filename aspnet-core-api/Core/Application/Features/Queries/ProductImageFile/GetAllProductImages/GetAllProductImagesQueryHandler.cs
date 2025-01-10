using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Repositories.Products;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Queries.ProductImageFile.GetAllProductImages
{
    public class GetAllProductImagesQueryHandler : IRequestHandler<GetAllProductImagesQueryRequest, List<GetAllProductImagesQueryResponse>>
    {
        readonly IProductReadRepository _productReadRepository;
        readonly IHttpContextAccessor _httpContextAccessor;

        public GetAllProductImagesQueryHandler(IProductReadRepository productReadRepository, IHttpContextAccessor httpContextAccessor)
        {
            _productReadRepository = productReadRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<GetAllProductImagesQueryResponse>> Handle(GetAllProductImagesQueryRequest request, CancellationToken cancellationToken)
        {
            var product = await _productReadRepository.Table.Include(p => p.ProductImageFiles).FirstOrDefaultAsync(p => p.Id == Guid.Parse(request.Id));

            var httpRequest = _httpContextAccessor.HttpContext.Request;
            var baseUrl = $"{httpRequest.Scheme}://{httpRequest.Host}";

            return product?.ProductImageFiles.Select(p => new GetAllProductImagesQueryResponse
            {
                Path = $"{baseUrl}/{p.Path}",
                FileName = p.FileName,
                Id = p.Id
            }).ToList();
        }
    }
}
