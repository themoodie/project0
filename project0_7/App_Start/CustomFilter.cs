using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using project0_7;
using project0_7.Models;
using project0_7.Controllers;
using System.Net;
using System.Web.Routing;

namespace project0_7.App_Start
{
    public class CustomFilter : ActionFilterAttribute, IActionFilter
    {
        void IActionFilter.OnActionExecuting(ActionExecutingContext filterContext)
        {
            Debug.WriteLine("This is the customer filter");
            //Debug.WriteLine($"User: {HttpContext.Current.Session["UserId"]}");
            //OnActionExecuting(filterContext);
            var db = new BankingEntities1();
            var query = from user in db.Customers
                        where user.Email == (string)HttpContext.Current.Session["UserId"]
                        && user.Password == (string)HttpContext.Current.Session["Password"]
                        select user;
            if(query == null)
            {
                Debug.WriteLine("Routing to login");
                RouteValueDictionary redirectTargetDictionary = new RouteValueDictionary();
                redirectTargetDictionary.Add("area", "");
                redirectTargetDictionary.Add("action", "Login");
                redirectTargetDictionary.Add("controller", "Home");
                filterContext.Result = new RedirectToRouteResult(redirectTargetDictionary);
                //filterContext.Result = new RedirectToRouteResult()

            }

        }
    }
}