using CAWA.Domain.Common;
using CAWA.Domain.Enums;
using CAWA.Domain.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CAWA.Domain
{
    public class ApplicantInformation : BaseEntity
    {
        [StringLength(maximumLength: 4000, ErrorMessage = "Açıklama en fazla 4000 karakter olabilir!")]
        public string? Description { get; set; }
        public ApprovalStatus ApprovalStatus { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public string FirstPhotoPath { get; set; } = null!;
        public string SecondPhotoPath { get; set; } = null!;
        public string PdfFilePath { get; set; } = null!;
        [StringLength(maximumLength: 4000, ErrorMessage = "Açıklama en fazla 4000 karakter olabilir!")]
        public string? Message { get; set; }

        //navigation props
        [ForeignKey(nameof(AppUser))]
        public string AppUserId { get; set; } = null!;
        public virtual AppUser AppUser { get; set; }
    }
}
