using System;
using System.Runtime.InteropServices;

namespace EveRouteOptimizer
{
    public static class DotNetRuntimeChecker
    {
        public static bool IsNet8Installed()
        {
            string version = RuntimeInformation.FrameworkDescription;
            return version.Contains(".NET") && version.Contains("8.");
        }
    }
}