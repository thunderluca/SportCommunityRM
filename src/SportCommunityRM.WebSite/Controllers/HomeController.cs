using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportCommunityRM.WebSite.WorkerServices;
using System;
using System.Threading.Tasks;

namespace SportCommunityRM.WebSite.Controllers
{
    [Authorize]
    public class HomeController : BaseController
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

        public IActionResult About()
        {
            throw new NotSupportedException();

            //ViewData["Message"] = "Your application description page.";

            //return View();
        }

        public IActionResult Contact()
        {
            throw new NotSupportedException();

            //ViewData["Message"] = "Your contact page.";

            //return View();
        }
    }
}
