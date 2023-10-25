using CAWA.Domain.Identity;

namespace CAWA.Application.MethodAnswers
{
    public record CawaUserServiceAnswer : BaseMethodAnswer
    {
        public AppUser? user { get; set; }
        public List<AppUser>? users { get; set; }
    }
}
