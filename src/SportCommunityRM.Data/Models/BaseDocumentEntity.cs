using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SportCommunityRM.Data.Models
{
    public abstract class BaseDocumentEntity
    {
        public DateTime InsertionDate { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        [ForeignKey(nameof(User))]
        public Guid? UserId { get; set; }

        public virtual RegisteredUser User { get; set; }
    }
}
