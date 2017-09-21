using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SportCommunityRM.Data;
using SportCommunityRM.Data.Models;
using SportCommunityRM.Data.ReadModel;
using SportCommunityRM.WebSite.Controllers;
using SportCommunityRM.WebSite.Models;
using SportCommunityRM.WebSite.Services;
using SportCommunityRM.WebSite.ViewModels.Account;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SportCommunityRM.WebSite.WorkerServices
{
    public class AccountControllerWorkerServices : BaseControllerWorkerServices
    {
        private readonly SignInManager<ApplicationUser> SignInManager;
        private readonly IEmailSender EmailSender;
        private readonly IUrlService UrlService;

        public AccountControllerWorkerServices(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            SCRMContext dbContext, 
            IDatabase database,
            IHttpContextAccessor httpContextAccessor,
            IEmailSender emailSender,
            IUrlService urlService,
            ILogger<AccountControllerWorkerServices> logger) : base(userManager, dbContext, database, httpContextAccessor, logger)
        {
            this.SignInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            this.EmailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
            this.UrlService = urlService ?? throw new ArgumentNullException(nameof(urlService));
        }

        public async Task<IdentityResult> RegisterUserAsync(RegisterViewModel viewModel)
        {
            var user = new ApplicationUser { UserName = viewModel.Username, Email = viewModel.Email };

            var result = await UserManager.CreateAsync(user, viewModel.Password);
            if (!result.Succeeded)
                return result;

            user = await this.FindApplicationUserByEmail(viewModel.Email, checkIfConfirmed: false);

            try
            {
                var registeredUser = new RegisteredUser
                {
                    AspNetUserId = user.Id,
                    FirstName = viewModel.FirstName,
                    LastName = viewModel.LastName,
                    Sex = MapSex(viewModel.SelectedSex),
                    BirthDate = viewModel.BirthDate
                };

                this.DbContext.RegisteredUsers.Add(registeredUser);
                await this.DbContext.SaveChangesAsync();
            }
            catch (Exception exception)
            {
                Logger.LogError(exception, "User creation failed.");
                throw exception;
            }

            Logger.LogInformation("User created a new account with password.");

            var callbackUrl = await this.UrlService.GenerateEmailConfirmationLinkAsync(user);
            await EmailSender.SendEmailConfirmationAsync(viewModel.Email, callbackUrl);

            await SignInManager.SignInAsync(user, isPersistent: false);
            Logger.LogInformation("User created a new account with password.");

            return result;
        }

        private static Sex MapSex(RegisterViewModel.Sex sex)
        {
            switch (sex)
            {
                case RegisterViewModel.Sex.Female:
                    return Sex.Female;
                case RegisterViewModel.Sex.Male:
                    return Sex.Male;
                default:
                    return Sex.Unspecified;
            }
        }

        public async Task<string> ConfirmEmailAsync(string userId, string code)
        {
            var user = await UserManager.FindByIdAsync(userId);
            if (user == null)
                throw new ApplicationException($"Unable to load user with ID '{userId}'.");

            var result = await UserManager.ConfirmEmailAsync(user, code);
            return result.Succeeded ? nameof(AccountController.ConfirmEmail) : nameof(AccountController.Error);
        }

        public ExternalLoginViewModel GetExternalLoginViewModel(ExternalLoginInfo info)
        {
            var email = info.Principal.FindFirstValue(ClaimTypes.Email);

            return new ExternalLoginViewModel { Email = email };
        }

        public async Task<IdentityResult> ConfirmExternalLoginAsync(ExternalLoginViewModel viewModel)
        {
            var info = await SignInManager.GetExternalLoginInfoAsync();
            if (info == null)
                throw new ApplicationException("Error loading external login information during confirmation.");

            var user = new ApplicationUser { UserName = viewModel.Email, Email = viewModel.Email };

            var result = await UserManager.CreateAsync(user);
            if (!result.Succeeded) return result;

            result = await UserManager.AddLoginAsync(user, info);
            if (!result.Succeeded) return result;

            user = await this.FindApplicationUserByEmail(viewModel.Email, checkIfConfirmed: false);
            await SignInManager.SignInAsync(user, isPersistent: false);
            Logger.LogInformation("User created an account using {Name} provider.", info.LoginProvider);

            return result;
        }

        public async Task<ApplicationUser> FindApplicationUserByEmail(string email, bool checkIfConfirmed)
        {
            var user = await UserManager.FindByEmailAsync(email);
            if (user == null)
                return null;

            if (!checkIfConfirmed) return user;

            var isEmailConfirmed = await UserManager.IsEmailConfirmedAsync(user);
            return isEmailConfirmed ? user : null;
        }

        public async Task SendForgotPasswordMailAsync(ApplicationUser user)
        {
            var callbackUrl = await this.UrlService.GenerateForgotPasswordLinkAsync(user);
            await EmailSender.SendForgotPasswordAsync(user.Email, callbackUrl);
        }

        public async Task<IdentityResult> ResetPasswordAsync(ResetPasswordViewModel viewModel)
        {
            var user = await UserManager.FindByEmailAsync(viewModel.Email);
            if (user == null)
                return IdentityResult.Success;

            var result = await UserManager.ResetPasswordAsync(user, viewModel.Code, viewModel.Password);
            return result;
        }
    }
}
