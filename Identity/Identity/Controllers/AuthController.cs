using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Identity.Models;

namespace Identity.Controllers
{
    [AllowAnonymous]
    public class AuthController : Controller
    {
        public ActionResult LogIn(string returnUrl)
        {
            var model = new LogInModel
            {
                ReturnUrl = returnUrl
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult LogIn(LogInModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (model.Email == "admin@admin.com" && model.Password == "password")
            {
                //store user info in claim
                var identity = new ClaimsIdentity(new[]
                {
                  new Claim(ClaimTypes.Name,"Tony"),
                  new Claim(ClaimTypes.Email,"ngohungphuc95@gmail.com"),
                  new Claim(ClaimTypes.Country,"England")
                }, "ApplicationCookie");

                // get obtain an IAuthenticationManager instance from the current OWIN context
                var ctx = Request.GetOwinContext();

                var authManager = ctx.Authentication;

                //passing the claims identity. This sets the authentication cookie on the client
                authManager.SignIn(identity);

                return Redirect(GetRedirectUrl(model.ReturnUrl));
            }

            // user authN failed
            ModelState.AddModelError("", "Invalid email or password");
            return View();
        }

        private string GetRedirectUrl(string returnUrl)
        {
            if (string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
            {
                return Url.Action("index", "home");
            }

            return returnUrl;
        }
    }
}