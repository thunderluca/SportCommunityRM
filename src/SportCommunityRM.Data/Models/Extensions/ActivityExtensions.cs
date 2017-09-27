using System;
using System.Linq;

namespace SportCommunityRM.Data.Models
{
    public static class ActivityExtensions
    {
        public static IQueryable<Activity> Competitions(this IQueryable<Activity> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Where(activity => activity is Match || activity is Tournament);
        }
    }
}
