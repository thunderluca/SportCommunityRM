using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace System.Linq
{
    public static class LinqExtensions
    {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> source)
        {
            return source == null || source.Count() == 0;
        }
    }
}
