using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SportCommunityRM.Data.Models
{
    public class MatchReport : Content
    {
        [ForeignKey(nameof(Match))]
        public Guid? MatchId { get; set; }

        public virtual Match Match { get; set; }
    }
}
