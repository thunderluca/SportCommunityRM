using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SportCommunityRM.Data.Models;

namespace SportCommunityRM.Data.ReadModel
{
    public class Database : IDatabase, IDisposable
    {
        private readonly SCRMContext DbContext;

        public Database(SCRMContext dbContext)
        {
            this.DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public IQueryable<Activity> Activities
        {
            get { return this.DbContext.Activities; }
        }

        public IQueryable<Location> Addresses
        {
            get { return this.DbContext.Addresses; }
        }

        public IQueryable<Coach> Coaches
        {
            get { return this.DbContext.Coaches; }
        }

        public IQueryable<Field> Fields
        {
            get { return this.DbContext.Fields; }
        }

        public IQueryable<Inscription> Inscriptions
        {
            get { return this.DbContext.Inscriptions; }
        }

        public IQueryable<Match> Matches
        {
            get { return this.DbContext.Matches; }
        }

        public IQueryable<MedicalCertificate> MedicalCertificates
        {
            get { return this.DbContext.MedicalCertificates; }
        }

        public IQueryable<RegisteredUser> RegisteredUsers
        {
            get { return this.DbContext.RegisteredUsers; }
        }

        public IQueryable<Team> Teams
        {
            get { return this.DbContext.Teams; }
        }

        public IQueryable<Tournament> Tournaments
        {
            get { return this.DbContext.Tournaments; }
        }

        public IQueryable<Training> Workouts
        {
            get { return this.DbContext.Workouts; }
        }

        public void Dispose()
        {
            if (this.DbContext != null)
                this.DbContext.Dispose();
        }
    }
}
