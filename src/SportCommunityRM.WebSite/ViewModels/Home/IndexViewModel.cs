using System;
using System.Collections.Generic;

namespace SportCommunityRM.WebSite.ViewModels.Home
{
    public class IndexViewModel
    {
        public IEnumerable<Content> PinnedContents { get; set; }

        public IEnumerable<Content> Contents { get; set; }

        public IEnumerable<Event> WeekEvents { get; set; }

        public IEnumerable<Scorer> TopScorers { get; set; }

        public class Content
        {
            public Guid Id { get; set; }

            public string Title { get; set; }

            public string Caption { get; set; }

            public DateTime PublicationDate { get; set; }

            public string Thumbnail { get; set; }

            public ContentType Type { get; set; }
        }

        public enum ContentType
        {
            Article = 0,
            MatchReport = 1
        }

        public class Event
        {
            public Guid Id { get; set; }

            public string Name { get; set; }

            public DateTime StartDate { get; set; }

            public DateTime EndDate { get; set; }
        }

        public class Scorer
        {
            public Guid Id { get; set; }

            public string FirstName { get; set; }

            public string LastName { get; set; }

            public int Points { get; set; }
        }
    }
}
