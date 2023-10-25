using CAWA.Domain.Enums;

namespace CAWA.Application.ViewModels.ApplicantInformationVM
{
    public record ApplicantInformationResponseVM
    {
        public string Id { get; set; } = null!;
        public string? Message { get; set; } = null!;
        public ApprovalStatus ApprovalStatus { get; set; }
    }
}
