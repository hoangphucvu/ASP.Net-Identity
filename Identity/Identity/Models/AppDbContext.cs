using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Identity.Models
{
    public class AppDbContext : IdentityDbContext<UserIdentity>
    {
        public AppDbContext()
            : base("ASP.Net-Identity")
        {
        }
    }
}