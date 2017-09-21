using System;

namespace SportCommunityRM.WebSite.Models
{
    public class UserSearchResult
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Name => $"{FirstName} {LastName}";

        public string FullInfo => $"{Name} ({BirthDate.ToShortDateString()})";

        public DateTime BirthDate { get; set; }
    }
}
