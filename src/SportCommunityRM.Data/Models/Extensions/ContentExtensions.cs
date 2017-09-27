using System;
using System.Linq;

namespace SportCommunityRM.Data.Models
{
    public static class ContentExtensions
    {
        public static IQueryable<Content> Pinned(this IQueryable<Content> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Where(content => content.IsPinned);
        }

        public static IQueryable<Content> NotPinned(this IQueryable<Content> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Where(content => !content.IsPinned);
        }
    }
}
