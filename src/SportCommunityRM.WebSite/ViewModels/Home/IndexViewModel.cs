using ReflectionIT.Mvc.Paging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SportCommunityRM.WebSite.ViewModels.Home
{
    public class IndexViewModel
    {
        public IEnumerable<Team> Teams { get; set; }

        public PagingList<Models.UserActivity> Activities { get; set; }

        public class Team
        {
            public Guid Id { get; set; }

            [Display(Name = "Name")]
            public string Name { get; set; }
        }
    }
}
