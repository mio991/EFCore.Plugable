using System.Collections.Generic;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EFCore.Plugable
{
    public interface IPluginConfig
    {
        void OnModelCreation(ModelBuilder modelBuilder);
        ICollection<TypeInfo> CollectMigrations();
    }
}