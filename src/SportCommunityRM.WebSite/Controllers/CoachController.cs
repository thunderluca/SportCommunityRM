﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SportCommunityRM.WebSite.Models;
using SportCommunityRM.WebSite.WorkerServices;
using SportCommunityRM.WebSite.ViewModels.Coach;
using Microsoft.AspNetCore.Authorization;

namespace SportCommunityRM.WebSite.Controllers
{
    [Authorize]
    public class CoachController : BaseController, IUserSearchController
    {
        private readonly CoachControllerWorkerServices WorkerServices;

        public CoachController(CoachControllerWorkerServices workerServices)
        {
            this.WorkerServices = workerServices ?? throw new ArgumentNullException(nameof(workerServices));
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var model = await this.WorkerServices.GetIndexViewModelAsync();

            return View(model);
        }

        [HttpPost]
        public IEnumerable<UserSearchResult> SearchUser([FromBody] UserSearchRequest request)
        {
            var results = this.WorkerServices.SearchUser(request.Filter);

            return results;
        }

        [HttpGet]
        [Authorize(Policy = nameof(ClaimPoliciesConstants.CreateCoaches))]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Policy = nameof(ClaimPoliciesConstants.CreateCoaches))]
        public async Task<IActionResult> Add(AddViewModel viewModel)
        {
            if (!ModelState.IsValid || viewModel.Id.IsNullOrEmpty())
                return View(viewModel);

            await this.WorkerServices.CreateCoachIfNotExists(viewModel.Id.Value);

            return RedirectToAction(nameof(this.Index));
        }

        [HttpGet]
        [Authorize(Policy = nameof(ClaimPoliciesConstants.DeleteTeams))]
        public async Task<IActionResult> Remove(Guid? coachId, Guid? teamId)
        {
            if (coachId.IsNullOrEmpty() || teamId.IsNullOrEmpty())
                return NotFound();

            await this.WorkerServices.RemoveCoachFromTeamAsync(coachId.Value, teamId.Value);

            return RedirectToAction(nameof(this.Index));
        }

        [HttpGet]
        [Authorize(Policy = nameof(ClaimPoliciesConstants.DeleteCoaches))]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id.IsNullOrEmpty())
                return NotFound();

            await this.WorkerServices.DeleteCoachAsync(id.Value);

            return RedirectToAction(nameof(this.Index));
        }
    }
}