using SportCommunityRM.WebSite.ViewModels.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SportCommunityRM.WebSite.ViewModels.Home
{
    public class IndexViewModel
    {
        public IEnumerable<Team> Teams { get; set; }

        public ActivitiesViewModel Activities { get; set; }

        public CalendarViewModel Calendar { get; set; }

        public NewsFeedViewModel NewsFeed { get; set; }

        public class Team
        {
            public Guid Id { get; set; }

            [Display(Name = "Name")]
            public string Name { get; set; }
        }
    }
}
