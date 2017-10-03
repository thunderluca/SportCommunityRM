using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SportCommunityRM.Data;
using SportCommunityRM.Data.Models;
using SportCommunityRM.Data.ReadModel;
using SportCommunityRM.WebSite.Controllers;
using SportCommunityRM.WebSite.Models;
using SportCommunityRM.WebSite.Services;
using SportCommunityRM.WebSite.ViewModels.Shared;
using SportCommunityRM.WebSite.ViewModels.User;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SportCommunityRM.WebSite.WorkerServices
{
    public class UserControllerWorkerServices : BaseControllerWorkerServices
    {
        public UserControllerWorkerServices(
            UserManager<ApplicationUser> userManager, 
            SCRMContext dbContext, 
            IDatabase database, 
            IHttpContextAccessor httpContextAccessor, 
            IHostingEnvironment hostingEnvironment,
            IUrlService urlService,
            IStorageService storageService,
            ILogger<UserControllerWorkerServices> logger) : base(userManager, dbContext, database, httpContextAccessor, hostingEnvironment, urlService, storageService, logger)
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

        public DetailViewModel GetDetailViewModel(Guid registeredUserId)
        {
            var baseQuery = this.Database.RegisteredUsers.Where(ru => ru.Id == registeredUserId);

            return GetDetailViewModel(baseQuery);
        }

        public async Task<DetailViewModel> GetDetailViewModelAsync(string username)
        {
            var user = await this.GetApplicationUserAsync(username);

            var baseQuery = this.Database.RegisteredUsers.Where(ru => ru.AspNetUserId == user.Id);

            return GetDetailViewModel(baseQuery);
        }

        private DetailViewModel GetDetailViewModel(IQueryable<RegisteredUser> baseRegisteredUserQuery)
        {
            var model = (from registeredUser in baseRegisteredUserQuery
                         let teams = registeredUser.Teams
                            .Select(rut => new DetailViewModel.Team
                            {
                                Id = rut.TeamId,
                                Name = rut.Team.Name
                            })
                         select new DetailViewModel
                         {
                             Id = registeredUser.Id,
                             FirstName = registeredUser.FirstName,
                             LastName = registeredUser.LastName,
                             BirthDate = registeredUser.BirthDate,
                             PictureId = registeredUser.PictureId,
                             BackgroundPictureId = registeredUser.BackgroundPictureId,
                             Teams = teams
                         }).SingleOrDefault();

            return model;
        }

        public async Task<byte[]> GetUserPictureByUsernameAsync(string username, int? size = null)
        {
            if (string.IsNullOrWhiteSpace(username))
                return null;

            var user = await this.GetApplicationUserAsync(username);

            var registeredUser = this.Database.RegisteredUsers.WithUserId(user.Id);
            return await this.GetUserPictureAsync(registeredUser?.PictureId, size);
        }

        public async Task<byte[]> GetUserPictureByIdAsync(Guid id, int? size = null)
        {
            if (id.IsNullOrEmpty()) return null;
            
            var registeredUser = this.Database.RegisteredUsers.WithId(id);
            return await this.GetUserPictureAsync(registeredUser?.PictureId, size);
        }

        private async Task<byte[]> GetUserPictureAsync(string pictureId, int? size = null)
        {
            var defaultStaticImagePath = this.GetDefaultStaticImagePath();
            return await this.GetPictureAsync(pictureId, defaultStaticImagePath);
        }
    }
}
