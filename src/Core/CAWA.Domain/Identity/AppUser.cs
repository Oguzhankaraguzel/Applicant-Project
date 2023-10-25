using CAWA.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace CAWA.Domain.Identity
{
    public class AppUser : IdentityUser<string>
    {
        [StringLength(maximumLength: 50, MinimumLength = 3, ErrorMessage = "Kullanıcı İlk adı en az 3, en fazla 50 karakter olabilir!")]
        public string Name { get; set; } = null!;
        public string SirName { get; set; } = null!;
        public DateTime BirthDate { get; set; }
        public UserRank UserRank { get; set; }
        /// <summary>
        /// Yöneticilerin davet ederken kullandıkları davetçi ismi
        /// </summary>
        public string? InviterName { get; set; }
        /// <summary>
        /// Siteye başvuru yapan kullanıcıların kim tarafından davet edildiklerinin bilgisi
        /// </summary>
        public string? InvitingUserName { get; set; }
        public string? ProfilePhotoPath { get; set; }
        public string? ApplicantInformationId { get; set; }
        public virtual ApplicantInformation? ApplicantInformation { get; set; }
    }
}
