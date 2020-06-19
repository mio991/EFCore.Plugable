using System.Collections.Generic;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EFCore.Plugable
{
    /// <summary>
    /// This is the Interface all Plugins have to implement to add to the <see cref="PlugableContext"/>
    /// </summary>
    public interface IPluginConfig
    {
        /// <summary>
        /// Define your Model here.
        /// </summary>
        /// <param name="modelBuilder">The <see cref="ModelBuilder"/> from <see cref="Microsoft.EntityFrameworkCore.DbContext.OnModelCreating(ModelBuilder)"/></param>
        void OnModelCreation(ModelBuilder modelBuilder);

        /// <summary>
        /// Collect the Types for the Migrations specific to your Plugin
        /// </summary>
        /// <returns>A Collection of Migrations specific to your Plugin</returns>
        ICollection<TypeInfo> CollectMigrations();
    }
}