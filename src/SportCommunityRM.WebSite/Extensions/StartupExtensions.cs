using System.Runtime.InteropServices;

namespace Microsoft.Extensions.Configuration
{
    public static class StartupExtensions
    {
        public static string GetConnectionStringByOS(this IConfiguration configuration)
        {
            var isMacOsPlatform = RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
            if (isMacOsPlatform)
                return configuration.GetConnectionString("DefaultMacOsConnection");

            var isLinuxPlatform = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
            if (isLinuxPlatform)
                return configuration.GetConnectionString("DefaultLinuxConnection");

            return configuration.GetConnectionString("DefaultWindowsConnection");
        }
    }
}