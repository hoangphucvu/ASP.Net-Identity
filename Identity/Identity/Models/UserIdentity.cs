using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Identity.Models
{
    public class UserIdentity : IdentityUser
    {
        //ASP.NET Identity makes it easy to store additional information about your users.
        //All you have to do is subclass IdentityUser and add the properties you need.
        public int Age { get; set; }

        public string Country { get; set; }
    }
}