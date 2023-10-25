using CAWA.Application.Repositories;
using CAWA.Application.Repositories.ApplicantInformationRepositories;
using CAWA.Domain.Identity;
using CAWA.Persistence.Context;
using Microsoft.AspNetCore.Identity;
using SpectraUtils;

namespace CAWA.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CAWADbContext _context;

        public UserManager<AppUser> UserManager { get; }

        public RoleManager<AppRole> RoleManager { get; }

        public SignInManager<AppUser> SignInManager { get; }

        public ISpectraUtil SpectraUtil { get; }

        public IApplicantInformationReadRepository AplicantInformationRead { get; }

        public IApplicantInformationWriteRepository AplicantInformationWrite { get; }

        public UnitOfWork(
            CAWADbContext context,
            UserManager<AppUser> userManager,
            RoleManager<AppRole> roleManager,
            SignInManager<AppUser> signInManager,
            ISpectraUtil spectraUtil,
            IApplicantInformationReadRepository aplicantInformationRead,
            IApplicantInformationWriteRepository aplicantInformationWrite
            )
        {
            _context = context;
            UserManager = userManager;
            RoleManager = roleManager;
            SignInManager = signInManager;
            SpectraUtil = spectraUtil;
            AplicantInformationRead = aplicantInformationRead;
            AplicantInformationWrite = aplicantInformationWrite;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }
    }
}
