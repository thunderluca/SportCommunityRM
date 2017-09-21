using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportCommunityRM.Data.Models
{
    public class Coach : BaseContextEntity
    {
        [ForeignKey(nameof(RegisteredUser))]
        public Guid RegisteredUserId { get; set; }

        public RegisteredUser RegisteredUser { get; set; }

        private ICollection<TeamCoach> _teams;
        public virtual ICollection<TeamCoach> Teams
        {
            get
            {
                if (_teams == null)
                    _teams = new HashSet<TeamCoach>();
                return _teams;
            }
            set { _teams = value; }
        }
    }
}
