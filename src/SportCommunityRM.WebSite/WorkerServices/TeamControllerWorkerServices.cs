using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using SportCommunityRM.Data.ReadModel;
using SportCommunityRM.WebSite.Models;
using SportCommunityRM.Data;
using Microsoft.Extensions.Logging;
using SportCommunityRM.WebSite.ViewModels.Team;
using Microsoft.EntityFrameworkCore;
using SportCommunityRM.Data.Models;

namespace SportCommunityRM.WebSite.WorkerServices
{
    public class TeamControllerWorkerServices : BaseControllerWorkerServices
    {
        public TeamControllerWorkerServices(
            UserManager<ApplicationUser> userManager,
            SCRMContext dbContext,
            IDatabase database, 
            IHttpContextAccessor httpContextAccessor,
            ILogger<TeamControllerWorkerServices> logger) : base(userManager, dbContext, database, httpContextAccessor, logger)
        {
        }

        public IndexViewModel GetIndexViewModel()
        {
            var teams = (from team in this.Database.Teams
                         orderby team.Name ascending
                         select new IndexViewModel.Team
                         {
                             Id = team.Id,
                             Name = team.Name,
                             PlayersCount = team.Players.Count(),
                             CoachesCount = team.Coaches.Count()
                         }).ToArray();

            return new IndexViewModel
            {
                Teams = teams
            };
        }

        public CreateViewModel GetCreateViewModel()
        {
            return new CreateViewModel();
        }

        public async Task CreateTeamAsync(CreateViewModel viewModel)
        {
            if (viewModel == null)
                throw new ArgumentNullException(nameof(viewModel));

            var newTeam = new Team
            {
                Name = viewModel.Name,
                MinBirthYear = viewModel.MinBirthYear,
                MaxBirthYear = viewModel.MaxBirthYear
            };

            var teamEntity = this.DbContext.Teams.Add(newTeam);
            await this.DbContext.SaveChangesAsync();

            if (viewModel.Players.IsNullOrEmpty() && viewModel.Coaches.IsNullOrEmpty()) return;

            var team = this.DbContext.Teams.WithId(teamEntity.Entity.Id);

            if (!viewModel.Players.IsNullOrEmpty())
            {
                var selectedPlayersIds = viewModel.Players?.Length > 0
                    ? viewModel.Players.Select(p => p.Id).ToArray()
                    : new Guid[0];

                var registeredUsersTeams = this.DbContext.RegisteredUsers
                    .Where(registeredUser => selectedPlayersIds.Any(id => registeredUser.Id == id))
                    .ToArray()
                    .Select(registeredUser => new RegisteredUserTeam
                    {
                        RegisteredUser = registeredUser,
                        Team = team
                    }).ToArray();

                foreach (var player in registeredUsersTeams)
                    team.Players.Add(player);
            }

            if (!viewModel.Coaches.IsNullOrEmpty())
            {
                var selectedCoachesId = viewModel.Coaches?.Length > 0
                    ? viewModel.Coaches.Select(c => c.Id).ToArray()
                    : new Guid[0];

                foreach (var id in selectedCoachesId)
                    await this.CreateCoachIfNotExists(id);

                var teamsCoaches = this.DbContext.Coaches
                    .Where(coach => selectedCoachesId.Any(id => coach.Id == id))
                    .ToArray()
                    .Select(coach => new TeamCoach
                    {
                        Team = team,
                        Coach = coach
                    }).ToArray();

                foreach (var coach in teamsCoaches)
                    team.Coaches.Add(coach);
            }

            await this.DbContext.SaveChangesAsync();
        }

        public EditViewModel GetEditViewModel(Guid teamId)
        {
            var model = (from team in this.Database.Teams
                         where team.Id == teamId
                         let players = team.Players
                            .Select(rut => new EditViewModel.Player
                            {
                                Id = rut.RegisteredUser.Id,
                                FirstName = rut.RegisteredUser.FirstName,
                                LastName = rut.RegisteredUser.LastName,
                                BirthDate = rut.RegisteredUser.BirthDate
                            })
                         let coaches = team.Coaches
                            .Select(tc => new EditViewModel.Coach
                            {
                                Id = tc.Coach.Id,
                                FirstName = tc.Coach.RegisteredUser.FirstName,
                                LastName = tc.Coach.RegisteredUser.LastName,
                                BirthDate = tc.Coach.RegisteredUser.BirthDate
                            })
                         select new EditViewModel
                         {
                             Id = team.Id,
                             Name = team.Name,
                             MinBirthYear = team.MinBirthYear,
                             MaxBirthYear = team.MaxBirthYear,
                             Players = players.ToArray(),
                             Coaches = coaches.ToArray()
                         }).SingleOrDefault();

            return model;
        }

        public async Task EditTeamAsync(EditViewModel viewModel)
        {
            var team = this.DbContext.Teams
                .Include(t => t.Players)
                .Include(t => t.Coaches)
                .WithId(viewModel.Id);

            team.Name = viewModel.Name;
            team.MinBirthYear = viewModel.MinBirthYear;
            team.MaxBirthYear = viewModel.MaxBirthYear;

            foreach (var playerToRemove in team.Players)
                team.Players.Remove(playerToRemove);

            foreach (var coachToRemove in team.Coaches)
                team.Coaches.Remove(coachToRemove);

            if (viewModel.Players.IsNullOrEmpty() && viewModel.Coaches.IsNullOrEmpty())
            {
                await this.DbContext.SaveChangesAsync();
                return;
            }

            team = this.DbContext.Teams
                .Include(t => t.Players)
                .Include(t => t.Coaches)
                .WithId(viewModel.Id);

            if (!viewModel.Players.IsNullOrEmpty())
            {
                var selectedPlayersIds = viewModel.Players?.Length > 0
                    ? viewModel.Players.Select(p => p.Id).ToArray()
                    : new Guid[0];

                var registeredUsersTeams = this.DbContext.RegisteredUsers
                    .Where(registeredUser => selectedPlayersIds.Any(id => registeredUser.Id == id))
                    .ToArray()
                    .Select(registeredUser => new RegisteredUserTeam
                    {
                        RegisteredUser = registeredUser,
                        Team = team
                    }).ToArray();

                foreach (var player in registeredUsersTeams)
                    team.Players.Add(player);
            }

            if (!viewModel.Coaches.IsNullOrEmpty())
            {
                var selectedCoachesId = viewModel.Coaches?.Length > 0
                    ? viewModel.Coaches.Select(c => c.Id).ToArray()
                    : new Guid[0];

                var teamsCoaches = this.DbContext.Coaches
                    .Where(coach => selectedCoachesId.Any(id => coach.Id == id))
                    .ToArray()
                    .Select(coach => new TeamCoach
                    {
                        Team = team,
                        Coach = coach
                    }).ToArray();

                foreach (var coach in teamsCoaches)
                    team.Coaches.Add(coach);
            }

            await this.DbContext.SaveChangesAsync();
        }

        public async Task DeleteTeamAsync(Guid teamId)
        {
            var team = this.DbContext.Teams.WithId(teamId);

            this.DbContext.Teams.Remove(team);

            await this.DbContext.SaveChangesAsync();
        }

        public DetailViewModel GetDetailViewModel(Guid teamId)
        {
            var model = (from team in this.Database.Teams
                         where team.Id == teamId
                         let players = team.Players
                            .Select(rut => new DetailViewModel.Player
                            {
                                Id = rut.RegisteredUser.Id,
                                Name = rut.RegisteredUser.FirstName + " " + rut.RegisteredUser.LastName,
                                BirthDate = rut.RegisteredUser.BirthDate
                            })
                         let coaches = team.Coaches
                            .Select(tc => new DetailViewModel.Coach
                            {
                                Id = tc.Coach.Id,
                                Name = tc.Coach.RegisteredUser.FirstName + " " + tc.Coach.RegisteredUser.LastName
                            })
                         select new DetailViewModel
                         {
                             Id = team.Id,
                             Name = team.Name,
                             MinBirthYear = team.MinBirthYear,
                             MaxBirthYear = team.MaxBirthYear,
                             Players = players,
                             Coaches = coaches
                         }).SingleOrDefault();

            return model;
        }

        public async Task RemoveCoachAsync(Guid teamId, Guid coachId)
        {
            var team = this.DbContext.Teams
                .Include(t => t.Coaches)
                .WithId(teamId);

            var coach = team.Coaches.SingleOrDefault(rut => rut.CoachId == coachId);
            if (coach != null)
                team.Coaches.Remove(coach);

            await this.DbContext.SaveChangesAsync();
        }

        public async Task RemovePlayerAsync(Guid teamId, Guid playerId)
        {
            var team = this.DbContext.Teams
                .Include(t => t.Players)
                .WithId(teamId);

            var player = team.Players.SingleOrDefault(rut => rut.RegisteredUserId == playerId);
            if (player != null)
                team.Players.Remove(player);

            await this.DbContext.SaveChangesAsync();
        }
    }
}
