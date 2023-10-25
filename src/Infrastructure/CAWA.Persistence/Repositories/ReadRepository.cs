using CAWA.Application.Repositories;
using CAWA.Domain.Common;
using CAWA.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CAWA.Persistence.Repositories
{
    public class ReadRepository<TEntity> : IReadRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly CAWADbContext _context;
        public ReadRepository(CAWADbContext context)
        {
            _context = context;
        }
        public DbSet<TEntity> Table => _context.Set<TEntity>();

        public IQueryable<TEntity> GetAll(bool tracking = true)
        {
            var query = Table.AsQueryable();
            if (!tracking) query = query.AsNoTracking();
            return query;
        }

        public async Task<TEntity> GetByIdAsync(string id, bool tracking = true)
        {
            var query = Table.AsQueryable();
            if (!tracking) query = query.AsNoTracking();
            return await query.FirstOrDefaultAsync(entity => entity.Id == id);
        }

        public async Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> method, bool tracking = true)
        {
            var query = Table.AsQueryable();
            if (!tracking) query = query.AsNoTracking();
            return await query.FirstOrDefaultAsync(method);
        }

        public IQueryable<TEntity> GetWhere(Expression<Func<TEntity, bool>> method, bool tracking = true)
        {
            var query = Table.Where(method);
            if (!tracking) query = query.AsNoTracking();
            return query;
        }
    }
}
