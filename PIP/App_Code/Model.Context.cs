﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

public partial class PIPEntities : DbContext
{
    public PIPEntities()
        : base("name=PIPEntities")
    {
    }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
        throw new UnintentionalCodeFirstException();
    }

    public DbSet<documente> documentes { get; set; }
    public DbSet<evenimente> evenimentes { get; set; }
    public DbSet<locatie> locaties { get; set; }
    public DbSet<organizeaza> organizeazas { get; set; }
    public DbSet<participa> participas { get; set; }
    public DbSet<replici> replicis { get; set; }
    public DbSet<roluri> roluris { get; set; }
    public DbSet<tranzitii> tranzitiis { get; set; }
    public DbSet<utilizator> utilizators { get; set; }
}
