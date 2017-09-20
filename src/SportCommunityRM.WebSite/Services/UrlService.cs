using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using SportCommunityRM.WebSite.Models;
using System;
using System.Threading.Tasks;

namespace SportCommunityRM.WebSite.Services
{
    public class UrlService : IUrlService
    {
        private readonly UserManager<ApplicationUser> UserManager;
        private readonly IUrlHelperFactory UrlHelperFactory;
        private readonly IActionContextAccessor ActionContextAccessor;

        public UrlService(
            UserManager<ApplicationUser> userManager,
            IUrlHelperFactory urlHelperFactory,
            IActionContextAccessor actionContextAccessor)
        {
            this.UserManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            this.UrlHelperFactory = urlHelperFactory ?? throw new ArgumentNullException(nameof(urlHelperFactory));
            this.ActionContextAccessor = actionContextAccessor ?? throw new ArgumentNullException(nameof(actionContextAccessor));
        }

        public async Task<string> GenerateEmailConfirmationLinkAsync(ApplicationUser user)
        {
            var code = await UserManager.GenerateEmailConfirmationTokenAsync(user);
            var urlHelper = UrlHelperFactory.GetUrlHelper(ActionContextAccessor.ActionContext);
            var scheme = ActionContextAccessor.ActionContext.HttpContext.Request.Scheme;
            var callbackUrl = urlHelper.EmailConfirmationLink(user.Id, code, scheme);

            return callbackUrl;
        }

        public async Task<string> GenerateForgotPasswordLinkAsync(ApplicationUser user)
        {
            var code = await UserManager.GeneratePasswordResetTokenAsync(user);
            var scheme = ActionContextAccessor.ActionContext.HttpContext.Request.Scheme;
            var urlHelper = UrlHelperFactory.GetUrlHelper(ActionContextAccessor.ActionContext);
            var callbackUrl = urlHelper.ResetPasswordCallbackLink(user.Id, code, scheme);

            return callbackUrl;
        }

        public string GetActionUrl(string actionName)
        {
            var urlHelper = UrlHelperFactory.GetUrlHelper(ActionContextAccessor.ActionContext);
            return urlHelper.Action(actionName);
        }
    }
}
