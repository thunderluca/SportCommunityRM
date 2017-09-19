using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SportCommunityRM.Data.Models
{
    public class Location : BaseContextEntity
    {
        public string Name { get; set; }

        [Required]
        public string Address { get; set; }
        
        public string CivicNumber { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string PostalCode { get; set; }

        [Required]
        public string Country { get; set; }

        public virtual ICollection<Field> Fields { get; set; }
    }
}
