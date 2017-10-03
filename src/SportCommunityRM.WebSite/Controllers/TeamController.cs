using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SportCommunityRM.WebSite.WorkerServices;
using SportCommunityRM.WebSite.ViewModels.Team;
using SportCommunityRM.WebSite.Models;
using Microsoft.AspNetCore.Authorization;
using SportCommunityRM.WebSite.Helpers;

namespace SportCommunityRM.WebSite.Controllers
{
    [Authorize]
    public class TeamController : BaseController, IUserSearchController, IPictureController
    {
        private readonly TeamControllerWorkerServices WorkerServices;

        public TeamController(TeamControllerWorkerServices workerServices)
        {
            this.WorkerServices = workerServices ?? throw new ArgumentNullException(nameof(workerServices));
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var model = await this.WorkerServices.GetIndexViewModelAsync();

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
        [Authorize(Policy = nameof(ClaimPoliciesConstants.CreateTeams))]
        public IActionResult Create()
        {
            var model = this.WorkerServices.GetCreateViewModel();

            return View(model);
        }

        [HttpPost]
        [Authorize(Policy = nameof(ClaimPoliciesConstants.CreateTeams))]
        public async Task<IActionResult> Create(CreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            await this.WorkerServices.CreateTeamAsync(viewModel);

            return RedirectToAction(nameof(this.Index));
        }

        [HttpGet]
        [Authorize(Policy = nameof(ClaimPoliciesConstants.EditTeams))]
        public IActionResult Edit(Guid? id)
        {
            if (id.IsNullOrEmpty())
                return NotFound();

            var model = this.WorkerServices.GetEditViewModel(id.Value);

            return View(model);
        }

        [HttpPost]
        [Authorize(Policy = nameof(ClaimPoliciesConstants.EditTeams))]
        public async Task<IActionResult> Edit(EditViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            await this.WorkerServices.EditTeamAsync(viewModel);

            return RedirectToAction(nameof(this.Index));
        }

        [HttpGet]
        [Authorize(Policy = nameof(ClaimPoliciesConstants.DeleteTeams))]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id.IsNullOrEmpty())
                return NotFound();

            await this.WorkerServices.DeleteTeamAsync(id.Value);

            return RedirectToAction(nameof(this.Index));
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Detail(Guid? id)
        {
            if (id.IsNullOrEmpty())
                return NotFound();

            var model = this.WorkerServices.GetDetailViewModel(id.Value);

            return View(model);
        }

        [HttpGet]
        [Authorize(Policy = nameof(ClaimPoliciesConstants.EditTeams))]
        public async Task<IActionResult> RemoveCoach(Guid? teamId, Guid? coachId)
        {
            if (teamId.IsNullOrEmpty() || coachId.IsNullOrEmpty())
                return NotFound();

            await this.WorkerServices.RemoveCoachAsync(teamId.Value, coachId.Value);

            return RedirectToAction(nameof(this.Detail), new { id = teamId.Value });
        }

        [HttpGet]
        [Authorize(Policy = nameof(ClaimPoliciesConstants.EditTeams))]
        public async Task<IActionResult> RemovePlayer(Guid? teamId, Guid? playerId)
        {
            if (teamId.IsNullOrEmpty() || playerId.IsNullOrEmpty())
                return NotFound();

            await this.WorkerServices.RemovePlayerAsync(teamId.Value, playerId.Value);

            return RedirectToAction(nameof(this.Detail), new { id = teamId.Value });
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Picture(Guid teamId, int? size)
        {
            var bytes = await this.WorkerServices.GetTeamPictureAsync(teamId, size);

            return File(bytes ?? new byte[0], ImagesHelper.JpegMimeType);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetPicture(string pictureId, int? size)
        {
            var bytes = await this.WorkerServices.GetPictureAsync(pictureId, "/images/default.jpg", size);

            return File(bytes ?? new byte[0], ImagesHelper.JpegMimeType);
        }
    }
}