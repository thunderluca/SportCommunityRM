using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportCommunityRM.WebSite.ViewModels.Team
{
    public class IndexViewModel
    {
        public IEnumerable<Team> Teams { get; set; }

        public class Team
        {
            public Guid Id { get; set; }

            public string Name { get; set; }

            public int PlayersCount { get; set; }

            public int CoachesCount { get; set; }
        }
    }
}
