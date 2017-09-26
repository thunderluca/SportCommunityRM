using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SportCommunityRM.WebSite.WorkerServices;
using Microsoft.AspNetCore.Authorization;

namespace SportCommunityRM.WebSite.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly UserControllerWorkerServices WorkerServices;

        public UserController(UserControllerWorkerServices workerServices)
        {
            this.WorkerServices = workerServices ?? throw new ArgumentNullException(nameof(workerServices));
        }

        [HttpGet]
        public async Task<IActionResult> Detail(Guid? id)
        {
            if (id.IsNullOrEmpty())
                return NotFound();

            var model = await this.WorkerServices.GetDetailViewModelAsync(id.Value);

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Picture(string username, int? size)
        {
            var bytes = await this.WorkerServices.GetUserPictureAsync(username, size);

            return File(bytes ?? new byte[0], "image/jpeg");
        }
    }
}