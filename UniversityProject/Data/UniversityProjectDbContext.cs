using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite;
using UniversityProject.Classes;
//using System.Data.Entity;
namespace UniversityProject.Data
{
    class UniversityProjectDbContext : DbContext
    {
        public UniversityProjectDbContext()
        { 
        }
        public DbSet<Disease> Diseases { get; set; }
        public DbSet<DiseaseCategory> DiseasesCategories { get; set; }
        public DbSet<Images> Images { get; set; }
        public DbSet<Plant> Plants { get; set; }
        public DbSet<Specie> Species { get; set; }
        public DbSet<Symptom> Symptoms { get; set; }    
        public DbSet<DiseaseSymptom> DiseaseSymptoms { get; set; }
        public DbSet<History> Histories{ get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<DiseaseSymptom>()
                .HasKey(bc => new { bc.DiseaseId, bc.SymptomId });
            modelBuilder.Entity<DiseaseSymptom>()
                .HasOne(bc => bc.Disease)
                .WithMany(b => b.DiseaseSymptoms)
                .HasForeignKey(bc => bc.DiseaseId);
            modelBuilder.Entity<DiseaseSymptom>()
                .HasOne(bc => bc.Symptom)
                .WithMany(c => c.DiseaseSymptoms)
                .HasForeignKey(bc => bc.SymptomId);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder); 
            optionsBuilder.UseSqlite(@$"FileName={Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\plants.db");
            
        }
    }
}
