using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SportCommunityRM.WebSite.WorkerServices;
using SportCommunityRM.WebSite.ViewModels.Team;
using SportCommunityRM.WebSite.Models;

namespace SportCommunityRM.WebSite.Controllers
{
    public class TeamController : BaseController, IUserSearchController
    {
        private readonly TeamControllerWorkerServices WorkerServices;

        public TeamController(TeamControllerWorkerServices workerServices)
        {
            this.WorkerServices = workerServices ?? throw new ArgumentNullException(nameof(workerServices));
        }

        [HttpGet]
        public IActionResult Index()
        {
            var model = this.WorkerServices.GetIndexViewModel();

            return View(model);
        }

        [HttpPost]
        public IEnumerable<UserSearchResult> SearchUser([FromBody] UserSearchRequest request)
        {
            if (request == null)
                return new UserSearchResult[0];

            var results = this.WorkerServices.SearchUser(request.Filter, request.MinBirthYear, request.MaxBirthYear, request.IdsToExclude);

            return results;
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = this.WorkerServices.GetCreateViewModel();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            await this.WorkerServices.CreateTeamAsync(viewModel);

            return RedirectToAction(nameof(this.Index));
        }

        [HttpGet]
        public IActionResult Edit(Guid? id)
        {
            if (id.IsNullOrEmpty())
                return NotFound();

            var model = this.WorkerServices.GetEditViewModel(id.Value);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            await this.WorkerServices.EditTeamAsync(viewModel);

            return RedirectToAction(nameof(this.Index));
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id.IsNullOrEmpty())
                return NotFound();

            await this.WorkerServices.DeleteTeamAsync(id.Value);

            return RedirectToAction(nameof(this.Index));
        }
    }
}