using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SportCommunityRM.Data;
using SportCommunityRM.Data.ReadModel;
using SportCommunityRM.WebSite.Models;
using SportCommunityRM.WebSite.ViewModels.User;

namespace SportCommunityRM.WebSite.WorkerServices
{
    public class UserControllerWorkerServices : BaseControllerWorkerServices
    {
        public UserControllerWorkerServices(
            UserManager<ApplicationUser> userManager, 
            SCRMContext dbContext, 
            IDatabase database, 
            IHttpContextAccessor httpContextAccessor, 
            ILogger<BaseControllerWorkerServices> logger) : base(userManager, dbContext, database, httpContextAccessor, logger)
        {
        }

        public DetailViewModel GetDetailViewModel(Guid registeredUserId)
        {
            var model = (from registeredUser in this.Database.RegisteredUsers
                         where registeredUser.Id == registeredUserId
                         let teams = registeredUser.Teams
                            .Select(rut => new DetailViewModel.Team
                            {
                                Id = rut.TeamId,
                                Name = rut.Team.Name
                            })
                         select new DetailViewModel
                         {
                             Id = registeredUserId,
                             FirstName = registeredUser.FirstName,
                             LastName = registeredUser.LastName,
                             BirthDate = registeredUser.BirthDate,
                             Teams = teams
                         }).SingleOrDefault();

            return model;
        }
    }
}
