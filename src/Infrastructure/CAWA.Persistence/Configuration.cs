using Microsoft.Extensions.Configuration;

namespace CAWA.Persistence
{
    static internal class Configuration
    {
        static internal string ConnectionString
        {
            get
            {
                ConfigurationManager configurationManager = new();
                try
                {
                    configurationManager.SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../../Presentation/CAWA.MVCUI"));
                    configurationManager.AddJsonFile("appsettings.json");
                }
                catch
                {
                    configurationManager.AddJsonFile("appsettings.Production.json");
                }

                return configurationManager.GetConnectionString("DefaultConnection");
            }
        }
    }
}
