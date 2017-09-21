using SportCommunityRM.Data.ReadModel;
using SportCommunityRM.WebSite.ViewModels.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using SportCommunityRM.WebSite.Models;
using Microsoft.Extensions.Logging;
using SportCommunityRM.Data;

namespace SportCommunityRM.WebSite.WorkerServices
{
    public class HomeControllerWorkerServices : BaseControllerWorkerServices
    {
        public HomeControllerWorkerServices(
            UserManager<ApplicationUser> userManager, 
            SCRMContext dbContext, 
            IDatabase database, 
            IHttpContextAccessor httpContextAccessor, 
            ILogger<HomeControllerWorkerServices> logger) : base(userManager, dbContext, database, httpContextAccessor, logger)
        {
        }

        public async Task<IndexViewModel> GetIndexViewModelAsync()
        {
            var user = await this.GetApplicationUserAsync();

            var teams = (from registeredUser in this.Database.RegisteredUsers
                         where registeredUser.AspNetUserId == user.Id
                         from rut in registeredUser.Teams
                         select new IndexViewModel.Team
                         {
                             Id = rut.Team.Id,
                             Name = rut.Team.Name
                         }).ToArray();

            return new IndexViewModel
            {
                RegisteredUserTeams = teams
            };
        }
    }
}
