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
        public IActionResult Detail(Guid? id)
        {
            if (id.IsNullOrEmpty())
                return NotFound();

            var model = this.WorkerServices.GetDetailViewModel(id.Value);

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Picture(string username)
        {
            var bytes = await this.WorkerServices.GetUserPictureAsync(username);

            return File(bytes, "image/jpeg");
        }
    }
}