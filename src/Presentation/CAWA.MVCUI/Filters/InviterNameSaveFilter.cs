using CAWA.Application.Absractions.Services;
using CAWA.Application.Absractions.Token;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CAWA.MVCUI.Filters
{
    public class InviterNameSaveFilter : IAsyncActionFilter
    {
        private readonly ICawaUserService _cawaUserService;
        private readonly ITokenHandler _tokenHandler;

        public InviterNameSaveFilter(ICawaUserService cawaUserService, ITokenHandler tokenHandler)
        {
            _cawaUserService = cawaUserService;
            _tokenHandler = tokenHandler;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var controller = context.Controller as Controller;
            if (controller != null)
            {
                string? userName = controller.User.Identity.Name;
                if (userName != null)
                {
                    var result = await _cawaUserService.GetUser(userName);
                    if (result.Success && result.user.UserRank == Domain.Enums.UserRank.Applicant && result.user.InviterName == null)
                    {
                        string? invitingUserName = controller.HttpContext.Session.GetString("inviterName");
                        if (invitingUserName is not null)
                        {
                            await _cawaUserService.UpdateUserInviter(userName, invitingUserName);
                            controller.HttpContext.Session.Remove("inviterName");
                        }

                    }
                }
            }
            await next();
        }
    }
}
