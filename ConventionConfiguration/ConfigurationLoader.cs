using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using log4net;
using Newtonsoft.Json;

namespace Microsoft.ConventionConfiguration
{
    public class ConfigurationLoader
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Looks for configuration files in the specified path, and maps those to available
        /// assemblies loaded in the current AppDomain.
        /// </summary>
        /// <remarks>
        /// Duplicate configurations will be ignored.
        /// </remarks>
        /// <param name="container"></param>
        /// <param name="configurationFilesPath"></param>
        /// <param name="classNameConvention"></param>
        public static void LoadConfigurations(StructureMap.IContainer container, string configurationFilesPath, string classNameConvention)
        {
            Log.DebugFormat("Looking for configuration files in '{0}'", configurationFilesPath);
            var configurationFiles = Directory.GetFiles(configurationFilesPath);
            foreach (var file in configurationFiles)
            {
                LoadFromConfigurationFile(container, file, classNameConvention);
            }
        }

        public static void LoadFromConfigurationFile(StructureMap.IContainer container, string file, string classNameConvention)
        {
            Log.DebugFormat("Reading configuration data from '{0}'", file);

            var fileName = Path.GetFileNameWithoutExtension(file);

            var className = string.Format(classNameConvention, fileName);

            // http://haacked.com/archive/2012/07/23/get-all-types-in-an-assembly.aspx/
            var types = new List<Type>();
            foreach (var assembly in GetAssemblies())
            {
                try
                {
                    var t = assembly.GetTypes();
                    types.AddRange(t);
                }
                catch (ReflectionTypeLoadException ex)
                {
                    types.AddRange(ex.Types);
                }
            }

            var matchingTypes = types.Where(x => x != null && x.Name == className).ToList();
            if (matchingTypes.Count == 0)
                return;
            if (matchingTypes.Count > 1)
                throw new Exception(string.Format("Multiple types found with the name '{0}'", className));

            var type = matchingTypes.First();
            if (container.TryGetInstance(type) != null)
                return;

            var configJson = File.ReadAllText(file);
            var deserialized = JsonConvert.DeserializeObject(configJson, type);

            Log.DebugFormat("Mapping configuration data in '{0}' to '{1}'", file, type.Name);

            //Note: When using Unity, naming the instance BREAKs the resolving
            container.Configure(x => x.For(type).Use(deserialized));
        }

        private static IEnumerable<Assembly> GetAssemblies()
        {
            var basePath = AppDomain.CurrentDomain.BaseDirectory;

            var extensions = new[] { ".exe", ".dll" };
            var assemblyNames = Directory.GetFiles(basePath).Where(x => extensions.Contains(Path.GetExtension(x)));
            var assemblies = new List<Assembly>();

            foreach (var assemblyName in assemblyNames)
            {
                try
                {
                    var a = Assembly.Load(Path.GetFileNameWithoutExtension(assemblyName));
                    assemblies.Add(a);
                }
                catch (BadImageFormatException)
                {
                }
                catch (FileLoadException)
                {
                }
            }

            return assemblies;
        }
    }
}
