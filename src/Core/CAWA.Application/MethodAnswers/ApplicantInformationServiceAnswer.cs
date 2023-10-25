using CAWA.Domain;

namespace CAWA.Application.MethodAnswers
{
    public record ApplicantInformationServiceAnswer : BaseMethodAnswer
    {
        public ApplicantInformation? ApplicantInformation { get; set; }
        public List<ApplicantInformation>? ApplicantInformations { get; set; }
    }
}
