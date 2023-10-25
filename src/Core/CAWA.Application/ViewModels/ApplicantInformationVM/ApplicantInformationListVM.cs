using CAWA.Domain;
using CAWA.Domain.Enums;

namespace CAWA.Application.ViewModels.ApplicantInformationVM
{
    public record ApplicantInformationListVM
    {
        /// <summary>
        /// ApplicantInformation Id bilgisi
        /// </summary>
        public string Id { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string FirstPhotoPath { get; set; } = null!;
        public string SecondPhotoPath { get; set; } = null!;
        public string PDFFilePath { get; set; } = null!;
        public DateTime BirthDate { get; set; }
        public string? Description { get; set; }
        public string? Message { get; set; }
        public ApprovalStatus ApprovalStatus { get; set; }
        public int Age
        {
            get
            {
                DateTime currentDate = DateTime.Now;
                int age = currentDate.Year - BirthDate.Year;

                if (BirthDate.Date > currentDate.Date.AddYears(-age))
                    age--;

                return age;
            }
        }

        public static explicit operator ApplicantInformationListVM(ApplicantInformation entity)
        {
            ApplicantInformationListVM applicantInformationListVM = new ApplicantInformationListVM()
            {
                Id = entity.Id,
                FullName = entity.AppUser.Name + " " + entity.AppUser.SirName,
                PhoneNumber = entity.AppUser.PhoneNumber,
                Email = entity.AppUser.Email,
                FirstPhotoPath = entity.FirstPhotoPath,
                SecondPhotoPath = entity.SecondPhotoPath,
                PDFFilePath = entity.PdfFilePath,
                BirthDate = entity.AppUser.BirthDate,
                Description = entity.Description,
                Message = entity.Message,
                ApprovalStatus = entity.ApprovalStatus
            };
            return applicantInformationListVM;
        }
    }
}
