using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SportCommunityRM.Data;
using SportCommunityRM.Data.ReadModel;
using SportCommunityRM.WebSite.Models;
using SportCommunityRM.WebSite.Services;

namespace SportCommunityRM.WebSite.WorkerServices
{
    public class ContentControllerWorkerServices : BaseControllerWorkerServices
    {
        public ContentControllerWorkerServices(
            UserManager<ApplicationUser> userManager, 
            SCRMContext dbContext, 
            IDatabase database, 
            IHttpContextAccessor httpContextAccessor, 
            IUrlService urlService, 
            IStorageService storageService,
            ILogger<ContentControllerWorkerServices> logger) : base(userManager, dbContext, database, httpContextAccessor, urlService, storageService, logger)
        {
        }
    }
}
