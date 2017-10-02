namespace SportCommunityRM.WebSite.ViewModels
{
    public abstract class IPermissionsViewModel
    {
        public IPermissionsViewModel(bool isCreateAllowed, bool isDeleteAllowed, bool isEditAllowed)
        {
            this.IsCreateAllowed = isCreateAllowed;
            this.IsDeleteAllowed = isDeleteAllowed;
            this.IsEditAllowed = isEditAllowed;
        }

        bool IsCreateAllowed { get; }

        bool IsEditAllowed { get; }

        bool IsDeleteAllowed { get; }
    }
}
