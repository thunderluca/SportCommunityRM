using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ReflectionIT.Mvc.Paging;
using SportCommunityRM.Data;
using SportCommunityRM.Data.Models;
using SportCommunityRM.Data.ReadModel;
using SportCommunityRM.WebSite.Helpers;
using SportCommunityRM.WebSite.Models;
using SportCommunityRM.WebSite.Services;
using SportCommunityRM.WebSite.ViewModels.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SportCommunityRM.WebSite.WorkerServices
{
    public abstract class BaseControllerWorkerServices
    {
        public readonly UserManager<ApplicationUser> UserManager;
        public readonly SCRMContext DbContext;
        public readonly IDatabase Database;
        public readonly IHttpContextAccessor HttpContextAccessor;
        public readonly IUrlService UrlService;
        public readonly IStorageService StorageService;
        public readonly ILogger Logger;

        public BaseControllerWorkerServices(
            UserManager<ApplicationUser> userManager,
            SCRMContext dbContext,
            IDatabase database,
            IHttpContextAccessor httpContextAccessor,
            IUrlService urlService,
            IStorageService storageService,
            ILogger<BaseControllerWorkerServices> logger)
        {
            this.UserManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            this.DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            this.Database = database ?? throw new ArgumentNullException(nameof(database));
            this.HttpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            this.UrlService = urlService ?? throw new ArgumentNullException(nameof(urlService));
            this.StorageService = storageService ?? throw new ArgumentNullException(nameof(storageService));
            this.Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public const int DefaultPageSize = 10;

        public async Task<ApplicationUser> GetApplicationUserAsync(string username = null)
        {
            var httpContextUser = this.HttpContextAccessor.HttpContext.User;

            var user = !string.IsNullOrWhiteSpace(username) 
                ? await UserManager.FindByNameAsync(username) 
                : await UserManager.GetUserAsync(httpContextUser);

            if (user == null)
                throw new ApplicationException($"Unable to load user with ID '{httpContextUser.GetUserId()}'.");

            return user;
        }

        public IEnumerable<UserSearchResult> SearchUser(
            string filter,
            int? minBirthYear = null,
            int? maxBirthYear = null, 
            params Guid[] idsToExclude)
        {
            if (string.IsNullOrWhiteSpace(filter))
                return new UserSearchResult[0];

            var splittedFilter = filter.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);

            var registeredUsersSearchQuery = this.Database.RegisteredUsers
                .Where(ru => splittedFilter.Contains(ru.FirstName) || splittedFilter.Contains(ru.LastName));

            if (idsToExclude != null && idsToExclude.Length > 0)
                registeredUsersSearchQuery = registeredUsersSearchQuery.Where(ru => idsToExclude.All(id => ru.Id != id));

            if (minBirthYear.HasValue)
                registeredUsersSearchQuery = registeredUsersSearchQuery.Where(ru => ru.BirthDate.Year >= minBirthYear.Value);

            if (maxBirthYear.HasValue)
                registeredUsersSearchQuery = registeredUsersSearchQuery.Where(ru => ru.BirthDate.Year <= maxBirthYear.Value);

            var results = (from registeredUser in registeredUsersSearchQuery
                           orderby registeredUser.BirthDate descending
                           select new UserSearchResult
                           {
                               Id = registeredUser.Id,
                               FirstName = registeredUser.FirstName,
                               LastName = registeredUser.LastName,
                               BirthDate = registeredUser.BirthDate
                           }).ToArray();

            return results;
        }

        public async Task CreateCoachIfNotExists(Guid registeredUserId)
        {
            var existingCoach = (from coach in this.Database.Coaches
                                 where coach.RegisteredUserId == registeredUserId
                                 select coach).SingleOrDefault();
            if (existingCoach != null) return;

            var registeredUser = this.DbContext.RegisteredUsers.WithId(registeredUserId);

            var newCoach = new Coach { RegisteredUser = registeredUser };

            this.DbContext.Coaches.Add(newCoach);
            await this.DbContext.SaveChangesAsync();
        }

        private const string DefaultActivitiesSortExpression = nameof(ActivitiesViewModel.Activity.StartDate);

        public async Task<PagingList<ActivitiesViewModel.Activity>> GetActivitiesAsync(
            string userId,
            string filter = null,
            int pageSize = DefaultPageSize,
            int page = 1,
            string sortExpression = DefaultActivitiesSortExpression)
        {
            if (pageSize < DefaultPageSize)
                pageSize = DefaultPageSize;

            if (page < 1)
                page = 1;

            if (string.IsNullOrWhiteSpace(sortExpression))
                sortExpression = DefaultActivitiesSortExpression;

            var baseActivitiesQuery = (from registeredUser in this.Database.RegisteredUsers
                                       where registeredUser.AspNetUserId == userId
                                       from rut in registeredUser.Teams
                                       from activity in rut.Team.Calendar
                                       select activity);

            if (!string.IsNullOrWhiteSpace(filter))
                baseActivitiesQuery = baseActivitiesQuery.Where(activity => activity.Name.Contains(filter));

            var activitiesQuery = (from activity in baseActivitiesQuery
                                   select new ActivitiesViewModel.Activity
                                   {
                                       Id = activity.Id,
                                       Name = activity.Name,
                                       StartDate = activity.StartDate,
                                       EndDate = activity.EndDate
                                   });

            var activities = await PagingList<ActivitiesViewModel.Activity>.CreateAsync(
                activitiesQuery,
                pageSize,
                page,
                sortExpression,
                DefaultActivitiesSortExpression);

            return activities;
        }

        private const string DefaultNewsFeedSortExpression = "-" + nameof(NewsFeedViewModel.Content.PublicationDate);

        public async Task<PagingList<NewsFeedViewModel.Content>> GetFeedAsync(
            string userId,
            string filter = null,
            int pageSize = DefaultPageSize,
            int page = 1,
            string sortExpression = DefaultNewsFeedSortExpression)
        {
            if (pageSize < DefaultPageSize)
                pageSize = DefaultPageSize;

            if (page < 1)
                page = 1;

            if (string.IsNullOrWhiteSpace(sortExpression))
                sortExpression = DefaultNewsFeedSortExpression;

            var baseNewsFeedContentsQuery = this.Database.Contents;

            if (!string.IsNullOrWhiteSpace(filter))
                baseNewsFeedContentsQuery = baseNewsFeedContentsQuery.Where(content => content.Title.Contains(filter));

            var newsFeedContentsQuery = (from content in baseNewsFeedContentsQuery
                                         let caption = content.Caption == null || content.Caption == "" 
                                            ? content.Body.Substring(0, 1000) 
                                            : content.Caption
                                         let matchReportContentType = content is MatchReport
                                         select new NewsFeedViewModel.Content
                                         {
                                             Id = content.Id,
                                             Title = content.Title,
                                             Caption = caption,
                                             PublicationDate = content.PublicationDate,
                                             Type = matchReportContentType 
                                                ? NewsFeedViewModel.ContentType.MatchReport 
                                                : NewsFeedViewModel.ContentType.Article
                                         });

            var newsFeedContents = await PagingList<NewsFeedViewModel.Content>.CreateAsync(
                newsFeedContentsQuery, 
                pageSize, 
                page, 
                sortExpression,
                DefaultNewsFeedSortExpression);

            return newsFeedContents;
        }

        public CalendarViewModel GetCalendarViewModel(
            string actionName, 
            string controllerName = null, 
            string areaName = null,
            bool editable = false,
            bool selectable = false)
        {
            var dataUrl = this.UrlService.GetActionUrl(actionName, controllerName, areaName);

            return new CalendarViewModel
            {
                DataUrl = dataUrl,
                Editable = editable,
                Selectable = selectable
            };
        }

        public async Task<CalendarEvent[]> GetUserCalendarEventsAsync(
            string userId,
            bool overlap,
            bool editable,
            bool durationEditable,
            DateTime? startDate = null, 
            DateTime? endDate = null)
        {
            var calendarEventsQuery = (from registeredUser in this.Database.RegisteredUsers
                                       where registeredUser.AspNetUserId == userId
                                       from rut in registeredUser.Teams
                                       from activity in rut.Team.Calendar
                                       select activity);

            var calendarEvents = await GetCalendarEventsAsync(calendarEventsQuery, overlap, editable, durationEditable, startDate, endDate);

            return calendarEvents;
        }

        public async Task<CalendarEvent[]> GetTeamCalendarEventsAsync(
            Guid teamId,
            bool overlap,
            bool editable,
            bool durationEditable,
            DateTime? startDate = null,
            DateTime? endDate = null)
        {
            var calendarEventsQuery = (from team in this.Database.Teams
                                       where team.Id == teamId
                                       from activity in team.Calendar
                                       select activity);

            var calendarEvents = await GetCalendarEventsAsync(calendarEventsQuery, overlap, editable, durationEditable, startDate, endDate);

            return calendarEvents;
        }

        private async Task<CalendarEvent[]> GetCalendarEventsAsync(
            IQueryable<Activity> baseCalendarEventsQuery,
            bool overlap,
            bool editable,
            bool durationEditable,
            DateTime? startDate = null,
            DateTime? endDate = null)
        {
            if (startDate.HasValue)
                baseCalendarEventsQuery = baseCalendarEventsQuery.Where(a => a.StartDate >= startDate.Value);

            if (endDate.HasValue)
                baseCalendarEventsQuery = baseCalendarEventsQuery.Where(a => a.EndDate <= endDate.Value);

            var calendarEvents = await (from activity in baseCalendarEventsQuery
                                        select new CalendarEvent
                                        {
                                            Id = activity.Id,
                                            Title = activity.Name,
                                            Start = activity.StartDate.ToString("yyyy-MM-dd HH:mm:ss"),
                                            End = activity.EndDate.ToString("yyyy-MM-dd HH:mm:ss"),
                                            Overlap = overlap,
                                            Editable = editable,
                                            DurationEditable = durationEditable
                                        }).ToArrayAsync();

            return calendarEvents;
        }

        public async Task<byte[]> GetPictureAsync(string pictureId, int? size = null)
        {
            var bytes = await this.StorageService.GetFileBytesAsync(pictureId);
            if (bytes == null) return bytes;

            if (size.HasValue && size.Value >= 1 && size.Value <= byte.MaxValue)
                bytes = await ImagesHelper.ResizeImageAsync(bytes, size.Value, quality: 70);

            return bytes;
        }
    }
}
