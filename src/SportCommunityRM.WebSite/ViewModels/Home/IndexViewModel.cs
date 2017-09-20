using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportCommunityRM.WebSite.ViewModels.Home
{
    public class IndexViewModel
    {
        public IEnumerable<Team> RegisteredUserTeams { get; set; }

        public class Team
        {
            public Guid Id { get; set; }

            public string Name { get; set; } 
        }
    }
}
