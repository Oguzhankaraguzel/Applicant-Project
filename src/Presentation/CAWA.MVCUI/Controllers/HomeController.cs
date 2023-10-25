using CAWA.Application.Absractions.Services;
using CAWA.Application.Absractions.Token;
using CAWA.Application.Services;
using CAWA.Application.ViewModels;
using CAWA.Application.ViewModels.ApplicantInformationVM;
using CAWA.Application.ViewModels.AppUserVM;
using CAWA.MVCUI.Filters;
using CAWA.MVCUI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CAWA.MVCUI.Controllers
{
    [TypeFilter(typeof(InviterNameSaveFilter))]
    public class HomeController : Controller
    {
        #region ctor and required instance
        private readonly ICawaUserService _cawaUserService;
        private readonly ITokenHandler _tokenHandler;
        private readonly IApplicantInformationServices _applicantInformationServices;
        public HomeController(ICawaUserService cawaUserService, ITokenHandler tokenHandler, IApplicantInformationServices applicantInformationServices)
        {
            _cawaUserService = cawaUserService;
            _tokenHandler = tokenHandler;
            _applicantInformationServices = applicantInformationServices;
        }
        #endregion

        #region actions
        public async Task<IActionResult> Index(string? inviterName)
        {
            if (!string.IsNullOrEmpty(inviterName))
                HttpContext.Session.SetString("inviterName", inviterName);

            HomePageVM vm = new();

            if (User.Identity.Name is not null)
            {
                var result = await _cawaUserService.GetUserWithAllDetails(User.Identity.Name);
                vm.UserName = result.user.Name;
                vm.ExistingUser = true;
                if (result.user.ApplicantInformation != null)
                {
                    vm.FilesUploaded = true;
                }
            }
            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> CreateApplicant()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateApplicant(AppUserCreateVM vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var result = await _cawaUserService.CreateUser(vm, HttpContext.Session.GetString("inviterName"));
            if (result.Success && result.Message == "Böyle bir kullanıcı bulunmaktadır!\n")
            {
                var token = _tokenHandler.CreateAccessToken(7, result.user);
                HttpContext.Session.SetString("JWToken", token.AccessToken);
                await _cawaUserService.Login(result.user.UserName);
                result = await _cawaUserService.GetUserWithAllDetails(result.user.UserName);
                if (result.user.ApplicantInformation is not null)
                {
                    return RedirectToAction("ApplicantResult");
                }
                else
                {
                    TempData["ExistingUserName"] = result.user.Name;
                    return RedirectToAction("CreateReference");
                }
            }

            else if (result.Success)
            {
                var token = _tokenHandler.CreateAccessToken(7, result.user);
                HttpContext.Session.SetString("JWToken", token.AccessToken);
                await _cawaUserService.Login(result.user.UserName);
                await _cawaUserService.Login(result.user.UserName);
                return RedirectToAction("CreateReference");
            }
            else
            {
                ViewBag.Message = result.Message + " " + result.ExceptionMessage + " " + "Var olan telefon numarası";
                return View(vm);
            }
        }

        [HttpGet]
        public async Task<IActionResult> CreateReference()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateReference(ApplicantInformationCreateVM vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var result = await _cawaUserService.CreateReference(vm, User.Identity.Name);
            if (result.Success)
            {
                return RedirectToAction("ApplicantResult");
            }
            else
            {
                ViewBag.ReferenceMessage = result.Message + " " + result.ExceptionMessage;
                return View(vm);
            }
        }

        public async Task<IActionResult> ApplicantResult()
        {
            if (User.Identity.Name is null) return RedirectToAction("ApplicantLogin", "Account");

            var result = await _cawaUserService.GetUserWithAllDetails(User.Identity.Name);
            if (result.Success && (result.user is not null))
            {
                ApplicantInformationResultVM vm = (ApplicantInformationResultVM)result.user.ApplicantInformation;
                ViewBag.Id = vm.Id;
                ViewBag.ApprovalStatus = vm.ApprovalStatus;
                return View(vm);
            }
            return RedirectToAction("ApplicantLogin", "Account");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        #endregion

        #region Methods

        public async Task<IActionResult> CheckUserStatus(string Id)
        {
            var result = await _applicantInformationServices.GetApplicantInformation(Id, false);
            if (result.ApplicantInformation.ApprovalStatus == CAWA.Domain.Enums.ApprovalStatus.Sent)
            {
                return BadRequest();
            }
            return Ok();
        }
        #endregion
    }
}