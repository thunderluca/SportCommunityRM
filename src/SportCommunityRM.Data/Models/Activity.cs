using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

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

        [ForeignKey(nameof(Team))]
        public Guid? TeamId { get; set; }

        public virtual Team Team { get; set; }

        private ICollection<Media> _medias;
        public ICollection<Media> Medias
        {
            get
            {
                if (_medias == null)
                    _medias = new HashSet<Media>();
                return _medias;
            }
            set { _medias = value; }
        }
    }
}
