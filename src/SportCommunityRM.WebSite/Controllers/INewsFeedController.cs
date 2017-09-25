using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportCommunityRM.WebSite.Controllers
{
    public interface INewsFeedController
    {
        Task<IActionResult> GetFeedAsync(int page);
    }
}
