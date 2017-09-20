using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

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

        [Required]
        public string AspNetUserId { get; set; }

        public virtual ICollection<MedicalCertificate> MedicalCertificatesHistory { get; set; }

        public virtual ICollection<Inscription> InscriptionsHistory { get; set; }

        //[System.ComponentModel.DataAnnotations.Schema.NotMapped]
        public virtual ICollection<RegisteredUserTeam> Teams { get; set; }
    }
}
