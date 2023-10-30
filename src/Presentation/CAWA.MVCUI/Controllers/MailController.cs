using CAWA.Application.Absractions.Services;
using CAWA.Application.ViewModels.MailVM;
using CAWA.MVCUI.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CAWA.MVCUI.Controllers
{
    [TypeFilter(typeof(AdminInformationFilter))]
    [Authorize(Roles = "Admin, SuperAdmin")]
    public class MailController : Controller
    {
        private readonly IMailService _mailService;
        public MailController(IMailService mailService)
        {
            _mailService = mailService;
        }

        [HttpGet]
        public async Task<IActionResult> SendMail()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendMail(SendMailVM vm)
        {
            if (!ModelState.IsValid) return View(vm);

            try
            {
                await _mailService.SendMailAsync(vm.Email, vm.Subject, vm.Body, vm.IsHtml);
                TempData["MailMessage"] = "Mail gönderildi";
                return View();
            }
            catch (Exception ex)
            {
                TempData["MailMessage"] = ex.Message;
                return View(vm);
            }
        }

        [HttpGet]
        public async Task<IActionResult> SendInvitationMail()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendInvitationMail(InvitationMailVM vm)
        {
            if (!ModelState.IsValid) return View(vm);

            if (string.IsNullOrEmpty(vm.Subject))
            {
                vm.Subject = "Bilgilendirme";
            }

            return await SendMail((SendMailVM)vm);
        }
    }
}
