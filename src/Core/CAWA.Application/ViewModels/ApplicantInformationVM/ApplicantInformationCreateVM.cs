using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace CAWA.Application.ViewModels.ApplicantInformationVM
{
    public record ApplicantInformationCreateVM
    {
        public string? Description { get; set; }
        [Required(ErrorMessage = "Lütfen birinci fotoğrafınızı seçiniz!")]
        public IFormFile FirstPhoto { get; set; } = null!;
        [Required(ErrorMessage = "Lütfen ikinci fotoğrafınızı seçiniz!")]
        public IFormFile SecondPhoto { get; set; } = null!;
        [Required(ErrorMessage = "Lütfen pdf dosyanızı yükleyiniz!")]
        public IFormFile PdfFile { get; set; } = null!;

    }
}
