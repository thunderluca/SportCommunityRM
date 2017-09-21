using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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

        private ICollection<Field> _fields;
        public virtual ICollection<Field> Fields
        {
            get
            {
                if (_fields == null)
                    _fields = new HashSet<Field>();
                return _fields;
            }
            set { _fields = value; }
        }
    }
}
