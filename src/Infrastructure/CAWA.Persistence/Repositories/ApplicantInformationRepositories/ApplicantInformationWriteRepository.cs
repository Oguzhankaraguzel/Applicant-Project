using CAWA.Application.Repositories.ApplicantInformationRepositories;
using CAWA.Domain;
using CAWA.Persistence.Context;

namespace CAWA.Persistence.Repositories.ApplicantInformationRepositories
{
    public class ApplicantInformationWriteRepository : WriteRepository<ApplicantInformation>, IApplicantInformationWriteRepository
    {
        public ApplicantInformationWriteRepository(CAWADbContext context) : base(context)
        {
        }
    }
}
