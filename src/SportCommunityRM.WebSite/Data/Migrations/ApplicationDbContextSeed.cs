using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using SportCommunityRM.Data;
using SportCommunityRM.Data.Models;
using SportCommunityRM.Data.ReadModel;
using SportCommunityRM.WebSite.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using static SportCommunityRM.WebSite.Models.ClaimPoliciesConstants;

namespace SportCommunityRM.WebSite.Data.Migrations
{
    public static class ApplicationDbContextSeed
    {
        public static async Task InitializeAdminsAsync(IServiceScope serviceScope)
        {
            var userManager = serviceScope.ServiceProvider.GetService<UserManager<ApplicationUser>>();
            var dbContext = serviceScope.ServiceProvider.GetService<SCRMContext>();

            var adminUser = await userManager.FindByNameAsync("Administrator");
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = "Administrator",
                    Email = "administrator@scrm.org",
                    EmailConfirmed = true,
                    LockoutEnabled = false,
                    SecurityStamp = Guid.NewGuid().ToString()
                };

                await userManager.CreateAsync(adminUser, "ScRm2@17");
            }

            var claims = await userManager.GetClaimsAsync(adminUser);

            var newClaims = AdministratorClaims.Where(c => claims.All(uc => uc.Type != c.Type));
            if (!newClaims.IsNullOrEmpty())
                await userManager.AddClaimsAsync(adminUser, newClaims);

            var registeredUser = dbContext.RegisteredUsers.WithUserId(adminUser.Id);
            if (registeredUser != null) return;

            registeredUser = new RegisteredUser
            {
                AspNetUserId = adminUser.Id,
                FirstName = "SCRM",
                LastName = "Administrator",
                BirthDate = DateTime.Now
            };

            dbContext.RegisteredUsers.Add(registeredUser);
            await dbContext.SaveChangesAsync();
        }
    }
}
