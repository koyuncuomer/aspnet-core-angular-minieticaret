using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Application.Features.Queries.ProductImageFile.GetAllProductImages
{
    public class GetAllProductImagesQueryRequest : IRequest<List<GetAllProductImagesQueryResponse>>
    {
        public string Id { get; set; }
    }
}
