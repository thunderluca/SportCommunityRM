using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace SportCommunityRM.WebSite.Controllers
{
    public interface IActivityController
    {
        Task<IActionResult> GetActivitiesAsync(string filter, int page, string sortExpression);

        Task<IActionResult> GetCalendarAsync(DateTime? startDate, DateTime? endDate);
    }
}
