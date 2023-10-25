using CAWA.Application.MethodAnswers;
using CAWA.Application.Repositories;
using CAWA.Application.Services;
using CAWA.Application.ViewModels.ApplicantInformationVM;
using Microsoft.EntityFrameworkCore;

namespace CAWA.Persistence.Services
{
    public class ApplicantInformationService : IApplicantInformationServices
    {
        private readonly IUnitOfWork _unitOfWork;
        public ApplicantInformationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ApplicantInformationServiceAnswer> DeleteApplicantInformation(string Id, string? inviterUserName)
        {
            ApplicantInformationServiceAnswer answer = new();
            try
            {
                if (string.IsNullOrEmpty(inviterUserName))
                {
                    var applicantInformation = await _unitOfWork.AplicantInformationRead.GetByIdAsync(Id);
                    var user = await _unitOfWork.UserManager.FindByIdAsync(applicantInformation.AppUserId);
                    user.InvitingUserName = inviterUserName;
                    var result = await _unitOfWork.UserManager.UpdateAsync(user);
                    _ = result.Succeeded == true ? answer.Message = "Kullanıcı başarıyla güncellendi\n" : answer.Message = "Kullanıcı güncellenirken bir sorun oluştu!\n";
                }

                answer.Success = await _unitOfWork.AplicantInformationWrite.RemoveAsync(Id);
                await _unitOfWork.AplicantInformationWrite.SaveAsync();
                answer.Message += answer.Success == true ? "Silme işlemi başarılı\n" : "silme işlemi gerçekleştirilemedi\n";
                return answer;
            }
            catch (Exception ex)
            {
                answer.Success = false;
                answer.Message += "Silme işlemi sırasında bir hata meydana geldi";
                answer.ExceptionMessage = ex.Message;
                return answer;
            }

        }

        public async Task<ApplicantInformationServiceAnswer> GetAllApplicantInformation(bool withUser = true, bool onlyUninvited = false)
        {
            ApplicantInformationServiceAnswer answer = new();
            try
            {
                answer.ApplicantInformations = _unitOfWork.AplicantInformationRead.GetAll().ToList();

                if (withUser)
                    answer.ApplicantInformations.ForEach(async x => x.AppUser = await _unitOfWork.UserManager.FindByIdAsync(x.AppUserId));
                if (onlyUninvited)
                    answer.ApplicantInformations = answer.ApplicantInformations.Where(x => x.AppUser.InvitingUserName == null).ToList();

                answer.Success = true;
                answer.Message = "Başarılı";
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

        public async Task<ApplicantInformationServiceAnswer> GetApplicantInformation(string Id, bool withUser = true)
        {
            ApplicantInformationServiceAnswer answer = new();
            try
            {
                answer.ApplicantInformation = await _unitOfWork.AplicantInformationRead.GetByIdAsync(Id);
                if (withUser)
                    answer.ApplicantInformation.AppUser = await _unitOfWork.UserManager.FindByIdAsync(answer.ApplicantInformation.AppUserId);

                answer.Success = true;
                answer.Message = "Başarılı";
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

        public async Task<ApplicantInformationServiceAnswer> GetApplicantInformationWithInviterFilter(string? inviterUserName)
        {
            ApplicantInformationServiceAnswer answer = new();
            try
            {
                answer.ApplicantInformations = _unitOfWork.AplicantInformationRead.GetAll().Include(x => x.AppUser).ToList();

                answer.ApplicantInformations = answer.ApplicantInformations.Where(x => x.AppUser.InvitingUserName == inviterUserName).ToList();

                answer.Success = true;
                answer.Message = "Başarılı";
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

        public async Task<ApplicantInformationServiceAnswer> UpdateApplicantInformation(ApplicantInformationResponseVM vm, string? inviterUserName)
        {
            ApplicantInformationServiceAnswer answer = new();
            try
            {
                answer.ApplicantInformation = await _unitOfWork.AplicantInformationRead.GetByIdAsync(vm.Id);


                if (inviterUserName is not null)
                {
                    answer.ApplicantInformation.AppUser = await _unitOfWork.UserManager.FindByIdAsync(answer.ApplicantInformation.AppUserId);
                    answer.ApplicantInformation.AppUser.InvitingUserName = inviterUserName;
                }

                answer.ApplicantInformation.ApprovalDate = DateTime.Now;
                answer.ApplicantInformation.ApprovalStatus = vm.ApprovalStatus;
                answer.ApplicantInformation.Message = vm.Message;

                var result = _unitOfWork.AplicantInformationWrite.Update(answer.ApplicantInformation);
                if (result)
                {
                    await _unitOfWork.AplicantInformationWrite.SaveAsync();
                    answer.Success = true;
                    answer.Message = "Başarılı";
                    return answer;
                }

                answer.Success = false;
                answer.Message = "Bir hata oluştu";
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
