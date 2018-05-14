using MZXRM.PSS.Business;
using MZXRM.PSS.Data;
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
            Session.Clear();
            return View();
        }
        #region " HttpPost method for Authenticating User "
        [HttpPost]
        public ActionResult LoginMethod(FormCollection form)
        {

            string login = form["login"].ToString();
            string password = form["password"].ToString();

            //ADDED BY KASHIF ABBAS FOR AUTHENTICATING USER
            UserManager.AuthenticateUser(login, password);
            Boolean testBoolean = RoleManager.IsInRole("admin");
            // UserManager.Login(login, password);
            if (Common.isAuthorize())
                return Redirect(Common.MyUrl);
            //return Redirect("/Home");

            ShowErrorMessageIfExists();
            return View("~/Views/Login/Index.cshtml");

        }
        #endregion
        #region " AuthenticateUser Function if in future we want to use asynch authentication from javascript "
        public ActionResult AuthenticateUser(String LoginName, String Password)
        {
            //ADDED BY KASHIF ABBAS FOR AUTHENTICATING USER
            UserManager.AuthenticateUser(LoginName, Password);

            // UserManager.Login(login, password);
            if (Common.isAuthorize())
                return Redirect(Common.MyUrl);
            return Redirect("/Home");
        }
        #endregion
        #region " Private ShowErrorMessageIfExists Function "
        private void ShowErrorMessageIfExists()
        {
            ViewBag.Error = String.Empty;
            if (Session["User"] != null)
            {
                User objUser = (User)Session["User"];
                if (!String.IsNullOrEmpty(objUser.Remarks))
                {
                    var ErrorMessage = objUser.Remarks;
                    ViewBag.Error = ErrorMessage;
                }
                else
                {
                    Session.Abandon();
                }
            }
        }
        #endregion
    }
}