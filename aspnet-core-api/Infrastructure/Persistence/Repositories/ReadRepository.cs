using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Application.Repositories;
using Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;

namespace Persistence.Repositories
{
    public class ReadRepository<T> : IReadRepository<T> where T : BaseEntity
    {
        private readonly ETicaretAPIDbContext _context;
        public ReadRepository(ETicaretAPIDbContext context)
        {
            _context = context;
        }

        public DbSet<T> Table => _context.Set<T>();

        private IQueryable<T> ApplyTracking(IQueryable<T> query, bool tracking)
        {
            return tracking ? query : query.AsNoTracking();
        }

        public IQueryable<T> GetAll(bool tracking = true)
        {
            return ApplyTracking(Table.AsQueryable(), tracking);
        }

        public IQueryable<T> GetWhere(Expression<Func<T, bool>> method, bool tracking = true)
        {
            return ApplyTracking(Table.Where(method), tracking);
        }

        public async Task<T> GetSingleAsync(Expression<Func<T, bool>> method, bool tracking = true)
        {
            return await ApplyTracking(Table.AsQueryable(), tracking).FirstOrDefaultAsync(method);
        }

        public async Task<T> GetByIdAsync(string id, bool tracking = true)
        {
            if (!Guid.TryParse(id, out var guidId))
                throw new ArgumentException("Invalid ID format", nameof(id));
            return await ApplyTracking(Table.AsQueryable(), tracking).FirstOrDefaultAsync(data => data.Id == guidId);
        }
    }
}
