using Microsoft.AspNetCore.Mvc;
using SportCommunityRM.WebSite.Models;
using System.Threading.Tasks;

namespace SportCommunityRM.WebSite.Controllers
{
    public interface IActivityController
    {
        Task<IActionResult> GetActivitiesAsync(string filter, int page, string sortExpression);

        Task<CalendarEvent[]> GetCalendarEventsAsync(CalendarEventsRequest request);
    }
}
