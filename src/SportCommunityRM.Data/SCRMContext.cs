using Microsoft.EntityFrameworkCore;
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

            modelBuilder.Entity<MatchScore>()
                .HasKey(ms => new { ms.RegisteredUserId, ms.MatchId });

            modelBuilder.Entity<MatchScore>()
                .HasOne(ms => ms.RegisteredUser)
                .WithMany(ru => ru.MatchScores)
                .HasForeignKey(ms => ms.RegisteredUserId);

            modelBuilder.Entity<MatchScore>()
                .HasOne(ms => ms.Match)
                .WithMany(m => m.MatchScores)
                .HasForeignKey(ms => ms.MatchId);

            modelBuilder.Entity<RegisteredUserTeam>()
                .HasKey(rut => new { rut.RegisteredUserId, rut.TeamId });

            modelBuilder.Entity<RegisteredUserTeam>()
                .HasOne(rut => rut.Team)
                .WithMany(t => t.Players)
                .HasForeignKey(rut => rut.TeamId);

            modelBuilder.Entity<RegisteredUserTeam>()
                .HasOne(rut => rut.RegisteredUser)
                .WithMany(r => r.Teams)
                .HasForeignKey(rut => rut.RegisteredUserId);

            modelBuilder.Entity<TeamCoach>()
                .HasKey(tc => new { tc.TeamId, tc.CoachId });

            modelBuilder.Entity<TeamCoach>()
                .HasOne(tc => tc.Coach)
                .WithMany(t => t.Teams)
                .HasForeignKey(tc => tc.CoachId);

            modelBuilder.Entity<TeamCoach>()
                .HasOne(tc => tc.Team)
                .WithMany(t => t.Coaches)
                .HasForeignKey(tc => tc.TeamId);
        }

        public DbSet<Activity> Activities { get; set; }

        public DbSet<Location> Addresses { get; set; }

        public DbSet<Article> Articles { get; set; }

        public DbSet<Coach> Coaches { get; set; }

        public DbSet<Content> Contents { get; set; }

        public DbSet<Field> Fields { get; set; }

        public DbSet<Inscription> Inscriptions { get; set; }

        public DbSet<Match> Matches { get; set; }

        public DbSet<MatchReport> MatchesReports { get; set; }

        public DbSet<MedicalCertificate> MedicalCertificates { get; set; }

        public DbSet<RegisteredUser> RegisteredUsers { get; set; }

        public DbSet<Team> Teams { get; set; }

        public DbSet<Tournament> Tournaments { get; set; }

        public DbSet<Training> Workouts { get; set; }
    }
}
