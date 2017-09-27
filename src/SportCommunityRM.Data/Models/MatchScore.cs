using System;

namespace SportCommunityRM.Data.Models
{
    public class MatchScore
    {
        public Guid? RegisteredUserId { get; set; }

        public RegisteredUser RegisteredUser { get; set; }

        public Guid MatchId { get; set; }

        public Match Match { get; set; }

        public int Points { get; set; }
    }
}
