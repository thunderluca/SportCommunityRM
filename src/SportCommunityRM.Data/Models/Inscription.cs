using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SportCommunityRM.Data.Models
{
    public class Inscription : BaseDocumentEntity
    {
        public decimal? AmountDue { get; set; }

        public decimal? PartialAmount { get; set; }
    }
}
