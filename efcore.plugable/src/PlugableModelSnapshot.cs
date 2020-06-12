using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EFCore.Plugable
{
    internal class PlugableModelSnapshot : ModelSnapshot
    {
        public override IModel Model
        {
            get
            {
                Console.WriteLine("lalala");
                return model;
            }
        }

        private IEnumerable<TypeInfo> migrations;
        private IModel model;

        public PlugableModelSnapshot(IEnumerable<TypeInfo> migrations)
        {
            this.migrations = migrations;
        }

        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            Migration last = (Migration)Activator.CreateInstance(
                migrations.OrderBy(ti => ti.GetCustomAttribute<MigrationAttribute>().Id).Last().GetType()
                );

            foreach (var name in last.TargetModel.GetEntityTypes().Select(t => t.Name))
            {
                Console.WriteLine(name);
            }

            this.model = last.TargetModel;
        }
    }
}