using SportCommunityRM.WebSite.Models;
using System;
using System.Collections.Generic;

namespace SportCommunityRM.WebSite.Controllers
{
    public interface IUserSearchController
    {
        IEnumerable<UserSearchResult> SearchUser(UserSearchRequest request);
    }
}
