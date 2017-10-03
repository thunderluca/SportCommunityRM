using System.Runtime.InteropServices;
using Microsoft.Extensions.Configuration;

namespace SportCommunityRM.WebSite
{
    public static class StartupExtensions
    {
        public static string GetConnectionStringByOS(this IConfiguration configuration)
        {
            var isWindowsOS = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            if (isWindowsOS)
                return configuration.GetConnectionString("DefaultWinConnection");

            return configuration.GetConnectionString("DefaultUnixConnection");
        }
    }
}