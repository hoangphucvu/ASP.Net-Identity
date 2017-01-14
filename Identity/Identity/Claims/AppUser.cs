using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace Identity.Claims
{
    public class AppUser : ClaimsPrincipal
    {
        public AppUser(ClaimsPrincipal principal) : base(principal)
        {
        }

        public string Name => FindFirst(ClaimTypes.Name).Value;

        public string Country => FindFirst(ClaimTypes.Country).Value;
    }
}