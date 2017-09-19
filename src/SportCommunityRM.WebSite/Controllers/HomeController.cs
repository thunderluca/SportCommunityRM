using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc;

namespace SportCommunityRM.WebSite.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(UrlEncoder urlEncoder) : base(urlEncoder)
        {
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }
    }
}
