using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace SportCommunityRM.WebSite.Controllers
{
    public interface IPictureController
    {
        Task<IActionResult> GetPicture(string pictureId, int? size);
    }
}
