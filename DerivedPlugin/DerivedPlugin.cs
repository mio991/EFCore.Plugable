using EFCore.Plugable;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace DerivedPlugin
{
    public class DerivedPlugin : IPluginConfig
    {

        public void OnModelCreation(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AnotherEntity>(ae =>
            {
                ae.HasKey(e => e.Id);

                ae.HasOne(e => e.BaseEntity)
                    .WithMany();
            });
        }
    }
}
