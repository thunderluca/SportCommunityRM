namespace SportCommunityRM.Data.Models
{
    public class Inscription : BaseDocumentEntity
    {
        public decimal? AmountDue { get; set; }

        public decimal? PartialAmount { get; set; }
    }
}
