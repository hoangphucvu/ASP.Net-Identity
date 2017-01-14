using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Identity.Claims;
using Identity.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace Identity.Controllers
{
    [AllowAnonymous]
    public class AuthController : Controller
    {
        private readonly UserManager<UserIdentity> userManager;

        public AuthController() : this(Startup.UserManagerFactory.Invoke())
        {
        }

        public AuthController(UserManager<UserIdentity> userManager)
        {
            this.userManager = userManager;
        }

        public ActionResult LogIn(string returnUrl)
        {
            var model = new LogInModel
            {
                ReturnUrl = returnUrl
            };

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> LogIn(LogInModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var user = await userManager.FindAsync(model.Email, model.Password);
            if (user != null)
            {
                //store user info in claim
                //var identity = new ClaimsIdentity(new[]
                //{
                //  new Claim(ClaimTypes.Name,"Tony"),
                //  new Claim(ClaimTypes.Email,"ngohungphuc95@gmail.com"),
                //  new Claim(ClaimTypes.Country,"England")
                //}, "ApplicationCookie");
                var identity = await userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
                // get obtain an IAuthenticationManager instance from the current OWIN context
                //var ctx = Request.GetOwinContext();

                //var authManager = ctx.Authentication;

                ////passing the claims identity. This sets the authentication cookie on the client
                //authManager.SignIn(identity);
                GetAuthenticationManager().SignIn(identity);
                return Redirect(GetRedirectUrl(model.ReturnUrl));
            }

            // user authN failed
            ModelState.AddModelError("", "Invalid email or password");
            return View();
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var user = new UserIdentity
            {
                //exists prop in useridentity
                UserName = model.Email,
                //customize prop
                Country = model.Country,
                Age = model.Age
            };

            var result = await userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await SignIn(user);
                return RedirectToAction("index", "home");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }

            return View();
        }

        private async Task SignIn(UserIdentity user)
        {
            var identity = await userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            identity.AddClaim(new Claim(ClaimTypes.Country, user.Country));
            GetAuthenticationManager().SignIn(identity);
        }

        private string GetRedirectUrl(string returnUrl)
        {
            if (string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
            {
                return Url.Action("index", "home");
            }

            return returnUrl;
        }

        public ActionResult Logout()
        {
            var ctx = Request.GetOwinContext();
            var authManager = ctx.Authentication;

            authManager.SignOut("ApplicationCookie");
            return RedirectToAction("index", "home");
        }

        private IAuthenticationManager GetAuthenticationManager()
        {
            var ctx = Request.GetOwinContext();
            return ctx.Authentication;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && userManager != null)
            {
                userManager.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}