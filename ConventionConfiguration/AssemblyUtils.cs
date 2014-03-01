using System;
using System.IO;
using System.Reflection;

namespace Microsoft.ConventionConfiguration
{
    public static class AssemblyUtils
    {
        public static string GetAssemblyDirectory()
        {
            var codeBase = Assembly.GetExecutingAssembly().CodeBase;
            var uri = new UriBuilder(codeBase);
            var path = Uri.UnescapeDataString(uri.Path);
            return Path.GetDirectoryName(path);
        }
    }
}
