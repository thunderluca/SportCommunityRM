using SportCommunityRM.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SportCommunityRM.Data.ReadModel
{
    public interface IDatabase
    {
        IQueryable<Activity> Activities { get; }

        IQueryable<Location> Addresses { get; }

        IQueryable<Article> Articles { get; }

        IQueryable<Coach> Coaches { get; }

        IQueryable<Content> Contents { get; }

        IQueryable<Field> Fields { get; }

        IQueryable<Inscription> Inscriptions { get; }

        IQueryable<Match> Matches { get; }

        IQueryable<MatchReport> MatchesReports { get; }

        IQueryable<MedicalCertificate> MedicalCertificates { get; }

        IQueryable<RegisteredUser> RegisteredUsers { get; }

        IQueryable<Team> Teams { get; }

        IQueryable<Tournament> Tournaments { get; }

        IQueryable<Training> Workouts { get; }
    }
}
