using ReflectionIT.Mvc.Paging;
using System;
using System.ComponentModel.DataAnnotations;

namespace SportCommunityRM.WebSite.ViewModels.Shared
{
    public class NewsFeedViewModel
    {
        [UIHint(nameof(NewsFeed))]
        public PagingList<Content> NewsFeed { get; set; }

        public class Content
        {
            public Guid Id { get; set; }

            public string Title { get; set; }

            public string Caption { get; set; }

            public DateTime PublicationDate { get; set; }

            public ContentType Type { get; set; }
        }

        public enum ContentType
        {
            Article = 0,
            MatchReport = 1
        }
    }
}
