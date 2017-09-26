using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportCommunityRM.WebSite.ViewModels.User
{
    public class DetailViewModel
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PictureUrl { get; set; }

        public string Name => $"{FirstName} {LastName}";

        public DateTime BirthDate { get; set; }

        public IEnumerable<Team> Teams { get; set; }

        public class Team
        {
            public Guid Id { get; set; }

            public string Name { get; set; }
        }
    }
}
