using CAWA.Domain.Identity;

namespace CAWA.Application.ViewModels.AdminVM
{
    public record AdminLayoutVM
    {
        public string Name { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? PhotoPath { get; set; }

        public static explicit operator AdminLayoutVM(AppUser user)
        {
            AdminLayoutVM vm = new AdminLayoutVM()
            {
                Name = user.Name,
                Email = user.Email,
                FullName = user.Name + " " + user.SirName,
                PhotoPath = user.ProfilePhotoPath
            };
            return vm;
        }
    }
}
