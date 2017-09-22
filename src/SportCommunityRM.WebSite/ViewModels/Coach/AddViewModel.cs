using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SportCommunityRM.WebSite.ViewModels.Coach
{
    public class AddViewModel
    {
        [Display(Name = "Search user")]
        [Required]
        public Guid? Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Name => $"{FirstName} {LastName}";

        public string FullInfo => $"{Name} ({BirthDate.ToShortDateString()})";

        public DateTime BirthDate { get; set; }
    }
}
