using CAWA.Application.ViewModels.ApplicantInformationVM;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace CAWA.Application.Validations.FluentValidations
{
    public class AplicantInformationCreateValidation : AbstractValidator<ApplicantInformationCreateVM>
    {
        public AplicantInformationCreateValidation()
        {
            RuleFor(vm => vm.Description)
            .MaximumLength(4000)
            .When(vm => !string.IsNullOrEmpty(vm.Description));

            RuleFor(vm => vm.FirstPhoto)
                .Must(BeAValidImage)
                .WithMessage("Birinci fotoğraf JPEG veya PNG formatında olmalıdır ve en fazla 2MB boyutunda olmalıdır!");

            RuleFor(vm => vm.SecondPhoto)
                .Must(BeAValidImage)
                .WithMessage("İkinci fotoğraf JPEG veya PNG formatında olmalıdır ve en fazla 2MB boyutunda olmalıdır!");

            RuleFor(vm => vm.PdfFile)
                .Must(BeAValidPdf)
                .WithMessage("PDF dosyası uzantısı sadece PDF olmalıdır ve en fazla 2MB boyutunda olmalıdır!");
        }

        private bool BeAValidImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return false;

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var fileExtension = Path.GetExtension(file.FileName).ToLower();

            return allowedExtensions.Contains(fileExtension) && file.Length <= (2 * 1024 * 1024); // 2MB
        }

        private bool BeAValidPdf(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return false;

            var allowedExtensions = new[] { ".pdf" };
            var fileExtension = Path.GetExtension(file.FileName).ToLower();

            return allowedExtensions.Contains(fileExtension) && file.Length <= (2 * 1024 * 1024); // 2MB
        }
    }
}
