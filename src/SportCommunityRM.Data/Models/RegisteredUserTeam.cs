using System;

namespace SportCommunityRM.Data.Models
{
    public class RegisteredUserTeam
    {
        public Guid RegisteredUserId { get; set; }

        public RegisteredUser RegisteredUser { get; set; }

        public Guid TeamId { get; set; }
        
        public Team Team { get; set; }
    }
}
