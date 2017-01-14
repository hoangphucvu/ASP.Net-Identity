using System.Web.Mvc;

namespace Identity
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilter(GlobalFilterCollection filter)
        {
            filter.Add(new HandleErrorAttribute());
            filter.Add(new AuthorizeAttribute());
        }
    }
}