using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace IdentityFundamental
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var username = "ngohungphuc95@gmail.com";
            var password = "Tony@95";
            var userStore = new UserStore<IdentityUser>();
            var userManager = new UserManager<IdentityUser>(userStore);

            //create new user
            //var creationResult = userManager.Create(new IdentityUser("ngohungphuc95@gmail.com"), "Tony@95");
            //Console.WriteLine("Created: {0}", creationResult.Succeeded);

            //add claims to specific user
            var user = userManager.FindByName(username);
            //var claimResult = userManager.AddClaim(user.Id, new Claim("given_name", "Tony"));
            //Console.WriteLine("Claim: {0}", claimResult.Succeeded);

            var passwordCheck = userManager.CheckPassword(user, password);
            Console.WriteLine("Password : {0}", passwordCheck);
        }
    }
}