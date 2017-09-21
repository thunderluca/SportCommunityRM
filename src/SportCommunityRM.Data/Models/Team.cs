using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SportCommunityRM.Data.Models
{
    public class Team : BaseContextEntity
    {
        [Required]
        public string Name { get; set; }

        public int? MinBirthYear { get; set; }

        public int? MaxBirthYear { get; set; }

        private ICollection<RegisteredUserTeam> _players;
        public virtual ICollection<RegisteredUserTeam> Players
        {
            get
            {
                if (_players == null)
                    _players = new HashSet<RegisteredUserTeam>();
                return _players;
            }
            set { _players = value; }
        }

        private ICollection<TeamCoach> _coaches;
        public virtual ICollection<TeamCoach> Coaches
        {
            get
            {
                if (_coaches == null)
                    _coaches = new HashSet<TeamCoach>();
                return _coaches;
            }
            set { _coaches = value; }
        }

        private ICollection<Activity> _calendar;
        public virtual ICollection<Activity> Calendar
        {
            get
            {
                if (_calendar == null)
                    _calendar = new HashSet<Activity>();
                return _calendar;
            }
            set { _calendar = value; }
        }
    }
}
