using CAWA.Domain;
using CAWA.Domain.Enums;

namespace CAWA.Application.ViewModels.ApplicantInformationVM
{
    public record ApplicantInformationResultVM
    {
        public string Id { get; set; } = null!;
        public string? Description { get; set; }
        public ApprovalStatus ApprovalStatus { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public string FirstPhotoPath { get; set; } = null!;
        public string SecondPhotoPath { get; set; } = null!;
        public string PdfFilePath { get; set; } = null!;
        public string? Message { get; set; }

        public static explicit operator ApplicantInformationResultVM(ApplicantInformation entity)
        {
            return new ApplicantInformationResultVM()
            {
                Id = entity.Id,
                Description = entity.Description,
                ApprovalStatus = entity.ApprovalStatus,
                ApprovalDate = entity.ApprovalDate,
                FirstPhotoPath = entity.FirstPhotoPath,
                SecondPhotoPath = entity.SecondPhotoPath,
                PdfFilePath = entity.PdfFilePath,
                Message = entity.Message,
            };
        }
    }
}
