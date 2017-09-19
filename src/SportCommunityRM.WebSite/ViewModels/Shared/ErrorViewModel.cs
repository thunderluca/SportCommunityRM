using System;

namespace SportCommunityRM.WebSite.ViewModels.Shared
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}