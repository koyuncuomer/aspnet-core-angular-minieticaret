﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Common;

namespace Domain.Entities
{
    public class Order : BaseEntity
    {
        public string Description { get; set; }
        public string Address { get; set; }
        public Guid CustomerId { get; set; }

        public ICollection<Product> Products { get; set; }
        public Customer Customer { get; set; }
    }
}
