using System;
using System.Collections.Generic;

namespace SportCommunityRM.WebSite.ViewModels.Team
{
    public class IndexViewModel : IPermissionsViewModel
    {
        public IndexViewModel(bool isCreateAllowed, bool isDeleteAllowed, bool isEditAllowed) 
            : base(isCreateAllowed, isDeleteAllowed, isEditAllowed)
        {
        }

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
