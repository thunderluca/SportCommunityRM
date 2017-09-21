using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportCommunityRM.WebSite.Models
{
    public class UserSearchRequest
    {
        public string Filter { get; set; }

        public int? MinBirthYear { get; set; }

        public int? MaxBirthYear { get; set; }

        public Guid[] IdsToExclude { get; set; }
    }
}
