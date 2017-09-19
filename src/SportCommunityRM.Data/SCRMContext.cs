﻿using Microsoft.EntityFrameworkCore;
using SportCommunityRM.Data.Models;
using System;
using static SportCommunityRM.Data.Helpers.DbContextHelper;

namespace SportCommunityRM.Data
{
    public class SCRMContext : DbContext
    {
        private readonly string ConnectionString;

        public SCRMContext(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentException(
                    $"{nameof(connectionString)} cannot be null or empty or composed by white spaces.", 
                    nameof(connectionString));

            this.ConnectionString = connectionString;
        }

        public SCRMContext(DbContextOptions options) : base(options)
        {
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseSqlServer(ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var type in modelBuilder.Model.GetEntityTypes())
                modelBuilder.Entity(type.ClrType).ToTable(GetFormattedLookupTableName("SCRM", typeof(SCRMContext), type.ClrType));
        }

        public DbSet<Activity> Activities { get; set; }

        public DbSet<Field> Fields { get; set; }

        public DbSet<Inscription> Inscriptions { get; set; }

        public DbSet<Location> Addresses { get; set; }

        public DbSet<Match> Matches { get; set; }

        public DbSet<MedicalCertificate> MedicalCertificates { get; set; }

        public DbSet<RegisteredUser> RegisteredUsers { get; set; }

        public DbSet<Team> Teams { get; set; }

        public DbSet<Tournament> Tournaments { get; set; }

        public DbSet<Training> Workouts { get; set; }
    }
}