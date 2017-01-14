using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Identity.Models;
using Microsoft.AspNet.Identity;

namespace Identity.Claims
{
    public class AppUserClaimsIdentityFactory : ClaimsIdentityFactory<UserIdentity>
    {
        public override async Task<ClaimsIdentity> CreateAsync(UserManager<UserIdentity, string> manager, UserIdentity user, string authenticationType)
        {
            var identity = await base.CreateAsync(manager, user, authenticationType);
            identity.AddClaim(new Claim(ClaimTypes.Country, user.Country));

            return identity;
        }
    }
}