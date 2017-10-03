using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using ReflectionIT.Mvc.Paging;
using SportCommunityRM.Data;
using SportCommunityRM.Data.Models;
using SportCommunityRM.Data.ReadModel;
using SportCommunityRM.WebSite.Models;
using SportCommunityRM.WebSite.Services;
using SportCommunityRM.WebSite.ViewModels.Coach;
using System;
using System.Linq;
using System.Threading.Tasks;
using static SportCommunityRM.WebSite.Models.ClaimPoliciesConstants;

namespace SportCommunityRM.WebSite.WorkerServices
{
    public class CoachControllerWorkerServices : BaseControllerWorkerServices
    {
        public CoachControllerWorkerServices(
            UserManager<ApplicationUser> userManager, 
            SCRMContext dbContext, 
            IDatabase database, 
            IHttpContextAccessor httpContextAccessor,
            IHostingEnvironment hostingEnvironment,
            IUrlService urlService,
            IStorageService storageService,
            ILogger<CoachControllerWorkerServices> logger) : base(userManager, dbContext, database, httpContextAccessor, hostingEnvironment, urlService, storageService, logger)
        {
        }

        public async Task<IndexViewModel> GetIndexViewModelAsync()
        {
            var coaches = await this.GetCoachesPagingListAsync();

            var userClaimTypes = await this.GetApplicationUserClaims();

            var isCreateAllowed = userClaimTypes.Any(uc => uc == CreateCoaches.Value);
            var isDeleteAllowed = userClaimTypes.Any(uc => uc == DeleteCoaches.Value);

            return new IndexViewModel(isCreateAllowed, isDeleteAllowed, isEditAllowed: false)
            {
                Coaches = coaches
            };
        }

        public async Task<PagingList<IndexViewModel.Coach>> GetCoachesPagingListAsync(
            string filter = null,
            int pageSize = DefaultPageSize,
            int page = 1,
            string sortExpression = nameof(IndexViewModel.Coach.Name))
        {
            if (pageSize < DefaultPageSize)
                pageSize = DefaultPageSize;

            if (page < 1)
                page = 1;

            if (string.IsNullOrWhiteSpace(sortExpression))
                sortExpression = nameof(IndexViewModel.Coach.Name);

            var baseCoachesQuery = this.Database.Coaches;
            if (!string.IsNullOrWhiteSpace(filter))
                baseCoachesQuery = baseCoachesQuery.Where(coach => coach.RegisteredUser.FirstName.Contains(filter) || coach.RegisteredUser.LastName.Contains(filter));

            var coachesQuery = (from coach in baseCoachesQuery
                                let teams = coach.Teams
                                     .Select(tc => new IndexViewModel.Team
                                     {
                                         Id = tc.TeamId,
                                         Name = tc.Team.Name
                                     })
                                select new IndexViewModel.Coach
                                {
                                    Id = coach.Id,
                                    Name = coach.RegisteredUser.FirstName + " " + coach.RegisteredUser.LastName,
                                    Teams = teams
                                });

            var coaches = await PagingList.CreateAsync(
                coachesQuery,
                pageSize,
                page,
                sortExpression,
                nameof(IndexViewModel.Coach.Name));

            return coaches;
        }

        public async Task RemoveCoachFromTeamAsync(Guid coachId, Guid teamId)
        {
            var team = this.DbContext.Teams
                .Include(t => t.Coaches)
                .WithId(teamId);

            var coach = team.Coaches.SingleOrDefault(c => c.CoachId == coachId);
            if (coach != null)
                team.Coaches.Remove(coach);

            await this.DbContext.SaveChangesAsync();
        }

        public async Task DeleteCoachAsync(Guid coachId)
        {
            var coach = this.DbContext.Coaches.WithId(coachId);

            var coachUser = await this.UserManager.FindByIdAsync(coach.RegisteredUser.AspNetUserId);

            var coachUserClaims = await this.UserManager.GetClaimsAsync(coachUser);
            if (!coachUserClaims.IsNullOrEmpty())
            {
                var claimsToRemove = coachUserClaims.Where(c => CoachesClaims.Any(uc => uc.Type == c.Type));
                await this.UserManager.RemoveClaimsAsync(coachUser, claimsToRemove);
            }

            this.DbContext.Coaches.Remove(coach);
            await this.DbContext.SaveChangesAsync();
        }
    }
}
