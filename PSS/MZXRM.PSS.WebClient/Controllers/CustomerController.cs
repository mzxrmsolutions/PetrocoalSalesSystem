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
                List<Customer> filteredCustomers = new List<Customer>();
                foreach (Customer cust in allCustomers)
                {
                    if (cust.TotalStock > 0)
                        filteredCustomers.Add(cust);
                }





                ViewBag.Customers = filteredCustomers;

            }
            catch (Exception ex)
            {
                ExceptionHandler.Error("Error! " + ex.Message, ex);
            }
            return View();
        }
        public ActionResult CustomerDetail(string id)
        {
            try
            {
                Common.MyUrl = Request.RawUrl;
                if (!Common.isAuthorize())
                    Response.Redirect("/Login");
                if (string.IsNullOrEmpty(id))
                    ExceptionHandler.Error("Error! No Customer Found");
                Guid CustId = Guid.Empty;
                if (Guid.TryParse(id, out CustId))
                {
                    Customer ThisCustomer = CustomerManager.GetCustomer(CustId);
                    ViewBag.ThisCustomer = ThisCustomer;
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.Error("Error! " + ex.Message, ex);
            }
            return View();
        }
    }
}