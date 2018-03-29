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

                string s = Request.QueryString["s"] != null ? Request.QueryString["s"] : "";

                List<Customer> allCustomers = CustomerManager.AllCustomers;
                List<Customer> filteredCustomers = new List<Customer>();
                foreach (Customer cust in allCustomers)
                {
                    if (s == "all")
                    {
                        filteredCustomers.Add(cust);
                        continue;
                    }
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

        #region /Customer/CreateCustomer
        public ActionResult CreateCustomer()
        {
            Common.MyUrl = Request.RawUrl;
            if (!Common.isAuthorize())
                Response.Redirect("/Login");
            return View();
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreateCustomer(FormCollection form)
        {
            try
            {
                if (!Common.isAuthorize())
                {
                    ExceptionHandler.Error("Session Timeout");
                    return View();
                }
                if (form["btn"] != null && form["btn"] == "Reset")
                    return View();

                Dictionary<string, string> values = new Dictionary<string, string>();
                foreach (string Key in form.Keys)
                {
                    values.Add(Key, form[Key]);
                }
                Customer Cust = CustomerManager.ValidateCreateForm(values);
                if (Cust != null)
                {
                    Guid createSuccess = CustomerManager.CreateCustomer(Cust);
                    if (createSuccess!=Guid.Empty)
                        Response.Redirect("/Customer/CustomerDetail/" + createSuccess);
                    else
                        ExceptionHandler.Warning("Error creating Customer");
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.Error("Error! " + ex.Message, ex);
            }
            return View(form);
        }
        #endregion

        #region /Customer/UpdateCustomer
        public ActionResult UpdateCustomer(string id)
        {
            Common.MyUrl = Request.RawUrl;
            if (!Common.isAuthorize())
                Response.Redirect("/Login");
            if (string.IsNullOrEmpty(id))
                Response.Redirect("/Purchase/CreateOrder");
            Customer ThisCust = CustomerManager.GetCustomer(new Guid(id));
            if (ThisCust == null)
                return Redirect("/Purchase/CreateOrder");
            ViewBag.ThisCust = ThisCust;
            return View();
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UpdateCustomer(FormCollection form)
        {
            if (!Common.isAuthorize())
            {
                ExceptionHandler.Error("Session Timeout");
                return View();
            }
            if (form["btn"] != null && form["btn"] == "Reset")
                return View();

            Dictionary<string, string> values = new Dictionary<string, string>();

            foreach (string Key in form.Keys)
            {
                values.Add(Key, form[Key]);
            }
            Customer cust= CustomerManager.ValidateCreateForm(values);
            if (cust != null)
            {
                if (form["btn"] != null && form["btn"] == "Save")
                {
                     CustomerManager.UpdateCustomer(cust);
                    return RedirectToAction("UpdateCustomer", new { id = cust.Id });
                }
                
            }
            return RedirectToAction("UpdateCustomer", new { id = values["cId"] });
        }
        public ActionResult CancelCustomer(string id)
        {

            Common.MyUrl = Request.RawUrl;
            if (!Common.isAuthorize())
                Response.Redirect("/Login");
            if (string.IsNullOrEmpty(id))
                Response.Redirect("/Purchase");
            try
            {
                PurchaseOrder PO = PurchaseManager.GetPO(id);
                if (PO == null)
                    return Redirect("/Purchase");
                PurchaseManager.CompleteOrder(PO);
                return RedirectToAction("OrderDetail", new { id = PO.PONumber });
            }
            catch (Exception ex)
            {
                ExceptionHandler.Error("Unable to complete order", ex);
            }
            return RedirectToAction("index");
        }
        #endregion
    }
}