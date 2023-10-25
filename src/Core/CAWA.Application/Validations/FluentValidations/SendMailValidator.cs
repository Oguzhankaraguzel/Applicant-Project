using CAWA.Application.ViewModels.MailVM;
using FluentValidation;

namespace CAWA.Application.Validations.FluentValidations
{
    public class SendMailValidator : AbstractValidator<SendMailVM>
    {
        public SendMailValidator()
        {

            RuleFor(x => x.Email)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("E-posta alanı boş olamaz.")
                .EmailAddress().WithMessage("Geçerli bir e-posta adresi girin.");

            RuleFor(x => x.Subject)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Konu alanı boş olamaz.")
                .MaximumLength(100).WithMessage("Konu en fazla 100 karakter olabilir.");

            RuleFor(x => x.Body)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Gövde alanı boş olamaz.")
                .MinimumLength(10).WithMessage("Gövde en az 10 karakter olmalıdır.");
        }
    }
}
