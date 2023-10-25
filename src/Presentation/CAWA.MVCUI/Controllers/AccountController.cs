using CAWA.Application.Absractions.Services;
using CAWA.Application.Absractions.Token;
using CAWA.Application.Consts;
using CAWA.Application.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CAWA.MVCUI.Controllers
{
    public class AccountController : Controller
    {
        #region ctor and required instance
        private readonly ICawaUserService _cawaUserService;
        private readonly ITokenHandler _tokenHandler;
        public AccountController(ICawaUserService cawaUserService, ITokenHandler tokenHandler)
        {
            _cawaUserService = cawaUserService;
            _tokenHandler = tokenHandler;
        }
        #endregion

        #region actions
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM vm)
        {
            if (ModelState.IsValid)
            {
                var result = await _cawaUserService.Login(vm.UserName, vm.Password);
                if (result.Success)
                {
                    var token = _tokenHandler.CreateAccessToken(7, result.user);
                    HttpContext.Session.SetString("JWToken", token.AccessToken);
                    return RedirectToAction("Index", "Admin");
                }
                ViewBag.Message = (result.Message + result.ExceptionMessage).Split("!");
                return View(vm);
            }
            return View(vm);
        }

        [HttpGet]
        public IActionResult ApplicantLogin()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ApplicantLogin(EmailLoginVM vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var result = await _cawaUserService.CheckUserByEmail(vm.Email);
            if (result.Success)
            {
                if (result.user.Name.ToLower() == vm.Name.ToLower() && result.user.SirName.ToLower() == vm.SirName.ToLower() && result.user.BirthDate == vm.BirthDate)
                {
                    var token = _tokenHandler.CreateAccessToken(7, result.user);
                    HttpContext.Session.SetString("JWToken", token.AccessToken);
                    await _cawaUserService.Login(result.user.UserName);
                    return RedirectToAction("ApplicantResult", "Home");
                }
                result.Message += "Hatalı giriş yaptınız. Lütfen girdiğiniz bilgileri kontrol edin! Dilerseniz Başvuru oluşturabilirsiniz!";
            }
            ViewBag.Message = (result.Message + result.ExceptionMessage).Split("!");
            return View(vm);
        }

        public async Task<IActionResult> LogOut()
        {
            if (User.IsInRole(RoleNames.Applicant)) return RedirectToAction("AccessDenied");
            await _cawaUserService.LogOut();
            HttpContext.Session.Remove("JWToken");
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> ALO()
        {
            await _cawaUserService.LogOut();
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
        #endregion

    }
}
