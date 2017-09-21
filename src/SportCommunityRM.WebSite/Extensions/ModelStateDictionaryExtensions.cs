using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;

namespace Microsoft.AspNetCore.Mvc
{
    public static class ModelStateDictionaryExtensions
    {
        public static void AddErrors(this ModelStateDictionary modelStateDictionary, IdentityResult identityResult)
        {
            if (modelStateDictionary == null)
                throw new ArgumentNullException(nameof(modelStateDictionary));

            if (identityResult == null || identityResult.Errors == null) return;

            foreach (var error in identityResult.Errors)
                modelStateDictionary.AddModelError(string.Empty, error.Description);
        }
    }
}
