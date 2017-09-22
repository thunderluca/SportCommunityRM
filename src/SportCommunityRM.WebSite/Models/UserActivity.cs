using System;
using System.ComponentModel.DataAnnotations;

namespace SportCommunityRM.WebSite.Models
{
    public class UserActivity
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
