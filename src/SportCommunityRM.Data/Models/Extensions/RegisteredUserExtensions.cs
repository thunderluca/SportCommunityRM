using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SportCommunityRM.Data.Models
{
    public static class RegisteredUserExtensions
    {
        public static RegisteredUser WithUserId(this IQueryable<RegisteredUser> source, string userId)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.SingleOrDefault(ru => ru.AspNetUserId == userId);
        }
    }
}
