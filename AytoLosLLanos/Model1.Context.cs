﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AytoLosLLanos
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class ESTACIONESEntities : DbContext
    {
        public ESTACIONESEntities()
            : base("name=ESTACIONESEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<Estacion> Estacions { get; set; }
        public DbSet<Estacion_Propietario> Estacion_Propietario { get; set; }
        public DbSet<Tabla> Tablas { get; set; }
        public DbSet<Tipo> Tipoes { get; set; }
        public DbSet<Propietario> Propietarios { get; set; }
    }
}