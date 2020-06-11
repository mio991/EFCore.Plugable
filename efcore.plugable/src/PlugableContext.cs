using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Utilities;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.Plugable
{
    public class PlugableContext : DbContext
    {
        private readonly ICollection<IPluginConfig> plugins;
        private readonly Action<IServiceCollection> collectServices;

        public PlugableContext(ICollection<IPluginConfig> plugins, DbContextOptions options, Action<IServiceCollection> collectServices) : base(options)
        {
            this.plugins = plugins;
            this.collectServices = collectServices;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (IPluginConfig plugin in plugins)
            {
                plugin.OnModelCreation(modelBuilder);
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IServiceCollection services = new ServiceCollection();

            collectServices(services);

            services.AddSingleton<IPluginRegistry>(new PluginRegistry(plugins));
            services.AddScoped<IMigrationsAssembly, PlugableMigrationsAssembly>();

            optionsBuilder.UseInternalServiceProvider(services.BuildServiceProvider());
        }
    }
}
