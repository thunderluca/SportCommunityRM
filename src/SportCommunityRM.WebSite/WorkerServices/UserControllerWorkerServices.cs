using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SportCommunityRM.Data;
using SportCommunityRM.Data.ReadModel;
using SportCommunityRM.WebSite.Models;
using SportCommunityRM.WebSite.ViewModels.User;
using SportCommunityRM.WebSite.Services;
using System.Threading.Tasks;
using SportCommunityRM.Data.Models;
using SportCommunityRM.WebSite.Helpers;
using SportCommunityRM.WebSite.Controllers;

namespace SportCommunityRM.WebSite.WorkerServices
{
    public class UserControllerWorkerServices : BaseControllerWorkerServices
    {
        private readonly IStorageService StorageService;

        public UserControllerWorkerServices(
            UserManager<ApplicationUser> userManager, 
            SCRMContext dbContext, 
            IDatabase database, 
            IHttpContextAccessor httpContextAccessor, 
            IUrlService urlService,
            IStorageService storageService,
            ILogger<BaseControllerWorkerServices> logger) : base(userManager, dbContext, database, httpContextAccessor, urlService, logger)
        {
            this.StorageService = storageService ?? throw new ArgumentNullException(nameof(storageService));
        }

        public async Task<DetailViewModel> GetDetailViewModelAsync(Guid registeredUserId)
        {
            var user = await this.GetApplicationUserAsync();

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

            if (model != null)
                model.PictureUrl = UrlService.GetActionUrl(nameof(UserController.Picture), "User", new { username = user.UserName });

            return model;
        }

        public async Task<byte[]> GetUserPictureAsync(string username, int? size = null)
        {
            if (string.IsNullOrWhiteSpace(username))
                return null;

            var user = await this.GetApplicationUserAsync(username);

            var registeredUser = this.Database.RegisteredUsers.WithUserId(user.Id);
            if (registeredUser == null || string.IsNullOrWhiteSpace(registeredUser.PictureId))
                return null;

            var bytes = await this.StorageService.GetFileBytesAsync(registeredUser.PictureId);
            if (bytes == null) return bytes;

            if (size.HasValue && size.Value >= 1 && size.Value <= byte.MaxValue)
                bytes = await ImagesHelper.ResizeImageAsync(bytes, size.Value, quality: 70);

            return bytes;
        }
    }
}
