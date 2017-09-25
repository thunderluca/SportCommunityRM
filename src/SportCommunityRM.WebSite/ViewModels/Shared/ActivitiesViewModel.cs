using ReflectionIT.Mvc.Paging;
using System;
using System.ComponentModel.DataAnnotations;

namespace SportCommunityRM.WebSite.ViewModels.Shared
{
    public class ActivitiesViewModel
    {
        [UIHint(nameof(Activities))]
        public PagingList<Activity> Activities { get; set; }

        public class Activity
        {
            public Guid Id { get; set; }

            [Display(Name = "Name")]
            public string Name { get; set; }

            [Display(Name = "Start date")]
            public DateTime StartDate { get; set; }

            [Display(Name = "End date")]
            public DateTime EndDate { get; set; }
        }
    }
}
