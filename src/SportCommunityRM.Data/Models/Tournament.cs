using System;
using System.Collections.Generic;
using System.Text;

namespace SportCommunityRM.Data.Models
{
    public class Tournament : Activity
    {
        public virtual ICollection<Match> Matches { get; set; }
    }
}
