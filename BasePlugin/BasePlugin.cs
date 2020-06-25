using EFCore.Plugable;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BasePlugin
{
    public class BasePlugin : IPluginConfig
    {
        public void OnModelCreation(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BaseEntity>(baseEntity =>
            {
                baseEntity.HasKey(b => b.Id);
            });
        }
    }
}
