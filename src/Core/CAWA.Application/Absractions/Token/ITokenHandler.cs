using CAWA.Domain.Identity;

namespace CAWA.Application.Absractions.Token
{
    public interface ITokenHandler
    {
        DTOs.Token CreateAccessToken(int day, AppUser appUser);
    }
}
