using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SportCommunityRM.Data;
using SportCommunityRM.Data.Models;
using SportCommunityRM.Data.ReadModel;
using SportCommunityRM.WebSite.Controllers;
using SportCommunityRM.WebSite.Helpers;
using SportCommunityRM.WebSite.Models;
using SportCommunityRM.WebSite.Services;
using SportCommunityRM.WebSite.ViewModels.Manage;
using System;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace SportCommunityRM.WebSite.WorkerServices
{
    public class ManageControllerWorkerServices : BaseControllerWorkerServices
    {
        private readonly SignInManager<ApplicationUser> SignInManager;
        private readonly UrlEncoder UrlEncoder;
        private readonly IEmailSender EmailSender;

        private const string AuthenicatorUriFormat = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";

        public ManageControllerWorkerServices(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            SCRMContext dbContext,
            IDatabase database,
            IHttpContextAccessor httpContextAccessor,
            IUrlService urlService,
            UrlEncoder urlEncoder,
            IEmailSender emailSender,
            IStorageService storageService,
            ILogger<ManageControllerWorkerServices> logger) : base(userManager, dbContext, database, httpContextAccessor, urlService, storageService, logger)
        {
            this.SignInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            this.UrlEncoder = urlEncoder ?? throw new ArgumentNullException(nameof(urlEncoder));
            this.EmailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
        }

        public async Task<IndexViewModel> GetIndexViewModelAsync(string statusMessage)
        {
            var user = await this.GetApplicationUserAsync();

            var registeredUser = this.Database.RegisteredUsers.WithUserId(user.Id);

            var pictureUrl = this.UrlService.GetActionUrl(nameof(UserController.UserPicture), "User", new { username = user.UserName });
            
            return new IndexViewModel
            {
                Username = user.UserName,
                Email = user.Email,
                PictureUrl = pictureUrl,
                PhoneNumber = user.PhoneNumber,
                FirstName = registeredUser.FirstName,
                LastName = registeredUser.LastName,
                BirthDate = registeredUser.BirthDate.ToShortDateString(),
                IsEmailConfirmed = user.EmailConfirmed,
                StatusMessage = statusMessage
            };
        }

        public async Task UpdateUserAsync(IndexViewModel viewModel)
        {
            var user = await this.GetApplicationUserAsync();

            var email = user.Email;
            if (viewModel.Email != email)
            {
                var setEmailResult = await UserManager.SetEmailAsync(user, viewModel.Email);
                if (!setEmailResult.Succeeded)
                    throw new ApplicationException($"Unexpected error occurred setting email for user with ID '{user.Id}'.");
            }

            var phoneNumber = user.PhoneNumber;
            if (viewModel.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await UserManager.SetPhoneNumberAsync(user, viewModel.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                    throw new ApplicationException($"Unexpected error occurred setting phone number for user with ID '{user.Id}'.");
            }
        }

        public async Task SendVerificationEmailAsync()
        {
            var user = await this.GetApplicationUserAsync();

            var callbackUrl = await this.UrlService.GenerateEmailConfirmationLinkAsync(user);

            await EmailSender.SendEmailConfirmationAsync(user.Email, callbackUrl);
        }

        public async Task<bool> StoreProfilePictureAsync(string base64Image)
        {
            var user = await this.GetApplicationUserAsync();

            try
            {
                var bytes = ImagesHelper.GetImageBytesFromBase64String(base64Image);
                if (bytes.IsNullOrEmpty()) return false;
 
                var fileId = Guid.NewGuid().ToString("N");

                var filePath = await this.StorageService.StoreFileAsync(fileId, bytes);
                if (string.IsNullOrWhiteSpace(filePath))
                    return false;

                var registeredUser = this.DbContext.RegisteredUsers.WithUserId(user.Id);
                var oldFileId = registeredUser.PictureId;
                registeredUser.PictureId = fileId;
                this.DbContext.SaveChanges();

                if (!string.IsNullOrWhiteSpace(oldFileId))
                    this.StorageService.DeleteFile(oldFileId);

                return true;
            }
            catch (Exception exception)
            {
                Logger.LogError(exception, exception.Message);
                return false;
            }
        }

        public async Task<ChangePasswordViewModel> GetChangePasswordViewModelAsync(string statusMessage)
        {
            var user = await this.GetApplicationUserAsync();

            var hasPassword = await UserManager.HasPasswordAsync(user);
            if (!hasPassword) return null;

            return new ChangePasswordViewModel { StatusMessage = statusMessage };
        }

        public async Task<IdentityResult> ChangeUserPasswordAsync(ChangePasswordViewModel viewModel)
        {
            var user = await this.GetApplicationUserAsync();

            var result = await UserManager.ChangePasswordAsync(user, viewModel.OldPassword, viewModel.NewPassword);
            if (!result.Succeeded) return result;

            await SignInManager.SignInAsync(user, isPersistent: false);
            Logger.LogInformation("User changed their password successfully.");

            return result;
        }

        public async Task<SetPasswordViewModel> GetSetPasswordViewModelAsync(string statusMessage)
        {
            var user = await this.GetApplicationUserAsync();

            var hasPassword = await UserManager.HasPasswordAsync(user);
            if (hasPassword) return null;

            return new SetPasswordViewModel { StatusMessage = statusMessage };
        }

        public async Task<IdentityResult> SetUserPasswordAsync(SetPasswordViewModel viewModel)
        {
            var user = await this.GetApplicationUserAsync();

            var result = await UserManager.AddPasswordAsync(user, viewModel.NewPassword);
            if (!result.Succeeded) return result;

            await SignInManager.SignInAsync(user, isPersistent: false);

            return result;
        }

        public async Task<ExternalLoginsViewModel> GetExternalLoginsViewModelAsync(string statusMessage)
        {
            var user = await this.GetApplicationUserAsync();

            var currentLogins = await UserManager.GetLoginsAsync(user);

            var authenticationSchemes = await SignInManager.GetExternalAuthenticationSchemesAsync();

            var otherLogins = authenticationSchemes.Where(auth => currentLogins.All(ul => auth.Name != ul.LoginProvider)).ToList();

            var showRemoveButton = await UserManager.HasPasswordAsync(user) || currentLogins.Count > 1;

            return new ExternalLoginsViewModel
            {
                CurrentLogins = currentLogins,
                OtherLogins = otherLogins,
                ShowRemoveButton = showRemoveButton,
                StatusMessage = statusMessage
            };
        }

        public async Task<AuthenticationProperties> GetAuthenticationPropertiesAsync(string provider)
        {
            var user = await this.GetApplicationUserAsync();

            await HttpContextAccessor.HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            var redirectUrl = UrlService.GetActionUrl(nameof(ManageController.LinkLoginCallback));
            var properties = SignInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl, user.Id);

            return properties;
        }

        public async Task LinkLoginAsync()
        {
            var user = await this.GetApplicationUserAsync();

            var info = await SignInManager.GetExternalLoginInfoAsync(user.Id);
            if (info == null)
                throw new ApplicationException($"Unexpected error occurred loading external login info for user with ID '{user.Id}'.");

            var result = await UserManager.AddLoginAsync(user, info);
            if (!result.Succeeded)
                throw new ApplicationException($"Unexpected error occurred adding external login for user with ID '{user.Id}'.");

            await HttpContextAccessor.HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
        }

        public async Task RemoveLoginAsync(RemoveLoginViewModel viewModel)
        {
            var user = await this.GetApplicationUserAsync();

            var result = await UserManager.RemoveLoginAsync(user, viewModel.LoginProvider, viewModel.ProviderKey);
            if (!result.Succeeded)
                throw new ApplicationException($"Unexpected error occurred removing external login for user with ID '{user.Id}'.");

            await SignInManager.SignInAsync(user, isPersistent: false);
        }

        public async Task<TwoFactorAuthenticationViewModel> GetTwoFactorAuthenticationViewModelAsync()
        {
            var user = await this.GetApplicationUserAsync();

            var hasAuthenticator = await UserManager.GetAuthenticatorKeyAsync(user) != null;

            var recoveryCodesLeft = await UserManager.CountRecoveryCodesAsync(user);

            return new TwoFactorAuthenticationViewModel
            {
                HasAuthenticator = hasAuthenticator,
                Is2faEnabled = user.TwoFactorEnabled,
                RecoveryCodesLeft = recoveryCodesLeft
            };
        }

        public async Task<bool> CheckIfUserCanDisableTwoFactorAuthentication()
        {
            var user = await this.GetApplicationUserAsync();
            if (!user.TwoFactorEnabled)
                throw new ApplicationException($"Unexpected error occured disabling 2FA for user with ID '{user.Id}'.");

            return true;
        }

        public async Task DisableTwoFactorAuthentication()
        {
            var user = await this.GetApplicationUserAsync();

            var disable2faResult = await UserManager.SetTwoFactorEnabledAsync(user, false);
            if (!disable2faResult.Succeeded)
                throw new ApplicationException($"Unexpected error occured disabling 2FA for user with ID '{user.Id}'.");

            Logger.LogInformation("User with ID {UserId} has disabled 2fa.", user.Id);
        }

        public async Task<bool> ValidateTwoFactorAuthenticationTokenAsync(string code)
        {
            var user = await this.GetApplicationUserAsync();
            
            var verificationCode = code.Replace(" ", string.Empty).Replace("-", string.Empty);

            var is2faTokenValid = await UserManager.VerifyTwoFactorTokenAsync(
                user, UserManager.Options.Tokens.AuthenticatorTokenProvider, verificationCode);

            if (is2faTokenValid)
            {
                await UserManager.SetTwoFactorEnabledAsync(user, true);
                Logger.LogInformation("User with ID {UserId} has enabled 2FA with an authenticator app.", user.Id);
            }

            return is2faTokenValid;
        }

        public async Task<EnableAuthenticatorViewModel> GetEnableAuthenticatorViewModelAsync()
        {
            var user = await this.GetApplicationUserAsync();

            var unformattedKey = await UserManager.GetAuthenticatorKeyAsync(user);
            if (string.IsNullOrEmpty(unformattedKey))
            {
                await UserManager.ResetAuthenticatorKeyAsync(user);
                unformattedKey = await UserManager.GetAuthenticatorKeyAsync(user);
            }

            return new EnableAuthenticatorViewModel
            {
                SharedKey = FormatKey(unformattedKey),
                AuthenticatorUri = GenerateQrCodeUri(user.Email, unformattedKey)
            };
        }

        public async Task ResetAuthenticatorAsync()
        {
            var user = await this.GetApplicationUserAsync();

            await UserManager.SetTwoFactorEnabledAsync(user, false);
            await UserManager.ResetAuthenticatorKeyAsync(user);
            Logger.LogInformation("User with id '{UserId}' has reset their authentication app key.", user.Id);
        }

        public async Task<GenerateRecoveryCodesViewModel> GetGenerateRecoveryCodesViewModelAsync()
        {
            var user = await this.GetApplicationUserAsync();
            if (!user.TwoFactorEnabled)
                throw new ApplicationException($"Cannot generate recovery codes for user with ID '{user.Id}' as they do not have 2FA enabled.");

            var recoveryCodes = await UserManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10);

            Logger.LogInformation("User with ID {UserId} has generated new 2FA recovery codes.", user.Id);

            return new GenerateRecoveryCodesViewModel { RecoveryCodes = recoveryCodes.ToArray() };
        }

        private string FormatKey(string unformattedKey)
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
