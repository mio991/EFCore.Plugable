using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.Plugable.Test
{
    ///<sumary>
    /// This is needed to create Migrations via the CLI
    ///</sumary>
    public class SimpleContext : PlugableContext
    {
        public SimpleContext() : base(
            new List<IPluginConfig> { new SimplePlugin() },
                services => services.AddEntityFrameworkSqlite(),
            new DbContextOptionsBuilder()
                .UseSqlite("Data Source=test.db")
                .Options
            )
        { }
    }
}