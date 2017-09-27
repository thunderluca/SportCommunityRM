using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SportCommunityRM.Data;
using SportCommunityRM.Data.ReadModel;
using SportCommunityRM.WebSite.ViewModels.Home;
using SportCommunityRM.WebSite.Models;
using SportCommunityRM.WebSite.Services;
using System.Linq;
using SportCommunityRM.Data.Models;

namespace SportCommunityRM.WebSite.WorkerServices
{
    public class HomeControllerWorkerServices : BaseControllerWorkerServices
    {
        public HomeControllerWorkerServices(
            UserManager<ApplicationUser> userManager, 
            SCRMContext dbContext, 
            IDatabase database, 
            IHttpContextAccessor httpContextAccessor, 
            IUrlService urlService,
            IStorageService storageService,
            ILogger<HomeControllerWorkerServices> logger) : base(userManager, dbContext, database, httpContextAccessor, urlService, storageService, logger)
        {
        }

        public IndexViewModel GetIndexViewModel(
            int pinnedContentsCount = 5,
            int contentsCount = 10,
            int weekEventsCount = 20,
            int topScorersCount = 10)
        {
            var pinnedContents = (from content in this.Database.Contents.Pinned()
                                  orderby content.PublicationDate descending
                                  select new IndexViewModel.Content
                                  {
                                      Id = content.Id,
                                      Title = content.Title,
                                      Caption = content.Caption,
                                      PublicationDate = content.PublicationDate,
                                      Type = content is Article ? IndexViewModel.ContentType.Article : IndexViewModel.ContentType.MatchReport
                                  }).Take(pinnedContentsCount).ToArray();

            var contents = (from content in this.Database.Contents.NotPinned()
                            orderby content.PublicationDate descending
                            select new IndexViewModel.Content
                            {
                                Id = content.Id,
                                Title = content.Title,
                                Caption = content.Caption,
                                PublicationDate = content.PublicationDate,
                                Type = content is Article ? IndexViewModel.ContentType.Article : IndexViewModel.ContentType.MatchReport
                            }).Take(contentsCount).ToArray();

            var weekEvents = (from team in this.Database.Teams
                              from activity in team.Calendar
                              where activity is Match || activity is Tournament
                              orderby activity.StartDate ascending
                              select new IndexViewModel.Event
                              {
                                  Id = activity.Id,
                                  Name = activity.Name,
                                  StartDate = activity.StartDate,
                                  EndDate = activity.EndDate
                              }).Take(weekEventsCount).ToArray();

            var topScorers = (from registeredUser in this.Database.RegisteredUsers
                              let points = registeredUser.MatchScores.Sum(ms => ms.Points)
                              orderby points descending
                              select new IndexViewModel.Scorer
                              {
                                  Id = registeredUser.Id,
                                  FirstName = registeredUser.FirstName,
                                  LastName = registeredUser.LastName,
                                  Points = points
                              }).Take(topScorersCount).ToArray();

            return new IndexViewModel
            {
                Contents = contents,
                PinnedContents = pinnedContents,
                WeekEvents = weekEvents,
                TopScorers = topScorers
            };
        }
    }
}
