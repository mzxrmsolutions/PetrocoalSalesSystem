using MZXRM.PSS.Business;
using MZXRM.PSS.Common;
using MZXRM.PSS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PatrocoalSalesSystem.Controllers
{
    public class CustomerController : Controller
    {
        public ActionResult Index()
        {
            try
            {
                Common.MyUrl = Request.RawUrl;
                if (!Common.isAuthorize())
                    Response.Redirect("/Login");
                List<Customer> allCustomers = CustomerManager.AllCustomers;

                ViewBag.Customers = allCustomers;

            }
            catch (Exception ex)
            {
                ExceptionHandler.Error("Error! " + ex.Message, ex);
            }
                return View();
        }
    }
}