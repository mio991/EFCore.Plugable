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
        private readonly Type contextType;
        private ModelSnapshot modelSnapshot;

        public PlugableMigrationsAssembly(IPluginRegistry registry, Type contextType)
        {
            this.migrations = registry.GetRegisteredPlugins()
                    .SelectMany(plugin =>
                    {
                        return plugin.GetType().Assembly.GetExportedTypes()
                            .Where(t => t.GetCustomAttribute<MigrationAttribute>() != null)
                            .Select(t => t.GetTypeInfo());
                    });

            this.contextType = contextType;
        }

        public IReadOnlyDictionary<string, TypeInfo> Migrations
        {
            get
            {
                return migrations
                    .ToDictionary(m => m.GetCustomAttribute<MigrationAttribute>().Id);
            }
        }

        public ModelSnapshot ModelSnapshot
        {
            get
            {
                return modelSnapshot
                ??= (from t in Assembly.DefinedTypes.Where(
                t => !t.IsAbstract
                    && !t.IsGenericTypeDefinition)
                     where t.IsSubclassOf(typeof(ModelSnapshot))
                         && t.GetCustomAttribute<DbContextAttribute>()?.ContextType == contextType
                     select (ModelSnapshot)Activator.CreateInstance(t.AsType()))
                .FirstOrDefault();
            }
        }

        public Assembly Assembly
        {
            get
            {
                return contextType.Assembly;
            }
        }

        public Migration CreateMigration(TypeInfo migrationClass, string activeProvider)
        {
            return (Migration)Activator.CreateInstance(migrationClass);
        }

        public string FindMigrationId(string nameOrId)
        {
            return migrations.SingleOrDefault(ti =>
                ti.GetCustomAttribute<MigrationAttribute>().Id == nameOrId
                || ti.Name == nameOrId
            )?.GetCustomAttribute<MigrationAttribute>().Id;
        }
    }
}