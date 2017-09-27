using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SportCommunityRM.Data;
using SportCommunityRM.Data.ReadModel;
using SportCommunityRM.WebSite.Models;
using SportCommunityRM.WebSite.ViewModels.Coach;
using ReflectionIT.Mvc.Paging;
using SportCommunityRM.Data.Models;
using Microsoft.EntityFrameworkCore;
using SportCommunityRM.WebSite.Services;

namespace SportCommunityRM.WebSite.WorkerServices
{
    public class CoachControllerWorkerServices : BaseControllerWorkerServices
    {
        public CoachControllerWorkerServices(
            UserManager<ApplicationUser> userManager, 
            SCRMContext dbContext, 
            IDatabase database, 
            IHttpContextAccessor httpContextAccessor, 
            IUrlService urlService,
            IStorageService storageService,
            ILogger<CoachControllerWorkerServices> logger) : base(userManager, dbContext, database, httpContextAccessor, urlService, storageService, logger)
        {
        }

        public async Task<IndexViewModel> GetIndexViewModelAsync()
        {
            var coaches = await this.GetCoachesPagingListAsync();

            return new IndexViewModel
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

            var coaches = await PagingList<IndexViewModel.Coach>.CreateAsync(
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

            this.DbContext.Coaches.Remove(coach);

            await this.DbContext.SaveChangesAsync();
        }
    }
}
