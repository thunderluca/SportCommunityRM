using System;
using System.Collections.Generic;

namespace SportCommunityRM.WebSite.ViewModels.Team
{
    public class IndexViewModel : IPermissionsViewModel
    {
        public IEnumerable<Team> Teams { get; set; }

        public bool IsCreateAllowed { get; set; }

        public bool IsEditAllowed { get; set; }

        public bool IsDeleteAllowed { get; set; }

        public class Team
        {
            public Guid Id { get; set; }

            public string Name { get; set; }

            public int PlayersCount { get; set; }

            public int CoachesCount { get; set; }
        }
    }
}
