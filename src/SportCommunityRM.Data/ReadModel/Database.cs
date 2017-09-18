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

        public IQueryable<Inscription> Inscriptions
        {
            get { return this.DbContext.Inscriptions; }
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

        public void Dispose()
        {
            if (this.DbContext != null)
                this.DbContext.Dispose();
        }
    }
}
