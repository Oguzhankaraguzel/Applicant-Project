using CAWA.Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CAWA.Application.EntityTypeConfigurations
{
    public class AppUserTypeConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.HasKey(au => au.Id);

            builder.Property(au => au.UserRank).HasConversion<string>();

            builder.Property(au => au.InviterName).IsRequired(false);
            builder.HasIndex(au => au.InviterName).IsUnique();
            builder.HasIndex(au => au.Email).IsUnique();
            builder.HasIndex(au => au.PhoneNumber).IsUnique();
        }
    }
}
