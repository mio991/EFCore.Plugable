using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EFCore.Plugable
{
    internal class PlugableMigrationsAssembly : IMigrationsAssembly
    {
        private readonly IEnumerable<TypeInfo> migrations;

        public PlugableMigrationsAssembly(IPluginRegistry registry)
        {
            this.migrations = registry.GetRegisteredPlugins()
                    .SelectMany(plugin => plugin.CollectMigrations());
        }

        public IReadOnlyDictionary<string, TypeInfo> Migrations
        {
            get
            {
                return migrations
                    .ToDictionary(m => m.GetCustomAttribute<MigrationAttribute>().Id);
            }
        }

        public ModelSnapshot ModelSnapshot => throw new System.NotImplementedException();

        public Assembly Assembly
        {
            get
            {
                return typeof(PlugableMigrationsAssembly).Assembly;
            }
        }

        public Migration CreateMigration(TypeInfo migrationClass, string activeProvider)
        {
            return (Migration)Activator.CreateInstance(migrationClass);
        }

        public string FindMigrationId(string nameOrId)
        {
            return migrations.Single(ti =>
                ti.GetCustomAttribute<MigrationAttribute>().Id == nameOrId
                || ti.Name == nameOrId
            ).GetCustomAttribute<MigrationAttribute>().Id;
        }
    }
}