using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SportCommunityRM.Data.Models
{
    public class Field : BaseContextEntity
    {
        [Required]
        public string Name { get; set; }

        [ForeignKey(nameof(Location))]
        public Guid LocationId { get; set; }

        public Location Location { get; set; }

        public virtual ICollection<Activity> BookedActivities { get; set; }
    }
}
