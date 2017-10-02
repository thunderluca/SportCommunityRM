using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SportCommunityRM.WebSite.ViewModels.User
{
    public class DetailViewModel : IPermissionsViewModel
    {
        public DetailViewModel() : base(isCreateAllowed: false, isDeleteAllowed: false, isEditAllowed: false) { }

        public DetailViewModel(bool isCreateAllowed, bool isDeleteAllowed, bool isEditAllowed)
            : base(isCreateAllowed, isDeleteAllowed, isEditAllowed)
        {
        }

        public Guid Id { get; set; }

        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Display(Name = "Birth date")]
        public DateTime BirthDate { get; set; }

        public string PictureId { get; set; }

        public string BackgroundPictureId { get; set; }

        public string Name => $"{FirstName} {LastName}";

        public IEnumerable<Team> Teams { get; set; }

        public class Team
        {
            public Guid Id { get; set; }

            public string Name { get; set; }
        }
    }
}
