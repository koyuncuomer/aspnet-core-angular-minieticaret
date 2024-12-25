using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Concretes;

namespace Persistence
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceServices(this IServiceCollection services)
        {
            services.AddSingleton<IProductService, ProductService>();
        }
    }
}
