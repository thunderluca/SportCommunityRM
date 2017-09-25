using SportCommunityRM.Data.ReadModel;
using SportCommunityRM.WebSite.ViewModels.Home;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using SportCommunityRM.WebSite.Models;
using Microsoft.Extensions.Logging;
using SportCommunityRM.Data;
using SportCommunityRM.WebSite.ViewModels.Shared;
using SportCommunityRM.WebSite.Services;
using SportCommunityRM.WebSite.Controllers;

namespace SportCommunityRM.WebSite.WorkerServices
{
    public class HomeControllerWorkerServices : BaseControllerWorkerServices
    {
        public HomeControllerWorkerServices(
            UserManager<ApplicationUser> userManager, 
            SCRMContext dbContext, 
            IDatabase database, 
            IHttpContextAccessor httpContextAccessor, 
            IUrlService urlService,
            ILogger<HomeControllerWorkerServices> logger) : base(userManager, dbContext, database, httpContextAccessor, urlService, logger)
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

            var activities = await this.GetActivitiesAsync(user.Id);

            var activitiesViewModel = new ActivitiesViewModel { Activities = activities };

            var newsFeedContents = await this.GetFeedAsync(user.Id);

            var newsFeedViewModel = new NewsFeedViewModel { NewsFeed = newsFeedContents };

            var calendarViewModel = this.GetCalendarViewModel(nameof(IActivityController.GetCalendarEventsAsync), "Home");

            return new IndexViewModel
            {
                Teams = teams,
                Activities = activitiesViewModel,
                NewsFeed = newsFeedViewModel,
                Calendar = calendarViewModel
            };
        }
    }
}
