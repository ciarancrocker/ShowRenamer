using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ShowRenamer.Extensibility
{
    public class PluginLoader
    {
        public static ICollection<IPlugin> LoadPlugins()
        {
            Type pluginParentType = typeof(IPlugin);
            IEnumerable<Type> visiblePlugins = AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes()).Where(p => pluginParentType.IsAssignableFrom(p));
            ICollection<IPlugin> loadedPlugins = new List<IPlugin>();
            foreach(Type pluginType in visiblePlugins)
            {
                try
                {
                    IPlugin pluginInstance = (IPlugin)Activator.CreateInstance(pluginType);
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
            return loadedPlugins;
        }

        public static IEnumerable<IFileNameProvider> LoadFileNameProviders(ICollection<IPlugin> plugins)
        {
            List<IFileNameProvider> allProviders = new List<IFileNameProvider>();
            // Regexes
            IEnumerable<Regex> allPluginRegexes = plugins.SelectMany(r => r.FileNameRegexes);
            List<SimpleRegexMatcher> allMatchers = new List<SimpleRegexMatcher>();
            foreach (Regex r in allPluginRegexes)
            {
                allMatchers.Add(new SimpleRegexMatcher(r));
            }
            allProviders.Concat(allMatchers);
            // Other providers
            allProviders.Concat(plugins.SelectMany(r => r.FileNameProviders));
            return allProviders;
        }

        public static IEnumerable<IFileNameProvider> LoadFileNameProviders()
        {
            return LoadFileNameProviders(LoadPlugins());
        }
    }
}
