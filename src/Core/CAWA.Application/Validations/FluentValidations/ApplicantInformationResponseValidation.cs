using CAWA.Application.ViewModels.ApplicantInformationVM;
using FluentValidation;

namespace CAWA.Application.Validations.FluentValidations
{
    public class ApplicantInformationResponseValidation : AbstractValidator<ApplicantInformationResponseVM>
    {
        public ApplicantInformationResponseValidation()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Id boş olamaz.");

            RuleFor(x => x.Message)
                .Must(BeValidDeniedMessage)
                .When(x => !string.IsNullOrEmpty(x.Message))
                .WithMessage("DeniedMessage, 4000 karakterden fazla olamaz.");

            RuleFor(x => x.ApprovalStatus).IsInEnum().WithMessage("Geçersiz ApprovalStatus değeri.");
        }

        private bool BeValidDeniedMessage(string? deniedMessage)
        {
            return deniedMessage!.Length <= 4000;
        }
    }
}
