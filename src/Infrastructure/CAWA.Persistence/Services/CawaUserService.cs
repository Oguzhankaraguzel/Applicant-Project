using CAWA.Application.Absractions.Services;
using CAWA.Application.Consts;
using CAWA.Application.MethodAnswers;
using CAWA.Application.Repositories;
using CAWA.Application.ViewModels.AdminVM;
using CAWA.Application.ViewModels.ApplicantInformationVM;
using CAWA.Application.ViewModels.AppUserVM;
using CAWA.Domain;
using CAWA.Domain.Enums;
using CAWA.Domain.Identity;
using CAWA.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace CAWA.Persistence.Services
{
    public class CawaUserService : ICawaUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileSaveService _fileSaveService;
        private readonly CAWADbContext _context;

        public CawaUserService(IUnitOfWork unitOfWork, IFileSaveService fileSaveService, CAWADbContext context)
        {
            _unitOfWork = unitOfWork;
            _fileSaveService = fileSaveService;
            _context = context;
        }

        public async Task<CawaUserServiceAnswer> CreateReference(ApplicantInformationCreateVM createReference, string userName)
        {
            CawaUserServiceAnswer answer = await CheckUserByUserName(userName);

            var firstPhotoResult = await _fileSaveService.SavePhoto(createReference.FirstPhoto, answer.user.UserName + "FisrtPhoto", null);
            var secondPhotoResult = await _fileSaveService.SavePhoto(createReference.SecondPhoto, answer.user.UserName + "SecondPhoto", null);
            var pdfFileResult = await _fileSaveService.SavePdf(createReference.PdfFile, answer.user.UserName + "PDFFile", null);

            answer.Success = firstPhotoResult.Success & secondPhotoResult.Success & pdfFileResult.Success;
            answer.Message = firstPhotoResult.Message + " \n" + secondPhotoResult.Message + " \n" + pdfFileResult.Message;
            answer.ExceptionMessage = firstPhotoResult.ExceptionMessage + " \n" + secondPhotoResult.ExceptionMessage + " \n" + pdfFileResult.ExceptionMessage;

            if (!answer.Success) return answer;

            ApplicantInformation applicantInformation = new ApplicantInformation()
            {
                Description = createReference.Description,
                FirstPhotoPath = firstPhotoResult.FilePath,
                SecondPhotoPath = secondPhotoResult.FilePath,
                PdfFilePath = pdfFileResult.FilePath,
                AppUserId = answer.user.Id,
            };

            try
            {
                var result = await _unitOfWork.AplicantInformationWrite.AddAsync(applicantInformation);
                answer.Success = result;
                if (result)
                {
                    await _unitOfWork.AplicantInformationWrite.SaveAsync();
                    answer.Message += "Dosyalar başarılı bir biçimde kaydedildi!\n";
                    return answer;
                }
                else
                {
                    answer.Message += "Dosyalar kaydedilirken bir hata oluştu!\n";
                    return answer;
                }
            }
            catch (Exception ex)
            {
                answer.Message += "Dosyalar kaydedilirken bir hata oluştu!\n";
                answer.ExceptionMessage = ex.Message;
                return answer;
            }
        }

        public async Task<CawaUserServiceAnswer> CreateUser(AppUserCreateVM appUserCreateVM, string? inviterUserName)
        {
            CawaUserServiceAnswer answer = await CheckUserByEmail(appUserCreateVM.Email);

            if (answer.Success) return answer;

            string firstName = _unitOfWork.SpectraUtil.NameEdit.NameCorrection(appUserCreateVM.Name.Split(" "));
            string sirName = _unitOfWork.SpectraUtil.NameEdit.SirNameCorrection(appUserCreateVM.SirName);
            string userName = _unitOfWork.SpectraUtil.NameEdit.CreateUserName(firstName.Split(" ")[0].Replace("ı", "i") + sirName);

            AppUser user = new AppUser()
            {
                Id = Guid.NewGuid().ToString(),
                Name = firstName,
                SirName = sirName,
                Email = appUserCreateVM.Email,
                BirthDate = appUserCreateVM.BirthDate,
                PhoneNumber = appUserCreateVM.PhoneNumber,
                UserName = userName,
                UserRank = UserRank.Applicant,
                InvitingUserName = inviterUserName
            };

            try
            {
                var result = await _unitOfWork.UserManager.CreateAsync(user);
                answer.Success = result.Succeeded;
                if (result.Succeeded)
                {
                    answer.user = await _unitOfWork.UserManager.FindByNameAsync(user.UserName);
                    answer.Message += "Kullanıcı başarılı bir biçimde oluşturuldu!\n";

                    await _unitOfWork.UserManager.AddToRoleAsync(answer.user, RoleNames.Applicant);

                    answer.user = await _unitOfWork.UserManager.FindByNameAsync(user.UserName);
                    return answer;
                }
                else
                {
                    answer.Message += "Kullanıcı oluşturulurken bir hata oluştu!\n";
                    answer.ExceptionMessage += string.Join("! ", result.Errors);
                    return answer;
                }
            }
            catch (Exception ex)
            {
                answer.Message += "Kullanıcı oluşturulurken bir hata oluştu!\n";
                answer.ExceptionMessage += ex.Message;
                return answer;
            }
        }

        public async Task<CawaUserServiceAnswer> GetUser(string username)
        {
            CawaUserServiceAnswer answer = await CheckUserByUserName(username);
            return answer;
        }

        public async Task<CawaUserServiceAnswer> GetUserWithAllDetails(string username)
        {
            CawaUserServiceAnswer answer = await CheckUserByUserName(username);

            if (!answer.Success) return answer;

            try
            {
                answer.user.ApplicantInformation = await _unitOfWork.AplicantInformationRead.GetSingleAsync(x => x.AppUserId == answer.user.Id);
                answer.Success = true;
                return answer;
            }
            catch (Exception ex)
            {
                answer.Success = false;
                answer.ExceptionMessage += ex.Message + "!";
                return answer;
            }
        }

        public async Task<CawaUserServiceAnswer> Login(string username)
        {
            CawaUserServiceAnswer answer = await CheckUserByUserName(username);

            if (!answer.Success) return answer;

            await _unitOfWork.SignInManager.SignInAsync(answer.user, false);
            answer.Message += "Kullanıcı başarılı bir şekilde giriş yaptı!\n";
            return answer;
        }

        public async Task<CawaUserServiceAnswer> Login(string username, string password)
        {
            CawaUserServiceAnswer answer = await CheckUserByUserName(username);

            if (!answer.Success) return answer;

            var result = await _unitOfWork.SignInManager.PasswordSignInAsync(answer.user, password, false, false);
            answer.Success = result.Succeeded;
            if (result.Succeeded)
            {
                answer.Message += "Kullanıcı başarılı bir şekilde giriş yaptı!\n";
                return answer;
            }

            answer.Message += "Hatalı şifre!\n";
            return answer;
        }

        public async Task<bool> LogOut()
        {
            try
            {
                await _unitOfWork.SignInManager.SignOutAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<CawaUserServiceAnswer> CheckUserByUserName(string username)
        {
            CawaUserServiceAnswer answer = new();

            var user = await _unitOfWork.UserManager.FindByNameAsync(username);
            if (user is null)
            {
                answer.Success = false;
                answer.Message = "Böyle bir kullanıcı bulunmamaktadır!\n";
                return answer;
            }
            answer.Success = true;
            answer.Message = "Böyle bir kullanıcı bulunmaktadır!\n";
            answer.user = user;
            return answer;
        }

        public async Task<CawaUserServiceAnswer> CheckUserByEmail(string email)
        {
            CawaUserServiceAnswer answer = new();

            var user = await _unitOfWork.UserManager.FindByEmailAsync(email);
            if (user is null)
            {
                answer.Success = false;
                answer.Message = "Böyle bir kullanıcı bulunmamaktadır!\n";
                return answer;
            }
            answer.Success = true;
            answer.user = user;
            answer.Message = "Böyle bir kullanıcı bulunmaktadır!\n";
            return answer;
        }

        public async Task<CawaUserServiceAnswer> GetAllUsers()
        {
            CawaUserServiceAnswer answer = new();
            answer.users = _context.AppUsers.ToList();
            return answer;
        }

        public async Task<CawaUserServiceAnswer> GetAllUsersWithAllDetails()
        {
            CawaUserServiceAnswer answer = new();
            answer.users = _context.AppUsers.Include(x => x.ApplicantInformation).ToList();
            return answer;
        }

        public async Task<CawaUserServiceAnswer> GetAllUsersWithAllDetailsAdminFilter(string username)
        {
            CawaUserServiceAnswer answer = await CheckUserByUserName(username);
            answer.users = _context.AppUsers.Include(x => x.ApplicantInformation).Where(x => x.InvitingUserName == (answer.user.InviterName ?? answer.user.UserName)).ToList();
            return answer;
        }

        public async Task<CawaUserServiceAnswer> UpdateUserInviter(string username, string inviterName)
        {
            var answer = await CheckUserByUserName(username);
            if (answer.Success)
            {
                answer.user.InvitingUserName = inviterName;
                var result = await _unitOfWork.UserManager.UpdateAsync(answer.user);
                answer.Success = result.Succeeded;

                if (answer.Success) answer.Message = "Kullanıcı başarıyla güncellendi";
                else answer.Message = "Kullanıcı güncellenirken bir hata oluştu";
            }
            return answer;
        }

        public async Task<CawaUserServiceAnswer> CreateAdmin(AdminCreateVM adminCreateVM)
        {
            CawaUserServiceAnswer answer = await CheckUserByEmail(adminCreateVM.Email);
            if (answer.Success)
            {
                answer.Success = false;
                return answer;
            }

            try
            {
                string firstName = _unitOfWork.SpectraUtil.NameEdit.NameCorrection(adminCreateVM.Name.Split(" "));
                string sirName = _unitOfWork.SpectraUtil.NameEdit.SirNameCorrection(adminCreateVM.SirName);
                string userName = _unitOfWork.SpectraUtil.NameEdit.CreateUserName(firstName + sirName);

                AppUser appUser = new AppUser()
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = userName,
                    Email = adminCreateVM.Email.Replace(" ", ""),
                    PhoneNumber = adminCreateVM.PhoneNumber.Replace(" ", ""),
                    InviterName = adminCreateVM.inviterName,
                    Name = firstName,
                    SirName = sirName,
                    UserRank = adminCreateVM.IsSuperAdmin == true ? UserRank.Site_Officer : UserRank.Site_Admin
                };

                if (adminCreateVM.ProfilePhoto is not null)
                {
                    var result = await _fileSaveService.SavePhoto(adminCreateVM.ProfilePhoto, appUser.UserName, null);
                    if (result.Success) appUser.ProfilePhotoPath = result.FilePath;
                }

                var response = await _unitOfWork.UserManager.CreateAsync(appUser, adminCreateVM.Password);
                if (response.Succeeded)
                {
                    answer = await CheckUserByUserName(appUser.UserName);
                    await _unitOfWork.UserManager.AddToRoleAsync(answer.user, adminCreateVM.IsSuperAdmin == false ? RoleNames.Admin : RoleNames.SuperAdmin);
                    answer.Message = "Kullanıcı başarılı bir biçimde oluşturuldu";
                    return answer;
                }
                else
                {
                    answer.Success = false;
                    answer.Message = "Kullanıcı oluşturulurken bir hata oluştu";
                    var errorMessages = response.Errors.Select(e => e.Description).ToList();
                    answer.ExceptionMessage = string.Join("\n", errorMessages);
                    return answer;
                }
            }
            catch (Exception ex)
            {
                answer.Message = "Bir hata oluştu";
                answer.ExceptionMessage = ex.Message;
                answer.Success = false;
                return answer;
            }

        }

        public async Task<CawaUserServiceAnswer> GetAllAdmin(bool includesSuperAdmin = false)
        {
            CawaUserServiceAnswer answer = await GetAllUsers();
            try
            {
                if (includesSuperAdmin)
                {
                    answer.users = answer.users.Where(x => x.UserRank == UserRank.Site_Officer || x.UserRank == UserRank.Site_Admin).ToList();
                    answer.Success = true;
                    return answer;
                }
                answer.users = answer.users.Where(x => x.UserRank == UserRank.Site_Admin).ToList();
                answer.Success = true;
                return answer;
            }
            catch (Exception ex)
            {
                answer.Success = false;
                answer.Message = "Bir hata oluştu";
                answer.ExceptionMessage = ex.Message;
                return answer;
            }

        }
    }
}
