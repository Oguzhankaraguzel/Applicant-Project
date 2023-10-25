using CAWA.Domain.Identity;
using Microsoft.AspNetCore.Identity;

namespace CAWA.Persistence.Context.SeedData
{
    public static class SeedData
    {
        public static async Task Initialize(RoleManager<AppRole> roleManager, UserManager<AppUser> userManager)
        {
            await SeedRoles(roleManager);
            await SeedUsers(userManager);
        }

        private static async Task SeedRoles(RoleManager<AppRole> roleManager)
        {
            string[] roles = { "Applicant", "Admin", "SuperAdmin" };

            foreach (var roleName in roles)
            {
                var roleExists = await roleManager.RoleExistsAsync(roleName);
                if (!roleExists)
                {
                    var role = new AppRole { Name = roleName, Id = Guid.NewGuid().ToString() };
                    await roleManager.CreateAsync(role);
                }
            }
        }

        private static async Task SeedUsers(UserManager<AppUser> userManager)
        {
            var user = new AppUser
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Cawa",
                SirName = "PROJECT",
                BirthDate = new DateTime(1995, 01, 10),
                UserName = "CawaProjectAdmin",
                Email = "cawa.project@gmial.com",
                PhoneNumber = "+905554449999",
                UserRank = Domain.Enums.UserRank.Site_Officer
            };

            var existingUser = await userManager.FindByNameAsync(user.UserName);
            if (existingUser == null)
            {
                var result = await userManager.CreateAsync(user, "P2hW8fFp");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "SuperAdmin");
                }
            }
        }
    }
}
