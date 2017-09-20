using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using SportCommunityRM.Data.ReadModel;
using SportCommunityRM.WebSite.Models;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SportCommunityRM.WebSite.WorkerServices
{
    public abstract class BaseControllerWorkerServices
    {
        public readonly IDatabase Database;
        public readonly UserManager<ApplicationUser> UserManager;
        public readonly IHttpContextAccessor HttpContextAccessor;

        public BaseControllerWorkerServices(
            UserManager<ApplicationUser> userManager,
            IDatabase database,
            IHttpContextAccessor httpContextAccessor)
        {
            this.UserManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            this.Database = database ?? throw new ArgumentNullException(nameof(database));
            this.HttpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public async Task<ApplicationUser> GetApplicationUserAsync()
        {
            var httpContextUser = this.HttpContextAccessor.HttpContext.User;
            var user = await UserManager.GetUserAsync(httpContextUser);
            if (user == null)
                throw new ApplicationException($"Unable to load user with ID '{httpContextUser.GetUserId()}'.");

            return user;
        }
    }
}
