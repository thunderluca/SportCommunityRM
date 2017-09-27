using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportCommunityRM.Data.Models
{
    public class Media : BaseContextEntity
    {
        public string FileId { get; set; }

        public string Name { get; set; }

        [ForeignKey(nameof(Activity))]
        public Guid? ActivityId { get; set; }

        public Activity Activity { get; set; }

        private ICollection<RegisteredUserMediaTag> _taggedUsers;
        public ICollection<RegisteredUserMediaTag> TaggedUsers
        {
            get
            {
                if (_taggedUsers == null)
                    _taggedUsers = new HashSet<RegisteredUserMediaTag>();
                return _taggedUsers;
            }
            set { _taggedUsers = value; }
        }
    }
}
