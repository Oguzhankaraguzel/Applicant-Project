using CAWA.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace CAWA.Application.Repositories
{
    public interface IRepository<T> where T : BaseEntity
    {
        DbSet<T> Table { get; }
    }
}
