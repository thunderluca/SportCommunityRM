using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SportCommunityRM.Data.Models
{
    public class Team : BaseContextEntity
    {
        [Required]
        public string Name { get; set; }

        public int? MinBirthYear { get; set; }

        public int? MaxBirthYear { get; set; }

        public virtual ICollection<RegisteredUserTeam> Players { get; set; }

        public virtual ICollection<TeamCoach> Coaches { get; set; }

        public virtual ICollection<Activity> Calendar { get; set; }
    }
}
