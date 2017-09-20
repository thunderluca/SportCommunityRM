using System;
using System.Collections.Generic;
using System.Text;

namespace SportCommunityRM.Data.Models
{
    public class TeamCoach
    {
        public Guid TeamId { get; set; }

        public Team Team { get; set; }

        public Guid CoachId { get; set; }

        public Coach Coach { get; set; }
    }
}
