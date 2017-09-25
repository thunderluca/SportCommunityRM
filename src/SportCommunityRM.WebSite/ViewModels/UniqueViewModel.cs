using System;

namespace SportCommunityRM.WebSite.ViewModels
{
    public abstract class UniqueViewModel
    {
        public UniqueViewModel()
        {
            this.Id = Guid.NewGuid();
        }

        public Guid Id { get; }
    }
}
