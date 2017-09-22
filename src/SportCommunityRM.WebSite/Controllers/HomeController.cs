using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportCommunityRM.WebSite.WorkerServices;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SportCommunityRM.WebSite.Controllers
{
    [Authorize]
    public class HomeController : BaseController, IActivityController
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

            return PartialView("_ActivitiesPartial", activities);
        }
    }
}
