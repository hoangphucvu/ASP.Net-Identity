using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;

namespace WebIdentity.Models
{
    public class ExtendedUser : IdentityUser
    {
        public ExtendedUser()
        {
            Addresses = new List<Address>();
        }

        public string FullName { get; set; }
        public virtual ICollection<Address> Addresses { get; private set; }
    }
}