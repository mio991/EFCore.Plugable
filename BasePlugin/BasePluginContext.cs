using EFCore.Plugable;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace BasePlugin
{
    public class BasePluginContext : PlugableContext
    {
        public BasePluginContext() : base(
            new List<IPluginConfig> { new BasePlugin()}, services => services.AddEntityFrameworkSqlServer(), new DbContextOptionsBuilder().UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=BasePlugin").Options
            )
        {

        }
    }
}
