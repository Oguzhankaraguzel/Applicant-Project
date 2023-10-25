using CAWA.Application.Absractions.Services;
using CAWA.Application.Absractions.Token;
using CAWA.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CAWA.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructureServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<ITokenHandler, TokenHandler>();
            serviceCollection.AddSingleton<IMailService, MailService>();
            serviceCollection.AddTransient<IFileSaveService, FileSaveService>();
        }
    }
}
