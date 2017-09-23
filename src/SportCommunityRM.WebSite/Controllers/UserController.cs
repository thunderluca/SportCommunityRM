using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SportCommunityRM.WebSite.WorkerServices;

namespace SportCommunityRM.WebSite.Controllers
{
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
    }
}