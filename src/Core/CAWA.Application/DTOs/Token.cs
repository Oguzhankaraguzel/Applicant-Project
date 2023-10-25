namespace CAWA.Application.DTOs
{
    public class Token
    {
        public string AccessToken { get; set; } = null!;
        public DateTime Expiration { get; set; }
    }
}
