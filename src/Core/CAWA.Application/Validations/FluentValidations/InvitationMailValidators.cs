using CAWA.Application.ViewModels.MailVM;
using FluentValidation;

namespace CAWA.Application.Validations.FluentValidations
{
    public class InvitationMailValidators : AbstractValidator<InvitationMailVM>
    {
        public InvitationMailValidators()
        {
            RuleFor(x => x.Email)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("E-posta alanı boş olamaz.")
                .EmailAddress().WithMessage("Geçerli bir e-posta adresi girin.");

            RuleFor(x => x.Name)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("İsim alanı boş olamaz.")
                .MinimumLength(3).WithMessage("İsim en az 3 karakter olmalıdır.");

            RuleFor(x => x.Link)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Link alanı boş olamaz.");
        }
    }
}
