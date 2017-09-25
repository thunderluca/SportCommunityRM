using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SportCommunityRM.Data.Models
{
    public class RegisteredUser : BaseContextEntity
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public DateTime BirthDate { get; set; }

        public Sex Sex { get; set; }

        public string FiscalCode { get; set; }

        public string Address { get; set; }

        public string CivicNumber { get; set; }
        
        public string City { get; set; }

        public string PostalCode { get; set; }

        public string Country { get; set; }

        public string PictureId { get; set; }

        [Required]
        public string AspNetUserId { get; set; }

        private ICollection<MedicalCertificate> _medicalCertificatesHistory;
        public virtual ICollection<MedicalCertificate> MedicalCertificatesHistory
        {
            get
            {
                if (_medicalCertificatesHistory == null)
                    _medicalCertificatesHistory = new HashSet<MedicalCertificate>();
                return _medicalCertificatesHistory;
            }
            set { _medicalCertificatesHistory = value; }
        }

        private ICollection<Inscription> _inscriptionsHistory;
        public virtual ICollection<Inscription> InscriptionsHistory
        {
            get
            {
                if (_inscriptionsHistory == null)
                    _inscriptionsHistory = new HashSet<Inscription>();
                return _inscriptionsHistory;
            }
            set { _inscriptionsHistory = value; }
        }

        private ICollection<RegisteredUserTeam> _teams;
        public virtual ICollection<RegisteredUserTeam> Teams
        {
            get
            {
                if (_teams == null)
                    _teams = new HashSet<RegisteredUserTeam>();
                return _teams;
            }
            set { _teams = value; }
        }
    }
}
