using System.ComponentModel.DataAnnotations;

namespace SportCommunityRM.WebSite.ViewModels.Manage
{
    public class IndexViewModel
    {
        public string Username { get; set; }

        public bool IsEmailConfirmed { get; set; }

        public string PictureUrl { get; set; }

        [Display(Name = "Picture")]
        public string CroppedImage { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Display(Name = "Birth date")]
        public string BirthDate { get; set; }

        public string StatusMessage { get; set; }
    }
}
