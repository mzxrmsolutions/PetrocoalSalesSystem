using MZXRM.PSS.Business;
using PatrocoalSalesSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PatrocoalSalesSystem.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            //comments by zeeshan
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Login(FormCollection form)
        {
            string login = form["login"].ToString();
            string password = form["password"].ToString();
            UserUtil.Login(login, password);
            if (Common.isAuthorize())
                return Redirect(Common.MyUrl);
            return Redirect("/Home");
        }
    }
}