using CAWA.Application.Absractions.Services;
using CAWA.Application.MethodAnswers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace CAWA.Infrastructure.Services
{
    public class FileSaveService : IFileSaveService
    {
        private readonly IWebHostEnvironment _hostingEnvironment;

        public FileSaveService(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<FileSaveAnswer> SavePhoto(IFormFile file, string? newFileName, string? oldFilePath)
        {
            FileSaveAnswer fileSaveAnswer = new();
            try
            {
                if (string.IsNullOrEmpty(oldFilePath))
                    if (System.IO.File.Exists(oldFilePath))
                        System.IO.File.Delete(oldFilePath);

                newFileName = (newFileName ?? "unknown") + ".jpeg";
                string newFilePath = Path.Combine(_hostingEnvironment.WebRootPath, "uploads", "images", newFileName);

                using (var fileStream = new FileStream(newFilePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }

                fileSaveAnswer.FilePath = "/uploads/images/" + newFileName;
                fileSaveAnswer.Success = true;
                return fileSaveAnswer;
            }
            catch (Exception ex)
            {
                fileSaveAnswer.Success = false;
                fileSaveAnswer.ExceptionMessage = ex.Message;
                return fileSaveAnswer;
            }
        }

        public async Task<FileSaveAnswer> SavePdf(IFormFile file, string? newFileName, string? oldFilePath)
        {
            FileSaveAnswer fileSaveAnswer = new();
            try
            {
                if (string.IsNullOrEmpty(oldFilePath))
                    if (System.IO.File.Exists(oldFilePath))
                        System.IO.File.Delete(oldFilePath);

                newFileName = (newFileName ?? "unknown") + ".pdf";
                string newFilePath = Path.Combine(_hostingEnvironment.WebRootPath, "uploads", "files", newFileName);

                using (var fileStream = new FileStream(newFilePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }

                fileSaveAnswer.FilePath = "/uploads/files/" + newFileName;
                fileSaveAnswer.Success = true;
                return fileSaveAnswer;
            }
            catch (Exception ex)
            {
                fileSaveAnswer.Success = false;
                fileSaveAnswer.ExceptionMessage = ex.Message;
                return fileSaveAnswer;
            }
        }

    }
}
