namespace CAWA.Application.ViewModels.MailVM
{
    public class SendMailVM
    {
        public string Email { get; set; } = null!;
        public string Subject { get; set; } = null!;
        public string Body { get; set; } = null!;
        public bool IsHtml { get; set; } = true;
    }
}
