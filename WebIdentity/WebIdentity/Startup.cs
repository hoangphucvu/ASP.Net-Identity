using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using WebIdentity.Models;
using WebIdentity.Services;

[assembly: OwinStartup(typeof(WebIdentity.Startup))]

namespace WebIdentity
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            const string connectionString =
                @"Data Source=.;Database=AspNetIdentityDemo1;trusted_connection=yes;";
            app.CreatePerOwinContext(() => new IdentityDbContext(connectionString));
            app.CreatePerOwinContext<UserStore<IdentityUser>>(
                (opt, cont) => new UserStore<IdentityUser>(cont.Get<IdentityDbContext>()));

            //register sms 2 factor authen
            app.CreatePerOwinContext<UserManager<IdentityUser>>(
                (opt, cont) =>
                {
                    var userManager = new UserManager<IdentityUser>(cont.Get<UserStore<IdentityUser>>());
                    userManager.RegisterTwoFactorProvider("SMS", new PhoneNumberTokenProvider<IdentityUser> { MessageFormat = "Token: {0}" });
                    userManager.SmsService = new SmsService();

                    return userManager;
                });

            app.CreatePerOwinContext<SignInManager<IdentityUser, string>>(
               (opt, cont) =>
                   new SignInManager<IdentityUser, string>(cont.Get<UserManager<IdentityUser>>(), cont.Authentication));

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie
            });

            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);
        }
    }
}