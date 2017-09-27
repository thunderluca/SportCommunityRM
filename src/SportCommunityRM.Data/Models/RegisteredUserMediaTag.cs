using System;
using System.Collections.Generic;
using System.Text;

namespace SportCommunityRM.Data.Models
{
    public class RegisteredUserMediaTag
    {
        public Guid RegisteredUserId { get; set; }

        public RegisteredUser RegisteredUser { get; set; }

        public Guid MediaId { get; set; }

        public Media Media { get; set; }
    }
}
