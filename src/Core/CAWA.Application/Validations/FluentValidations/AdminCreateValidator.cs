using CAWA.Application.ViewModels.AdminVM;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace CAWA.Application.Validations.FluentValidations
{
    public class AdminCreateValidator : AbstractValidator<AdminCreateVM>
    {
        public AdminCreateValidator()
        {
            RuleFor(vm => vm.ProfilePhoto)
                .Must(BeAValidImage)
                .WithMessage("Birinci fotoğraf JPEG veya PNG formatında olmalıdır ve en fazla 2MB boyutunda olmalıdır!");

            RuleFor(vm => vm.Name)
           .NotEmpty()
           .WithMessage("Ad alanı boş olamaz!")
           .Length(3, 50)
           .WithMessage("Ad en az 3, en fazla 50 karakter içerebilir!");

            RuleFor(vm => vm.SirName)
                .NotEmpty()
                .WithMessage("Soyad alanı boş olamaz!")
                .Length(3, 75)
                .WithMessage("Soyad en az 3, en fazla 75 karakter içerebilir!");

            RuleFor(vm => vm.BirthDate)
                .Must(BeValidBirthDate)
                .WithMessage("Geçerli bir doğum tarihi giriniz!");

            RuleFor(vm => vm.Email)
                .NotEmpty()
                .WithMessage("E-posta alanı boş olamaz.");

            RuleFor(vm => vm.PhoneNumber)
                .NotEmpty()
                .WithMessage("Telefon numarası boş olamaz!")
                .Matches(@"^(\+90|0)?\s*(\(\d{3}\)[\s-]*\d{3}[\s-]*\d{2}[\s-]*\d{2}|\(\d{3}\)[\s-]*\d{3}[\s-]*\d{4}|\(\d{3}\)[\s-]*\d{7}|\d{3}[\s-]*\d{3}[\s-]*\d{4}|\d{3}[\s-]*\d{3}[\s-]*\d{2}[\s-]*\d{2})$")
                .WithMessage("Geçerli bir telefon numarası formatı giriniz (örn. +905554443322, 05554443322)!");

            RuleFor(vm => vm.Password)
            .NotEmpty()
            .WithMessage("Şifre Alanı Boş Bırakılamaz.")
            .MinimumLength(8)
            .WithMessage("Şifre en az 8 karakter uzunluğunda olmalıdır.")
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$")
            .WithMessage("Şifre en az bir küçük harf, bir büyük harf ve bir rakam içermelidir.");

        }

        private bool BeAValidImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return true;

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var fileExtension = Path.GetExtension(file.FileName).ToLower();

            return allowedExtensions.Contains(fileExtension) && file.Length <= (2 * 1024 * 1024);
        }

        private bool BeValidBirthDate(DateTime birthDate)
        {
            var eightTeenYearsAgo = DateTime.Now.AddYears(-18);
            return birthDate <= eightTeenYearsAgo && birthDate <= DateTime.Now;
        }
    }
}
