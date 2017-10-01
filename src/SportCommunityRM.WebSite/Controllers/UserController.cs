using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SportCommunityRM.WebSite.WorkerServices;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using SportCommunityRM.WebSite.Models;
using SportCommunityRM.WebSite.Helpers;

namespace SportCommunityRM.WebSite.Controllers
{
    [Authorize]
    public class UserController : BaseController, IActivityController, INewsFeedController, IPictureController
    {
        private readonly UserControllerWorkerServices WorkerServices;

        public UserController(UserControllerWorkerServices workerServices)
        {
            this.WorkerServices = workerServices ?? throw new ArgumentNullException(nameof(workerServices));
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index(string username = null, string id = null)
        {
            if (!string.IsNullOrWhiteSpace(username) || !string.IsNullOrWhiteSpace(id))
                return GetUserView(username, id);

            var model = await this.WorkerServices.GetIndexViewModelAsync();

            return View(model);
        }

        private IActionResult GetUserView(string username, string id)
        {
            var isGuid = Guid.TryParse(id, out Guid guid);
            if (isGuid && !guid.IsNullOrEmpty())
                return RedirectToAction(nameof(this.Detail), new { id = id });

            if (guid.IsNullOrEmpty() && string.IsNullOrWhiteSpace(username))
                return NotFound();

            return RedirectToAction(nameof(this.Detail), new { username = guid.IsNullOrEmpty() ? username : id });
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
        
        //public IActionResult Detail(Guid id)
        //{
        //    if (id.IsNullOrEmpty())
        //        return NotFound();

        //    var model = this.WorkerServices.GetDetailViewModel(id);

        //    return View(model);
        //}
        
        public async Task<IActionResult> Detail(string username, Guid id)
        {
            if (string.IsNullOrWhiteSpace(username) && id.IsNullOrEmpty())
                return NotFound();

            var model = !string.IsNullOrWhiteSpace(username) 
                ? await this.WorkerServices.GetDetailViewModelAsync(username)
                : this.WorkerServices.GetDetailViewModel(id);

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> UserPicture(string username, int? size)
        {
            var bytes = await this.WorkerServices.GetUserPictureAsync(username, size);

            return File(bytes ?? new byte[0], ImagesHelper.JpegMimeType);
        }

        public async Task<IActionResult> UserIdPicture(Guid id, int? size)
        {
            var bytes = await this.WorkerServices.GetUserPictureByIdAsync(id, size);

            return File(bytes ?? new byte[0], ImagesHelper.JpegMimeType);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetPicture(string pictureId, int? size)
        {
            var bytes = await this.WorkerServices.GetPictureAsync(pictureId, size);

            return File(bytes ?? new byte[0], ImagesHelper.JpegMimeType);
        }
    }
}