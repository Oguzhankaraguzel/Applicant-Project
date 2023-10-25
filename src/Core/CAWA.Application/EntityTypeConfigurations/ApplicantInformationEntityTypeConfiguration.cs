using CAWA.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CAWA.Application.EntityTypeConfigurations
{
    public class ApplicantInformationEntityTypeConfiguration : IEntityTypeConfiguration<ApplicantInformation>
    {
        public void Configure(EntityTypeBuilder<ApplicantInformation> builder)
        {
            builder.Property(ai => ai.ApprovalStatus).HasConversion<string>();
        }
    }
}
