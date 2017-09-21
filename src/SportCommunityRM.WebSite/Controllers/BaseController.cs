using Microsoft.AspNetCore.Mvc;
using SportCommunityRM.WebSite.ViewModels.Shared;
using System.Diagnostics;

namespace SportCommunityRM.WebSite.Controllers
{
    public abstract class BaseController : Controller
    {
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult RedirectToLocal(
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