using CAWA.Domain.Enums;
using CAWA.Domain.Identity;

namespace CAWA.Application.ViewModels.AdminVM
{
    public record AdminListVM
    {
        public string FullName { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? inviterName { get; set; }
        public string? ProfilePhotoPath { get; set; }
        public DateTime BirthDate { get; set; }
        public UserRank UserRank { get; set; }
        public int Age
        {
            get
            {
                DateTime currentDate = DateTime.Now;
                int age = currentDate.Year - BirthDate.Year;

                if (BirthDate.Date > currentDate.Date.AddYears(-age))
                    age--;

                return age;
            }
        }

        public static explicit operator AdminListVM(AppUser appUsers)
        {
            AdminListVM adminListVMs = new AdminListVM()
            {
                FullName = appUsers.Name + " " + appUsers.SirName,
                UserName = appUsers.UserName,
                PhoneNumber = appUsers.PhoneNumber,
                Email = appUsers.Email,
                inviterName = appUsers.InviterName,
                ProfilePhotoPath = appUsers.ProfilePhotoPath,
                BirthDate = appUsers.BirthDate,
                UserRank = appUsers.UserRank,
            };
            return adminListVMs;
        }
    }
}
