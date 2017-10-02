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

        public bool IsCreateAllowed { get; }

        public bool IsEditAllowed { get; }

        public bool IsDeleteAllowed { get; }
    }
}
