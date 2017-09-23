using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SportCommunityRM.Data.Models;
using SportCommunityRM.Data.ReadModel;
using SportCommunityRM.WebSite.Models;
using System;
using System.Security.Claims;

namespace SportCommunityRM.WebSite.Components
{
    public class LoginViewComponent : ViewComponent
    {
        private readonly UserManager<ApplicationUser> UserManager;
        private readonly SignInManager<ApplicationUser> SignInManager;
        private readonly IDatabase Database;

        public LoginViewComponent(
            UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager,
            IDatabase database)
        {
            this.UserManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            this.SignInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            this.Database = database ?? throw new ArgumentNullException(nameof(database));
        }

        public IViewComponentResult Invoke()
        {
            var userId = this.UserClaimsPrincipal.GetUserId();

            var registeredUser = this.Database.RegisteredUsers.WithUserId(userId);
            if (registeredUser == null)
                return View(new Model());

            var username = UserManager.GetUserName(this.UserClaimsPrincipal);

            var isLogged = SignInManager.IsSignedIn(this.UserClaimsPrincipal);

            var model = new Model
            {
                RegisteredUserId = registeredUser.Id,
                Username = username,
                FirstName = registeredUser.FirstName,
                LastName = registeredUser.LastName,
                IsLogged = isLogged
            };

            return View(model);
        }

        public class Model
        {
            public Guid RegisteredUserId { get; set; }

            public string Username { get; set; }

            public string FirstName { get; set; }

            public string LastName { get; set; }

            public bool IsLogged { get; set; }

            public string[] Roles { get; set; }
        }
    }
}
