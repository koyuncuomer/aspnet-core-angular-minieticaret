﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.RequestParameters;
using MediatR;

namespace Application.Features.Queries.Product.GetAllProducts
{
    public class GetAllProductsQueryRequest : IRequest<GetAllProductsQueryResponse>
    {
        //public Pagination Pagination { get; set; }
        public int Page { get; set; } = 1;
        public int Size { get; set; } = 5;
    }
}
