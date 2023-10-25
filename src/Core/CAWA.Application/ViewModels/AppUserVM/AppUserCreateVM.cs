namespace CAWA.Application.ViewModels.AppUserVM
{
    public record AppUserCreateVM
    {
        public string Name { get; set; } = null!;
        public string SirName { get; set; } = null!;
        public DateTime BirthDate { get; set; }
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
    }
}
