using System.Collections.Generic;

namespace SportCommunityRM.Data.Models
{
    public class Match : Activity
    {
        public string EnemyTeamName { get; set; }

        public int EnemyTeamScore { get; set; }

        public int Score { get; set; }

        private ICollection<MatchScore> _matchScores;
        public ICollection<MatchScore> MatchScores
        {
            get
            {
                if (_matchScores == null)
                    _matchScores = new HashSet<MatchScore>();
                return _matchScores;
            }
            set { _matchScores = value; }
        }
    }
}
