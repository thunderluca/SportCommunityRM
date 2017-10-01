namespace SportCommunityRM.WebSite.ViewModels
{
    public interface IPermissionsViewModel
    {
        bool IsCreateAllowed { get; set; }

        bool IsEditAllowed { get; set; }

        bool IsDeleteAllowed { get; set; }
    }
}
