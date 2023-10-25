using CAWA.Application.Repositories;
using CAWA.Domain.Common;
using CAWA.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CAWA.Persistence.Repositories
{
    public class WriteRepository<TEntity> : IWriteRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly CAWADbContext _context;
        public WriteRepository(CAWADbContext context)
        {
            _context = context;
        }
        public DbSet<TEntity> Table => _context.Set<TEntity>();

        public async Task<bool> AddAsync(TEntity model)
        {
            EntityEntry<TEntity> entityEntry = await Table.AddAsync(model);
            return entityEntry.State == EntityState.Added;
        }

        public async Task<bool> AddRangeAsync(List<TEntity> datas)
        {
            await Table.AddRangeAsync(datas);
            return true;
        }

        public bool Remove(TEntity model)
        {
            model.IsDeleted = true;
            model.DeletedAt = DateTime.Now;
            return Update(model);
        }

        public async Task<bool> RemoveAsync(string id)
        {
            TEntity entity = await Table.FindAsync(id);
            return Remove(entity);
        }

        public bool RemoveRange(List<TEntity> datas)
        {
            datas.ForEach(x => x.IsDeleted = true);
            datas.ForEach(x => x.DeletedAt = DateTime.Now);
            return UpdateRange(datas);
        }

        public async Task<int> SaveAsync() => await _context.SaveChangesAsync();

        public bool Update(TEntity model)
        {
            EntityEntry<TEntity> entityEntry = Table.Update(model);
            return entityEntry.State == EntityState.Modified;
        }

        public bool UpdateRange(List<TEntity> entities)
        {
            Table.UpdateRange(entities);
            return true;
        }
    }
}
