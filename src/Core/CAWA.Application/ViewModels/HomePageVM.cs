namespace CAWA.Application.ViewModels
{
    public record HomePageVM
    {
        public bool ExistingUser { get; set; } = false;
        public bool FilesUploaded { get; set; } = false;
        public string? UserName { get; set; }

    }
}
