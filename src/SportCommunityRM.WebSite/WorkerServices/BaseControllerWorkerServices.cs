using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using ReflectionIT.Mvc.Paging;
using SportCommunityRM.Data;
using SportCommunityRM.Data.Models;
using SportCommunityRM.Data.ReadModel;
using SportCommunityRM.WebSite.Models;
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
        public readonly ILogger Logger;

        public BaseControllerWorkerServices(
            UserManager<ApplicationUser> userManager,
            SCRMContext dbContext,
            IDatabase database,
            IHttpContextAccessor httpContextAccessor,
            ILogger<BaseControllerWorkerServices> logger)
        {
            this.UserManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            this.DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            this.Database = database ?? throw new ArgumentNullException(nameof(database));
            this.HttpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            this.Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public const int DefaultPageSize = 10;

        public async Task<ApplicationUser> GetApplicationUserAsync()
        {
            var httpContextUser = this.HttpContextAccessor.HttpContext.User;

            var user = await UserManager.GetUserAsync(httpContextUser);
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

        public async Task<PagingList<UserActivity>> GetActivitiesAsync(
            string userId,
            string filter = null,
            int pageSize = DefaultPageSize,
            int page = 1,
            string sortExpression = nameof(UserActivity.StartDate))
        {
            if (pageSize < DefaultPageSize)
                pageSize = DefaultPageSize;

            if (page < 1)
                page = 1;

            if (string.IsNullOrWhiteSpace(sortExpression))
                sortExpression = nameof(UserActivity.StartDate);

            var baseActivitiesQuery = (from registeredUser in this.Database.RegisteredUsers
                                       where registeredUser.AspNetUserId == userId
                                       from rut in registeredUser.Teams
                                       from activity in rut.Team.Calendar
                                       select activity);

            if (!string.IsNullOrWhiteSpace(filter))
                baseActivitiesQuery = baseActivitiesQuery.Where(activity => activity.Name.Contains(filter));

            var activitiesQuery = (from activity in baseActivitiesQuery
                                   select new UserActivity
                                   {
                                       Id = activity.Id,
                                       Name = activity.Name,
                                       StartDate = activity.StartDate,
                                       EndDate = activity.EndDate
                                   });

            var activities = await PagingList<UserActivity>.CreateAsync(
                activitiesQuery,
                pageSize,
                page,
                sortExpression,
                nameof(UserActivity.StartDate));

            return activities;
        }
    }
}
