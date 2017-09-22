using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SportCommunityRM.WebSite.ViewModels.Team
{
    public class CreateViewModel
    {
        [Display(Name = "Name")]
        [Required]
        public string Name { get; set; }

        [Display(Name = "Min. Birth Year")]
        [Range(ushort.MinValue, ushort.MaxValue, ErrorMessage = "Insert a valid year")]
        public int? MinBirthYear { get; set; }

        [Display(Name = "Max. Birth Year")]
        [Range(ushort.MinValue, ushort.MaxValue, ErrorMessage = "Insert a valid year")]
        public int? MaxBirthYear { get; set; }

        [Display(Name = "Players")]
        public Player[] Players { get; set; }

        [Display(Name = "Coaches")]
        public Coach[] Coaches { get; set; }

        public class Player
        {
            public Guid Id { get; set; }

            public string FirstName { get; set; }

            public string LastName { get; set; }

            public string Name => $"{FirstName} {LastName}";

            public string FullInfo => $"{Name} ({BirthDate.ToShortDateString()})";

            public DateTime BirthDate { get; set; }

            public bool IsCaptain { get; set; }
        }

        public class Coach
        {
            public Guid Id { get; set; }

            public string FirstName { get; set; }

            public string LastName { get; set; }

            public string Name => $"{FirstName} {LastName}";

            public string FullInfo => $"{Name} ({BirthDate.ToShortDateString()})";

            public DateTime BirthDate { get; set; }

            public bool IsMainCoach { get; set; }
        }
    }
}
