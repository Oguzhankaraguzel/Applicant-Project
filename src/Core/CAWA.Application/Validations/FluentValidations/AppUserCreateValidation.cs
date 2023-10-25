using CAWA.Application.ViewModels.AppUserVM;
using FluentValidation;

namespace CAWA.Application.Validations.FluentValidations
{
    public class AppUserCreateValidation : AbstractValidator<AppUserCreateVM>
    {
        public AppUserCreateValidation()
        {
            RuleFor(user => user.Name)
           .NotEmpty()
           .WithMessage("Ad alanı boş olamaz!")
           .Length(3, 50)
           .WithMessage("Ad en az 3, en fazla 50 karakter içerebilir!");

            RuleFor(user => user.SirName)
                .NotEmpty()
                .WithMessage("Soyad alanı boş olamaz!")
                .Length(3, 75)
                .WithMessage("Soyad en az 3, en fazla 75 karakter içerebilir!");

            RuleFor(user => user.BirthDate)
                .Must(BeValidBirthDate)
                .WithMessage("Geçerli bir doğum tarihi giriniz!");

            RuleFor(user => user.Email)
                .NotEmpty()
                .WithMessage("E-posta alanı boş olamaz.");

            RuleFor(user => user.PhoneNumber)
                .NotEmpty()
                .WithMessage("Telefon numarası boş olamaz!")
                .Matches(@"^(\+90|0)?\s*(\(\d{3}\)[\s-]*\d{3}[\s-]*\d{2}[\s-]*\d{2}|\(\d{3}\)[\s-]*\d{3}[\s-]*\d{4}|\(\d{3}\)[\s-]*\d{7}|\d{3}[\s-]*\d{3}[\s-]*\d{4}|\d{3}[\s-]*\d{3}[\s-]*\d{2}[\s-]*\d{2})$")
                .WithMessage("Geçerli bir telefon numarası formatı giriniz (örn. +905554443322, 05554443322)!");
        }
        private bool BeValidBirthDate(DateTime birthDate)
        {
            var threeYearsAgo = DateTime.Now.AddYears(-3);
            return birthDate <= threeYearsAgo && birthDate <= DateTime.Now;
        }
    }
}
