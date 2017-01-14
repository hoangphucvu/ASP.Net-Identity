using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace Identity.Claims
{
    public abstract class AppViewPage<TModel> : WebViewPage<TModel>
    {
        protected AppUser CurrentUser => new AppUser(User as ClaimsPrincipal);
    }

    public abstract class AppViewPage : AppViewPage<dynamic>
    {
    }
}