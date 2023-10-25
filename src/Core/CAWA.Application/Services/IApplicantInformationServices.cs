using CAWA.Application.MethodAnswers;
using CAWA.Application.ViewModels.ApplicantInformationVM;

namespace CAWA.Application.Services
{
    public interface IApplicantInformationServices
    {
        public Task<ApplicantInformationServiceAnswer> GetAllApplicantInformation(bool withUser = true, bool onlyUninvited = false);
        public Task<ApplicantInformationServiceAnswer> GetApplicantInformation(string Id, bool withUser = true);
        public Task<ApplicantInformationServiceAnswer> GetApplicantInformationWithInviterFilter(string? inviterUserName);
        public Task<ApplicantInformationServiceAnswer> UpdateApplicantInformation(ApplicantInformationResponseVM vm, string? inviterUserName);
        public Task<ApplicantInformationServiceAnswer> DeleteApplicantInformation(string Id, string? inviterUserName);
    }
}
