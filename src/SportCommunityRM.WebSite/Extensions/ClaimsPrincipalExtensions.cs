using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace System.Security.Claims
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetUserId(this ClaimsPrincipal claimsPrincipal)
        {
            if (claimsPrincipal == null)
                throw new ArgumentNullException(nameof(claimsPrincipal));

            return claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
}
