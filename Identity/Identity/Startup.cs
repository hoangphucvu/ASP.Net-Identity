using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Identity.Claims;
using Identity.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;

namespace Identity
{
    public class Startup
    {
        public static Func<UserManager<UserIdentity>> UserManagerFactory { get; private set; }

        public void Configuration(IAppBuilder app)
        {
            //use cookie based authentication
            app.UseCookieAuthentication(new CookieAuthenticationOptions()
            {
                AuthenticationType = "ApplicationCookie",
                LoginPath = new PathString("/auth/login")
            });

            UserManagerFactory = () =>
            {
                // configure the user manager
                var userManager = new UserManager<UserIdentity>(new UserStore<UserIdentity>(new AppDbContext()));

                // allow alphanumeric characters in username
                userManager.UserValidator = new UserValidator<UserIdentity>(userManager)
                {
                    AllowOnlyAlphanumericUserNames = false
                };
                userManager.PasswordValidator = new PasswordValidator
                {
                    RequiredLength = 4,
                    RequireNonLetterOrDigit = false,
                    RequireDigit = false,
                    RequireLowercase = false,
                    RequireUppercase = false,
                };
                // use out custom claims provider
                userManager.ClaimsIdentityFactory = new AppUserClaimsIdentityFactory();
                return userManager;
            };
        }
    }
}