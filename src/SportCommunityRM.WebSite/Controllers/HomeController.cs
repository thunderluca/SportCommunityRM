using Microsoft.AspNetCore.Mvc;
using SportCommunityRM.WebSite.WorkerServices;
using System;
using System.Threading.Tasks;

namespace SportCommunityRM.WebSite.Controllers
{
    public class HomeController : BaseController, IPictureController
    {
        private readonly HomeControllerWorkerServices WorkerServices;

        public HomeController(HomeControllerWorkerServices workerServices)
        {
            this.WorkerServices = workerServices ?? throw new ArgumentNullException(nameof(workerServices));
        }

        [HttpGet]
#if !DEBUG
        [ResponseCache(Duration = 1800)]
#endif
        public IActionResult Index()
        {
            var model = this.WorkerServices.GetIndexViewModel();

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetPicture(string pictureId, int? size)
        {
            var bytes = await this.WorkerServices.GetPictureAsync(pictureId, size);

            return File(bytes ?? new byte[0], "image/jpeg");
        }
    }
}
