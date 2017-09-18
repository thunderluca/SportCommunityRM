using SportCommunityRM.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SportCommunityRM.Data.ReadModel
{
    public interface IDatabase
    {
        IQueryable<Inscription> Inscriptions { get; }

        IQueryable<MedicalCertificate> MedicalCertificates { get; }

        IQueryable<RegisteredUser> RegisteredUsers { get; }

        IQueryable<Team> Teams { get; }
    }
}
