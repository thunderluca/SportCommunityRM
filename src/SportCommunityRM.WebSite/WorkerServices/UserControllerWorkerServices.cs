using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SportCommunityRM.Data;
using SportCommunityRM.Data.ReadModel;
using SportCommunityRM.WebSite.Models;
using SportCommunityRM.WebSite.ViewModels.User;
using SportCommunityRM.WebSite.Services;
using System.Threading.Tasks;
using SportCommunityRM.Data.Models;
using SportCommunityRM.WebSite.Controllers;
using SportCommunityRM.WebSite.ViewModels.Shared;

namespace SportCommunityRM.WebSite.WorkerServices
{
    public class UserControllerWorkerServices : BaseControllerWorkerServices
    {
        public UserControllerWorkerServices(
            UserManager<ApplicationUser> userManager, 
            SCRMContext dbContext, 
            IDatabase database, 
            IHttpContextAccessor httpContextAccessor, 
            IUrlService urlService,
            IStorageService storageService,
            ILogger<UserControllerWorkerServices> logger) : base(userManager, dbContext, database, httpContextAccessor, urlService, storageService, logger)
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

            var calendarViewModel = this.GetCalendarViewModel(nameof(IActivityController.GetCalendarEventsAsync), "User");

            return new IndexViewModel
            {
                Teams = teams,
                Activities = activitiesViewModel,
                NewsFeed = newsFeedViewModel,
                Calendar = calendarViewModel
            };
        }

        public async Task<DetailViewModel> GetDetailViewModelAsync(Guid registeredUserId)
        {
            var user = await this.GetApplicationUserAsync();

            var model = (from registeredUser in this.Database.RegisteredUsers
                         where registeredUser.Id == registeredUserId
                         let teams = registeredUser.Teams
                            .Select(rut => new DetailViewModel.Team
                            {
                                Id = rut.TeamId,
                                Name = rut.Team.Name
                            })
                         select new DetailViewModel
                         {
                             Id = registeredUserId,
                             FirstName = registeredUser.FirstName,
                             LastName = registeredUser.LastName,
                             BirthDate = registeredUser.BirthDate,
                             PictureId = registeredUser.PictureId,
                             BackgroundPictureId = registeredUser.BackgroundPictureId,
                             Teams = teams
                         }).SingleOrDefault();

            return model;
        }

        public async Task<byte[]> GetUserPictureAsync(string username, int? size = null)
        {
            if (string.IsNullOrWhiteSpace(username))
                return null;

            var user = await this.GetApplicationUserAsync(username);

            var registeredUser = this.Database.RegisteredUsers.WithUserId(user.Id);
            if (registeredUser == null || string.IsNullOrWhiteSpace(registeredUser.PictureId))
                return null;

            return await this.GetPictureAsync(registeredUser.PictureId);
        }
    }
}
