using Microsoft.AspNetCore.Http;

namespace CAWA.Application.ViewModels.AdminVM
{
    public record AdminCreateVM
    {
        public string Name { get; set; } = null!;
        public string SirName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? inviterName { get; set; }
        public DateTime BirthDate { get; set; }
        public bool IsSuperAdmin { get; set; }
        public IFormFile? ProfilePhoto { get; set; }
    }
}
