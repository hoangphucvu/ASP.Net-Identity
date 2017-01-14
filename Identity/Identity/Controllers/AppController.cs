using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Identity.Claims;

namespace Identity.Controllers
{
    public abstract class AppController : Controller
    {
        public AppUser CurrentUser => new AppUser(User as ClaimsPrincipal);
    }
}