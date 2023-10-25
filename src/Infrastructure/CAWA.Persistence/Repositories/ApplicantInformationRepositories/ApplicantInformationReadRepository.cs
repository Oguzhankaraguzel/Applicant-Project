using CAWA.Application.Repositories.ApplicantInformationRepositories;
using CAWA.Domain;
using CAWA.Persistence.Context;

namespace CAWA.Persistence.Repositories.ApplicantInformationRepositories
{
    public class ApplicantInformationReadRepository : ReadRepository<ApplicantInformation>, IApplicantInformationReadRepository
    {
        public ApplicantInformationReadRepository(CAWADbContext context) : base(context)
        {
        }
    }
}
