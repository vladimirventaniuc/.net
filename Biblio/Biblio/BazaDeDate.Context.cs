﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Biblio
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class BazaDeDateContainer : DbContext
    {
        public BazaDeDateContainer()
            : base("name=BazaDeDateContainer")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Gen> GenSet { get; set; }
        public virtual DbSet<Autor> AutorSet { get; set; }
        public virtual DbSet<Carte> CarteSet { get; set; }
        public virtual DbSet<Imprumut> ImprumutSet { get; set; }
        public virtual DbSet<Cititor> CititorSet { get; set; }
        public virtual DbSet<Review> ReviewSet { get; set; }
    }
}
