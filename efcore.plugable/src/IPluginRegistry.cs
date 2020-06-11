using System.Collections.Generic;
using System.Linq;

namespace EFCore.Plugable
{
    public interface IPluginRegistry
    {
        void RegisterPlugin(IPluginConfig plugin);
        IEnumerable<IPluginConfig> GetRegisteredPlugins();
    }

    internal class PluginRegistry : IPluginRegistry
    {
        private readonly List<IPluginConfig> plugins;

        public PluginRegistry()
        {
            plugins = new List<IPluginConfig>();
        }

        public PluginRegistry(IEnumerable<IPluginConfig> plugins)
        {
            this.plugins = plugins.ToList();
        }

        public IEnumerable<IPluginConfig> GetRegisteredPlugins()
        {
            return plugins;
        }

        public void RegisterPlugin(IPluginConfig plugin)
        {
            plugins.Add(plugin);
        }
    }
}