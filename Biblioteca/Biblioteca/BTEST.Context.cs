﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Biblioteca
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class BTESTEntities : DbContext
    {
        public BTESTEntities()
            : base("name=BTESTEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<AUTOR> AUTOR { get; set; }
        public virtual DbSet<CARTE> CARTE { get; set; }
        public virtual DbSet<CITITOR> CITITOR { get; set; }
        public virtual DbSet<GEN> GEN { get; set; }
        public virtual DbSet<IMPRUMUT> IMPRUMUT { get; set; }
        public virtual DbSet<REVIEW> REVIEW { get; set; }
    }
}
