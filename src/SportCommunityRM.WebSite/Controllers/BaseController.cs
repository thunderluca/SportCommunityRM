using System;
using Microsoft.AspNetCore.Mvc;
using SportCommunityRM.WebSite.ViewModels.Shared;
using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using System.Text;
using System.Text.Encodings.Web;

namespace SportCommunityRM.WebSite.Controllers
{
    public abstract class BaseController : Controller
    {
        private readonly UrlEncoder UrlEncoder;

        public BaseController(UrlEncoder urlEncoder)
        {
            this.UrlEncoder = urlEncoder ?? throw new ArgumentNullException(nameof(urlEncoder));
        }

        private const string AuthenicatorUriFormat = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        internal void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        internal IActionResult RedirectToLocal(string returnUrl, string actionName, string controllerName)
        {
            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction(actionName, controllerName);
        }

        internal string FormatKey(string unformattedKey)
        {
            var result = new StringBuilder();
            int currentPosition = 0;
            while (currentPosition + 4 < unformattedKey.Length)
            {
                result.Append(unformattedKey.Substring(currentPosition, 4)).Append(" ");
                currentPosition += 4;
            }
            if (currentPosition < unformattedKey.Length)
            {
                result.Append(unformattedKey.Substring(currentPosition));
            }

            return result.ToString().ToLowerInvariant();
        }

        internal string GenerateQrCodeUri(string email, string unformattedKey)
        {
            return string.Format(
                AuthenicatorUriFormat,
                UrlEncoder.Encode($"{nameof(SportCommunityRM)}.{nameof(WebSite)}"),
                UrlEncoder.Encode(email),
                unformattedKey);
        }
    }
}