using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportCommunityRM.WebSite.WorkerServices;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using SportCommunityRM.WebSite.Models;

namespace SportCommunityRM.WebSite.Controllers
{
    [Authorize]
    public class HomeController : BaseController, IActivityController, INewsFeedController
    {
        private readonly HomeControllerWorkerServices WorkerServices;

        public HomeController(HomeControllerWorkerServices workerServices)
        {
            this.WorkerServices = workerServices ?? throw new ArgumentNullException(nameof(workerServices));
        }

        public async Task<IActionResult> Index()
        {
            var model = await this.WorkerServices.GetIndexViewModelAsync();

            return View(model);
        }

        public async Task<IActionResult> GetActivitiesAsync(string filter, int page, string sortExpression)
        {
            var userId = this.User.GetUserId();

            var activities = await this.WorkerServices.GetActivitiesAsync(userId, filter, page: page, sortExpression: sortExpression);

            return PartialView("DisplayTemplates/Activities", activities);
        }

        [HttpPost]
        public async Task<CalendarEvent[]> GetCalendarEventsAsync([FromBody] CalendarEventsRequest request)
        {
            var userId = this.User.GetUserId();

            var newsFeedContents = await this.WorkerServices.GetUserCalendarEventsAsync(
                userId,
                request.Overlap,
                request.Editable,
                request.DurationEditable,
                request.StartDate,
                request.EndDate);

            return newsFeedContents;
        }

        public async Task<IActionResult> GetFeedAsync(int page)
        {
            var userId = this.User.GetUserId();

            var newsFeedContents = await this.WorkerServices.GetFeedAsync(userId, page: page);

            return PartialView("DisplayTemplates/NewsFeed", newsFeedContents);
        }
    }
}
