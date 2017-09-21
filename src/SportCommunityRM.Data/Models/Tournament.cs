using System.Collections.Generic;

namespace SportCommunityRM.Data.Models
{
    public class Tournament : Activity
    {
        private ICollection<Match> _matches;
        public virtual ICollection<Match> Matches
        {
            get
            {
                if (_matches == null)
                    _matches = new HashSet<Match>();
                return _matches;
            }
            set { _matches = value; }
        }
    }
}
