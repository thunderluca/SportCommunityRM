namespace SportCommunityRM.Data.Models
{
    public class Match : Activity
    {
        public string EnemyTeamName { get; set; }

        public int TeamScore { get; set; }

        public int EnemyTeamScore { get; set; }
    }
}
