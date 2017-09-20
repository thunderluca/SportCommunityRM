using Microsoft.AspNetCore.Mvc;
using SportCommunityRM.WebSite.ViewModels.Shared;
using System.Diagnostics;
using Microsoft.AspNetCore.Identity;

namespace SportCommunityRM.WebSite.Controllers
{
    public abstract class BaseController : Controller
    {
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

        internal IActionResult RedirectToLocal(
            string returnUrl, 
            string actionName = nameof(HomeController.Index), 
            string controllerName = "Home")
        {
            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction(actionName, controllerName);
        }
    }
}