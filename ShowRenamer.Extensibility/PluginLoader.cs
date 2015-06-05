using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace ShowRenamer.Extensibility
{
    /// <summary>
    /// Simple plugin loader that will load visible classes that implement <see cref="IPlugin"/> or a subtype.
    /// </summary>
    public class PluginLoader
    {
        /// <summary>
        /// Load all plugins in the current <see cref="AppDomain"/>
        /// </summary>
        /// <returns>A collection of loaded plugins.</returns>
        public static ICollection<IPlugin> LoadPlugins()
        {
            Type pluginParentType = typeof(IPlugin);
            IEnumerable<Type> visiblePlugins = AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes()).Where(p => pluginParentType.IsAssignableFrom(p));
            Debug.WriteLine($"{visiblePlugins.Count()} visible plugins.");
            ICollection<IPlugin> loadedPlugins = new List<IPlugin>();
            foreach(Type pluginType in visiblePlugins)
            {
                if (pluginType == typeof(IPlugin))
                {
                    Debug.WriteLine("IPlugin init skipped.");
                    continue;
                }
                try
                {
                    Debug.WriteLine($"Initialising plugin {pluginType.Name} from {pluginType.Assembly.FullName}");
                    IPlugin pluginInstance = (IPlugin)Activator.CreateInstance(pluginType);
                    Debug.WriteLine($"Plugin {pluginType.Name} loaded.");
                    loadedPlugins.Add(pluginInstance);
                }
                catch(TargetInvocationException)
                {
                    Console.WriteLine("Failed to instantiate plugin {0} from assembly {1}.", pluginType.Name, pluginType.Assembly.FullName);
                }
                catch(MissingMethodException)
                {
                    Console.WriteLine("Failed to instantiate plugin {0} from assembly {1}; no public constructor.", pluginType.Name, pluginType.Assembly.FullName);
                }
            }
            Debug.WriteLine($"Loaded {loadedPlugins.Count} plugins.");
            return loadedPlugins;
        }

        /// <summary>
        /// Load file name providers from the given collection of plugins
        /// </summary>
        /// <param name="plugins">A collection of plugins to load providers from</param>
        /// <returns>All loadable instances of <see cref="IFileNameProvider"/></returns>
        public static IEnumerable<IFileNameProvider> LoadFileNameProviders(ICollection<IPlugin> plugins)
        {
            Debug.WriteLine($"Loading providers from {plugins.Count} plugins.");
            List<IFileNameProvider> allProviders = new List<IFileNameProvider>();
            // Regexes
            IEnumerable<Regex> allPluginRegexes = plugins.SelectMany(r => r.FileNameRegexes);
            Debug.WriteLine($"Found {allPluginRegexes.Count()} regexes.");
            foreach (Regex r in allPluginRegexes)
            {
                allProviders.Add(new SimpleRegexMatcher(r));
            }
            Debug.WriteLine($"{allProviders.Count} providers after regexes.");
            // Other providers
            allProviders.Concat(plugins.SelectMany(r => r.FileNameProviders));
            Debug.WriteLine($"{allProviders.Count} providers after plugin providers.");
            return allProviders;
        }

        /// <summary>
        /// Load file name providers from all loadable plugins.
        /// </summary>
        /// <returns>All loadable instances of <see cref="IFileNameProvider"/></returns>
        public static IEnumerable<IFileNameProvider> LoadFileNameProviders()
        {
            return LoadFileNameProviders(LoadPlugins());
        }
    }
}
