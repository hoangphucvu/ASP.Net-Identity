using System;
using System.Collections.Generic;
using System.Linq;
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
            var userStore = new UserStore<IdentityUser>();
            var userManager = new UserManager<IdentityUser>(userStore);

            var creationResult = userManager.Create(new IdentityUser("ngohungphuc95@gmail.com"), "Tony@95");
            Console.WriteLine("Created: {0}", creationResult.Succeeded);
        }
    }
}