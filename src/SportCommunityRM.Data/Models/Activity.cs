using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SportCommunityRM.Data.Models
{
    public abstract class Activity : BaseContextEntity
    {
        public string Name { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        [ForeignKey(nameof(Field))]
        public Guid? FieldId { get; set; }

        public virtual Field Field { get; set; }

        public string Notes { get; set; }
    }
}
