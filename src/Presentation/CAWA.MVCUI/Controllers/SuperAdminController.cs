using CAWA.Application.Absractions.Services;
using CAWA.Application.ViewModels.AdminVM;
using CAWA.MVCUI.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CAWA.MVCUI.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    [TypeFilter(typeof(AdminInformationFilter))]
    public class SuperAdminController : Controller
    {
        private readonly ICawaUserService _cawaUserService;
        public SuperAdminController(ICawaUserService cawaUserService)
        {
            _cawaUserService = cawaUserService;
        }
        [HttpGet]
        public IActionResult CreateAdmin()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateAdmin(AdminCreateVM vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var answer = await _cawaUserService.CreateAdmin(vm);

            if (answer.Success) return RedirectToAction("AdminList");

            List<string> errorMessages = new List<string>();

            if (!string.IsNullOrEmpty(answer.ExceptionMessage))
            {
                var exceptionErrorMessages = answer.ExceptionMessage.Split(". ", StringSplitOptions.RemoveEmptyEntries);
                errorMessages.AddRange(exceptionErrorMessages);
            }

            if (!string.IsNullOrEmpty(answer.Message)) errorMessages.Add(answer.Message);

            ViewBag.ErrorMessages = errorMessages;

            return View(vm);
        }
        public async Task<IActionResult> AdminList()
        {
            var result = await _cawaUserService.GetAllAdmin();
            List<AdminListVM> vm = result.users.Select(x => (AdminListVM)x).ToList();
            return View(vm);
        }
    }
}
