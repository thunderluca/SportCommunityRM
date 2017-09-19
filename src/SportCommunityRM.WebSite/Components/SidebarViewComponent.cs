using System;
using Microsoft.AspNetCore.Mvc;
using SportCommunityRM.Data.ReadModel;

namespace SportCommunityRM.WebSite.Components
{
    public class SidebarViewComponent : ViewComponent
    {
        private readonly IDatabase Database;

        public SidebarViewComponent(IDatabase database)
        {
            this.Database = database ?? throw new ArgumentNullException(nameof(database));
        }

        public IViewComponentResult Invoke()
        {
            return View();
        }

        public class Model
        {

        }
    }
}
