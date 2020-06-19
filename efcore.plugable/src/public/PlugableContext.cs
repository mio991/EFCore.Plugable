using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.Plugable
{
    public class PlugableContext : DbContext
    {
        private readonly ICollection<IPluginConfig> plugins;
        private readonly Action<IServiceCollection> collectServices;

        public PlugableContext(ICollection<IPluginConfig> plugins, Action<IServiceCollection> collectServices, DbContextOptions options) : base(options)
        {
            this.plugins = plugins;
            this.collectServices = collectServices;
        }

        /// <summary>
        /// Creates the Model used by this <see cref="DbContext"/>.
        /// 
        /// 
        /// If you override this make sure you call this in your <see cref="OnModelCreating"/> Methode.
        /// </summary>
        /// <param name="modelBuilder">The <see cref="ModelBuilder"/> used to build the Model.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (IPluginConfig plugin in plugins)
            {
                plugin.OnModelCreation(modelBuilder);
            }
        }

        /// <summary>
        /// Configures the DbContext and adds Plugin related features
        /// </summary>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IServiceCollection services = new ServiceCollection();

            collectServices(services);

            services.AddSingleton<IPluginRegistry>(new PluginRegistry(plugins));
            services.AddScoped<IMigrationsAssembly, PlugableMigrationsAssembly>(s => new PlugableMigrationsAssembly(s.GetRequiredService<IPluginRegistry>(), this.GetType()));

            optionsBuilder.UseInternalServiceProvider(services.BuildServiceProvider());
        }
    }
}
