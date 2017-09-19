using System;
using System.Collections.Generic;
using System.Text;

namespace SportCommunityRM.Data.Models
{
    public class Match : Activity
    {
        public string EnemyTeamName { get; set; }

        public int TeamScore { get; set; }

        public int EnemyTeamScore { get; set; }
    }
}
