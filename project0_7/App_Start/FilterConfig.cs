using System.Web;
using System.Web.Mvc;
using project0_7.App_Start;
namespace project0_7
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            //filters.Add(new CustomFilter());
        }
    }
}
