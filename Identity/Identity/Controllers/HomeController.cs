using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace Identity.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            //Accessing custom claim data
            ViewBag.Country = claimsIdentity.FindFirst(ClaimTypes.Country).Value;
            return View();
        }
    }
}