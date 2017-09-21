using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SportCommunityRM.Data.Models
{
    public static class BaseContextEntityExtensions
    {
        public static T WithId<T>(this IQueryable<T> source, Guid id) where T : BaseContextEntity
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.SingleOrDefault(item => item.Id == id);
        }
    }
}
