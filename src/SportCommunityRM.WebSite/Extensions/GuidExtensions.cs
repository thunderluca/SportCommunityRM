using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace System
{
    public static class GuidExtensions
    {
        public static bool IsNullOrEmpty(this Guid guid)
        {
            return guid == null || guid == Guid.Empty;
        }

        public static bool IsNullOrEmpty(this Guid? nullableGuid)
        {
            return !nullableGuid.HasValue || nullableGuid.GetValueOrDefault() == Guid.Empty;
        }
    }
}
