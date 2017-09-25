using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportCommunityRM.WebSite.ViewModels.Shared
{
    public class CalendarViewModel : UniqueViewModel
    {
        public string DataUrl { get; set; }

        public bool Overlap { get; set; }

        public bool Editable { get; set; }

        public bool Selectable { get; set; }

        public bool DurationEditable { get; set; }
    }
}
