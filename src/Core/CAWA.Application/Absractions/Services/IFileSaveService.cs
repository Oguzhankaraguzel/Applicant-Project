using CAWA.Application.MethodAnswers;
using Microsoft.AspNetCore.Http;

namespace CAWA.Application.Absractions.Services
{
    public interface IFileSaveService
    {
        Task<FileSaveAnswer> SavePhoto(IFormFile file, string? newFileName, string? oldFilePath);
        Task<FileSaveAnswer> SavePdf(IFormFile file, string? newFileName, string? oldFilePath);
    }
}
