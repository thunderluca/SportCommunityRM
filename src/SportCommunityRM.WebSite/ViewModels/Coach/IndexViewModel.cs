using ReflectionIT.Mvc.Paging;
using System;
using System.Collections.Generic;

namespace SportCommunityRM.WebSite.ViewModels.Coach
{
    public class IndexViewModel : IPermissionsViewModel
    {
        public PagingList<Coach> Coaches { get; set; }

        public bool IsCreateAllowed { get; set; }

        public bool IsEditAllowed { get; set; }

        public bool IsDeleteAllowed { get; set; }

        public class Coach
        {
            public Guid Id { get; set; }

            public string Name { get; set; }

            public IEnumerable<Team> Teams { get; set; }
        }

        public class Team
        {
            public Guid Id { get; set; }

            public string Name { get; set; }
        }
    }
}
