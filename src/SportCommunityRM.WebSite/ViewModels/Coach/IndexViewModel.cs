using ReflectionIT.Mvc.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportCommunityRM.WebSite.ViewModels.Coach
{
    public class IndexViewModel
    {
        public PagingList<Coach> Coaches { get; set; }

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
