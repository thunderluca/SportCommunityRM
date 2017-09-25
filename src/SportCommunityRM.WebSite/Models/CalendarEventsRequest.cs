using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportCommunityRM.WebSite.Models
{
    public class CalendarEventsRequest
    {
        public bool Overlap { get; set; }

        public bool Editable { get; set; }

        public bool DurationEditable { get; set; }

        public Guid? TeamId { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}
