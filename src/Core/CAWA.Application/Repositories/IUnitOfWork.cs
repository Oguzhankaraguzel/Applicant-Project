using CAWA.Application.Repositories.ApplicantInformationRepositories;
using CAWA.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using SpectraUtils;

namespace CAWA.Application.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        ISpectraUtil SpectraUtil { get; }
        IApplicantInformationReadRepository AplicantInformationRead { get; }
        IApplicantInformationWriteRepository AplicantInformationWrite { get; }
        UserManager<AppUser> UserManager { get; }
        RoleManager<AppRole> RoleManager { get; }
        SignInManager<AppUser> SignInManager { get; }
    }
}
