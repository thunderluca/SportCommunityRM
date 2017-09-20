using Microsoft.AspNetCore.Mvc;
using ReflectionIT.Mvc.Paging;
using SportCommunityRM.Data.ReadModel;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SportCommunityRM.WebSite.Components
{
    public class ActivitiesViewComponent : ViewComponent
    {
        private readonly IDatabase Database;

        public ActivitiesViewComponent(IDatabase database)
        {
            this.Database = database ?? throw new ArgumentNullException(nameof(database));
        }

        private const int DefaultPageSize = 10;

        public async Task<IViewComponentResult> InvokeAsync(
            string filter = null, 
            int? pageSize = null, 
            int? page = null,
            string sortExpression = null)
        {
            var userId = this.UserClaimsPrincipal.GetUserId();
            if (string.IsNullOrWhiteSpace(userId))
                return View();

            var pagingList = await GetActivitiesPagingListAsync(
                userId,
                filter,
                pageSize.GetValueOrDefault(),
                page.GetValueOrDefault(),
                sortExpression);

            return View(pagingList);
        }

        private async Task<PagingList<Model.Activity>> GetActivitiesPagingListAsync(
            string userId, 
            string filter = null,
            int pageSize = DefaultPageSize, 
            int page = 1,
            string sortExpression = nameof(Model.Activity.StartDate))
        {
            if (pageSize < DefaultPageSize)
                pageSize = DefaultPageSize;

            if (page < 1)
                page = 1;

            if (string.IsNullOrWhiteSpace(sortExpression))
                sortExpression = nameof(Model.Activity.StartDate);

            var baseActivitiesQuery = (from registeredUser in this.Database.RegisteredUsers
                                       where registeredUser.AspNetUserId == userId
                                       from rut in registeredUser.Teams
                                       from activity in rut.Team.Calendar
                                       select activity);

            if (!string.IsNullOrWhiteSpace(filter))
                baseActivitiesQuery = baseActivitiesQuery.Where(activity => activity.Name.Contains(filter));

            var activitiesQuery = (from activity in baseActivitiesQuery
                                   select new Model.Activity
                                   {
                                       Id = activity.Id,
                                       Name = activity.Name,
                                       StartDate = activity.StartDate,
                                       EndDate = activity.EndDate
                                   });

            var activities = await PagingList<Model.Activity>.CreateAsync(
                activitiesQuery, 
                pageSize, 
                page, 
                sortExpression, 
                nameof(Model.Activity.StartDate));

            return activities;
        }

        public class Model
        {
            public PagingList<Activity> Activities { get; set; }

            public class Activity
            {
                public Guid Id { get; set; }

                [Display(Name = "Name")]
                public string Name { get; set; }

                [Display(Name = "Start date")]
                public DateTime StartDate { get; set; }

                [Display(Name = "End date")]
                public DateTime EndDate { get; set; }
            }
        }
    }
}
