using CAWA.Application.Absractions.Services;
using CAWA.Application.MethodAnswers;
using CAWA.Application.Services;
using CAWA.Application.ViewModels.AdminVM;
using CAWA.Application.ViewModels.ApplicantInformationVM;
using CAWA.MVCUI.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CAWA.MVCUI.Controllers
{
    [Authorize(Roles = "Admin, SuperAdmin")]
    [TypeFilter(typeof(AdminInformationFilter))]
    public class AdminController : Controller
    {
        #region ctor and required instance
        private readonly ICawaUserService _cawaUserService;
        private readonly IApplicantInformationServices _applicantInformationServices;
        private string? _inviterName;

        public AdminController(ICawaUserService cawaUserService, IApplicantInformationServices applicantInformationServices)
        {
            _cawaUserService = cawaUserService;
            _applicantInformationServices = applicantInformationServices;
        }
        #endregion

        #region actions
        public async Task<IActionResult> Index()
        {
            var answer = await _cawaUserService.GetUser(User.Identity.Name);
            var user = (AdminListVM)answer.user;
            return View(user);
        }

        public async Task<IActionResult> DeniedApplicantList()
        {
            await SetInviterName();
            var answer = await _applicantInformationServices.GetApplicantInformationWithInviterFilter(_inviterName);
            List<ApplicantInformationListVM> vm = answer.ApplicantInformations.Where(x => x.ApprovalStatus == Domain.Enums.ApprovalStatus.Denied).Select(x => (ApplicantInformationListVM)x).ToList();
            SetErrosMessages(answer);
            return View(vm);
        }

        public async Task<IActionResult> ApprovedApplicantList()
        {
            await SetInviterName();
            var answer = await _applicantInformationServices.GetApplicantInformationWithInviterFilter(_inviterName);
            List<ApplicantInformationListVM> vm = answer.ApplicantInformations.Where(x => x.ApprovalStatus == Domain.Enums.ApprovalStatus.Approved).Select(x => (ApplicantInformationListVM)x).ToList();
            SetErrosMessages(answer);
            return View(vm);
        }

        public async Task<IActionResult> PendingApplicantList()
        {
            await SetInviterName();
            var answer = await _applicantInformationServices.GetApplicantInformationWithInviterFilter(_inviterName);
            List<ApplicantInformationListVM> vm = answer.ApplicantInformations.Where(x => x.ApprovalStatus == Domain.Enums.ApprovalStatus.Sent).Select(x => (ApplicantInformationListVM)x).ToList();

            answer.Message = answer.Message == "Başarılı" ? null : answer.Message;
            SetErrosMessages(answer);
            return View(vm);
        }

        public async Task<IActionResult> AllApplicantList()
        {
            await SetInviterName();
            var answer = await _applicantInformationServices.GetApplicantInformationWithInviterFilter(_inviterName);
            List<ApplicantInformationListVM> vm = answer.ApplicantInformations.Select(x => (ApplicantInformationListVM)x).ToList();
            SetErrosMessages(answer);
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> ReplyToApplicant(ApplicantInformationResponseVM vm)
        {
            ApplicantInformationServiceAnswer answer = new();

            if (!ModelState.IsValid)
            {
                answer.Message = string.Join(". ", ModelState.Values
                                       .SelectMany(v => v.Errors)
                                       .Select(e => e.ErrorMessage));
                SetErrosMessages(answer);
                return RedirectToAction("GetDetail");
            }

            answer = await _applicantInformationServices.UpdateApplicantInformation(vm, _inviterName);

            SetErrosMessages(answer);
            return RedirectToAction("PendingApplicantList");

        }

        public async Task<IActionResult> GetDetail(string Id)
        {
            var result = await _applicantInformationServices.GetApplicantInformation(Id);
            ApplicantInformationListVM vm = (ApplicantInformationListVM)result.ApplicantInformation;
            return View(vm);
        }
        #endregion

        #region Methods
        private async Task SetInviterName()
        {
            var answer = await _cawaUserService.GetUser(User.Identity.Name);
            _inviterName = answer.user.InviterName;
        }
        private void SetErrosMessages(ApplicantInformationServiceAnswer answer)
        {
            List<string> errorMessages = new List<string>();

            if (!string.IsNullOrEmpty(answer.ExceptionMessage))
            {
                var exceptionErrorMessages = answer.ExceptionMessage.Split(". ", StringSplitOptions.RemoveEmptyEntries);
                errorMessages.AddRange(exceptionErrorMessages);
            }

            if (!string.IsNullOrEmpty(answer.Message)) errorMessages.Add(answer.Message);

            ViewBag.ErrorMessages = errorMessages;
        }
        #endregion
    }
}
