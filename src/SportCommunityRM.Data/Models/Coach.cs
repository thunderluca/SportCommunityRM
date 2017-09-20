using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SportCommunityRM.Data.Models
{
    public class Coach : BaseContextEntity
    {
        [ForeignKey(nameof(RegisteredUser))]
        public Guid RegisteredUserId { get; set; }

        public RegisteredUser RegisteredUser { get; set; }

        public virtual ICollection<TeamCoach> Teams { get; set; }
    }
}
