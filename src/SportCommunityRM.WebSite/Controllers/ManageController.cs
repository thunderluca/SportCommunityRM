using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportCommunityRM.WebSite.ViewModels.Manage;
using SportCommunityRM.WebSite.WorkerServices;

namespace SportCommunityRM.WebSite.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class ManageController : BaseController
    {
        private readonly ManageControllerWorkerServices WorkerServices;

        public ManageController(ManageControllerWorkerServices workerServices)
        {
            this.WorkerServices = workerServices ?? throw new ArgumentNullException(nameof(workerServices));
        }

        [TempData]
        public string StatusMessage { get; set; }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var model = await this.WorkerServices.GetIndexViewModelAsync(this.StatusMessage);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(IndexViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            await this.WorkerServices.UpdateUserAsync(model);

            StatusMessage = "Your profile has been updated";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendVerificationEmail(IndexViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            await this.WorkerServices.SendVerificationEmailAsync();

            StatusMessage = "Verification email sent. Please check your email.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {
            var model = await this.WorkerServices.GetChangePasswordViewModelAsync(StatusMessage);
            if (model == null)
                return RedirectToAction(nameof(SetPassword));

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await this.WorkerServices.ChangeUserPasswordAsync(model);
            if (!result.Succeeded)
            {
                ModelState.AddErrors(result);
                return View(model);
            }

            StatusMessage = "Your password has been changed.";
            return RedirectToAction(nameof(ChangePassword));
        }

        [HttpGet]
        public async Task<IActionResult> SetPassword()
        {
            var model = await this.WorkerServices.GetSetPasswordViewModelAsync(StatusMessage);
            if (model == null)
                return RedirectToAction(nameof(this.ChangePassword));
            
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await this.WorkerServices.SetUserPasswordAsync(model);
            if (!result.Succeeded)
            {
                ModelState.AddErrors(result);
                return View(model);
            }

            StatusMessage = "Your password has been set.";
            return RedirectToAction(nameof(SetPassword));
        }

        [HttpGet]
        public async Task<IActionResult> ExternalLogins()
        {
            var model = await this.WorkerServices.GetExternalLoginsViewModelAsync(StatusMessage);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LinkLogin(string provider)
        {
            var properties = await this.WorkerServices.GetAuthenticationPropertiesAsync(provider);
            
            return new ChallengeResult(provider, properties);
        }

        [HttpGet]
        public async Task<IActionResult> LinkLoginCallback()
        {
            await this.WorkerServices.LinkLoginAsync();

            StatusMessage = "The external login was added.";
            return RedirectToAction(nameof(ExternalLogins));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveLogin(RemoveLoginViewModel model)
        {
            await this.WorkerServices.RemoveLoginAsync(model);

            StatusMessage = "The external login was removed.";
            return RedirectToAction(nameof(ExternalLogins));
        }

        [HttpGet]
        public async Task<IActionResult> TwoFactorAuthentication()
        {
            var model = await this.WorkerServices.GetTwoFactorAuthenticationViewModelAsync();

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Disable2faWarning()
        {
            var userCanDisableTwoFactorAuthentication = await this.WorkerServices.CheckIfUserCanDisableTwoFactorAuthentication();
            if (userCanDisableTwoFactorAuthentication)
                return View(nameof(Disable2fa));

            return RedirectToAction(nameof(this.TwoFactorAuthentication));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Disable2fa()
        {
            await this.WorkerServices.DisableTwoFactorAuthentication();

            return RedirectToAction(nameof(TwoFactorAuthentication));
        }

        [HttpGet]
        public async Task<IActionResult> EnableAuthenticator()
        {
            var model = await this.WorkerServices.GetEnableAuthenticatorViewModelAsync();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EnableAuthenticator(EnableAuthenticatorViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var is2faTokenValid = await this.WorkerServices.ValidateTwoFactorAuthenticationTokenAsync(model.Code);
            if (is2faTokenValid)
                return RedirectToAction(nameof(this.GenerateRecoveryCodes));

            ModelState.AddModelError("model.Code", "Verification code is invalid.");
            return View(model);
        }

        [HttpGet]
        public IActionResult ResetAuthenticatorWarning()
        {
            return View(nameof(ResetAuthenticator));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetAuthenticator()
        {
            await this.WorkerServices.ResetAuthenticatorAsync();

            return RedirectToAction(nameof(EnableAuthenticator));
        }

        [HttpGet]
        public async Task<IActionResult> GenerateRecoveryCodes()
        {
            var model = await this.WorkerServices.GetGenerateRecoveryCodesViewModelAsync();

            return View(model);
        }
    }
}
