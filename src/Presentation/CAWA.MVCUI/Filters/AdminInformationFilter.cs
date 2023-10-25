using CAWA.Application.Absractions.Services;
using CAWA.Application.ViewModels.AdminVM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CAWA.MVCUI.Filters
{
    public class AdminInformationFilter : IAsyncActionFilter
    {
        private readonly ICawaUserService _cawaUserService;

        public AdminInformationFilter(ICawaUserService cawaUserService)
        {
            _cawaUserService = cawaUserService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var controller = context.Controller as Controller;
            if (controller != null)
            {
                var answer = await _cawaUserService.GetUser(controller.User.Identity.Name);
                controller.ViewBag.Admin = (AdminLayoutVM)answer.user;
            }
            await next();
        }
    }
}
