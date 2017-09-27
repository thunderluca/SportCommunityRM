using Microsoft.AspNetCore.Mvc;
using SportCommunityRM.WebSite.WorkerServices;
using System;

namespace SportCommunityRM.WebSite.Controllers
{
    public class HomeController : BaseController
    {
        private readonly HomeControllerWorkerServices WorkerServices;

        public HomeController(HomeControllerWorkerServices workerServices)
        {
            this.WorkerServices = workerServices ?? throw new ArgumentNullException(nameof(workerServices));
        }

        [HttpGet]
        [ResponseCache(Duration = 1800)]
        public IActionResult Index()
        {
            var model = this.WorkerServices.GetIndexViewModel();

            return View(model);
        }
    }
}
