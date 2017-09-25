using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportCommunityRM.WebSite.Models
{
    public class CalendarEvent
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Start { get; set; }

        public string End { get; set; }

        public bool Overlap { get; set; }

        public bool Editable { get; set; }

        public bool DurationEditable { get; set; }
    }
}
