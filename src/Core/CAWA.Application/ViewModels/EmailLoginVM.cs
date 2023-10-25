namespace CAWA.Application.ViewModels
{
    public record EmailLoginVM
    {
        public string Name { get; set; }
        public string SirName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Email { get; set; }
    }
}
