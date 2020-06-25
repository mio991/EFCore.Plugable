using EFCore.Plugable;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using BasePlugin;

namespace DerivedPlugin
{
    public class DerivedPluginContext : PlugableContext
    {
        public DerivedPluginContext() : base(
            new List<IPluginConfig> { new BasePlugin.BasePlugin() ,new DerivedPlugin()}, services => services.AddEntityFrameworkSqlServer(), new DbContextOptionsBuilder().UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=BasePlugin").Options
            )
        {

        }
    }
}
