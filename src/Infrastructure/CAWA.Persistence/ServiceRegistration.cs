using CAWA.Application.Absractions.Services;
using CAWA.Application.Repositories;
using CAWA.Application.Repositories.ApplicantInformationRepositories;
using CAWA.Application.Services;
using CAWA.Domain.Identity;
using CAWA.Persistence.Context;
using CAWA.Persistence.Repositories;
using CAWA.Persistence.Repositories.ApplicantInformationRepositories;
using CAWA.Persistence.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SpectraUtils;

namespace CAWA.Persistence
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceServices(this IServiceCollection services)
        {
            #region Databases and identity
            services.AddDbContext<CAWADbContext>(options => options.UseSqlServer(Configuration.ConnectionString));

            services.AddIdentity<AppUser, AppRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 3;
                //options.Password.RequiredUniqueChars = 0;
                //options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
                //options.Lockout.MaxFailedAccessAttempts = 3;
                //options.Lockout.AllowedForNewUsers = true;
                //options.SignIn.RequireConfirmedAccount = false;
            }).AddEntityFrameworkStores<CAWADbContext>()
                .AddDefaultTokenProviders()
                .AddUserManager<UserManager<AppUser>>()
                .AddSignInManager<SignInManager<AppUser>>()
                .AddRoleManager<RoleManager<AppRole>>();

            services.Configure<IdentityOptions>(options => options.SignIn.RequireConfirmedEmail = false);
            #endregion

            #region Repositories and Services
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<ICawaUserService, CawaUserService>();

            services.AddScoped<ISpectraUtil, SpectraUtil>();

            services.AddScoped<IApplicantInformationServices, ApplicantInformationService>();
            services.AddScoped<IApplicantInformationReadRepository, ApplicantInformationReadRepository>();
            services.AddScoped<IApplicantInformationWriteRepository, ApplicantInformationWriteRepository>();
            #endregion
        }
    }
}
