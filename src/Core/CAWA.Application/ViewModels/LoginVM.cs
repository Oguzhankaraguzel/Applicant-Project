namespace CAWA.Application.ViewModels
{
    public record LoginVM
    {
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
