using CAWA.Application.EntityTypeConfigurations;
using CAWA.Domain;
using CAWA.Domain.Common;
using CAWA.Domain.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CAWA.Persistence.Context
{
    public class CAWADbContext : IdentityDbContext<AppUser, AppRole, string>
    {
        public CAWADbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<ApplicantInformation> ApplicantInformations { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<AppRole> AppRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new AppUserTypeConfiguration());
            builder.ApplyConfiguration(new ApplicantInformationEntityTypeConfiguration());

            base.OnModelCreating(builder);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var datas = ChangeTracker
                 .Entries<BaseEntity>();

            foreach (var data in datas)
            {
                _ = data.State switch
                {
                    EntityState.Modified => data.Entity.UpdatedAt = DateTime.Now,
                    _ => DateTime.UtcNow
                };
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
