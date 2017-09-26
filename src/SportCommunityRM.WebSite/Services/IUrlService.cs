using SportCommunityRM.WebSite.Models;
using System.Threading.Tasks;

namespace SportCommunityRM.WebSite.Services
{
    public interface IUrlService
    {
        Task<string> GenerateEmailConfirmationLinkAsync(ApplicationUser user);

        Task<string> GenerateForgotPasswordLinkAsync(ApplicationUser user);

        string GetActionUrl(string actionName, string controllerName = null, object values = null);
    }
}
