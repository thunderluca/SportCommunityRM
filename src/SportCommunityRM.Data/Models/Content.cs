using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportCommunityRM.Data.Models
{
    public class Content : BaseContextEntity
    {
        public string Title { get; set; }

        public string Caption { get; set; }

        public string Body { get; set; }

        public string PictureId { get; set; }

        public string PictureUrl { get; set; }

        [ForeignKey(nameof(Author))]
        public Guid? AuthorId { get; set; }

        public virtual RegisteredUser Author { get; set; }

        public DateTime PublicationDate { get; set; }

        public DateTime? LastModifiedDate { get; set; }

        public bool IsPinned { get; set; }
    }
}
