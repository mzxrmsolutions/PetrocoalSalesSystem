using PatrocoalSalesSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PatrocoalSalesSystem.Controllers
{
    public class HomeController : Controller
    {
        
        // GET: Home
        public ActionResult Index()
        {
            if (Request.QueryString["refresh"] == "1")
                Common.ReloadData();
            Common.MyUrl = Request.RawUrl;
            if (!Common.isAuthorize())
               return Redirect("/Login");

            //User user = Common.CurrentUser;
            //DBUtil.SaveUser(user);




            // ViewBag.AllPO = Common.AllPO;
            return View();
        }
    }
}