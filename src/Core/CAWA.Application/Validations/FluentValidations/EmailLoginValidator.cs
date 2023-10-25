using CAWA.Application.ViewModels;
using FluentValidation;

namespace CAWA.Application.Validations.FluentValidations
{
    public class EmailLoginValidator : AbstractValidator<EmailLoginVM>
    {
        public EmailLoginValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .NotNull()
                .EmailAddress()
                .WithMessage("Lütfen geçerli bir mail adresi giriniz");

            RuleFor(x => x.Name)
                .NotNull()
                .NotEmpty()
                .WithMessage("Ad Alanı boş Bırakılamaz")
                .Length(3, 50)
                .WithMessage("Ad en az 3, en fazla 50 karakter içerebilir!");

            RuleFor(x => x.SirName)
                .NotNull()
                .NotEmpty()
                .WithMessage("Ad Alanı boş Bırakılamaz")
                .Length(3, 75)
                .WithMessage("Ad en az 3, en fazla 75 karakter içerebilir!");

            RuleFor(user => user.BirthDate)
               .Must(BeValidBirthDate)
               .WithMessage("Geçerli bir doğum tarihi giriniz!");

        }

        private bool BeValidBirthDate(DateTime birthDate)
        {
            var threeYearsAgo = DateTime.Now.AddYears(-3);
            return birthDate <= threeYearsAgo && birthDate <= DateTime.Now;
        }
    }
}
