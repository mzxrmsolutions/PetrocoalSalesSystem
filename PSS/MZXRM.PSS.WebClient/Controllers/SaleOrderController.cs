using MZXRM.PSS.Business;
using MZXRM.PSS.Common;
using MZXRM.PSS.Data;
using PatrocoalSalesSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PatrocoalSalesSystem.Controllers
{
    public class SaleOrderController : Controller
    {
        // GET: SaleOrder
        public ActionResult Index()
        {
            Common.MyUrl = Request.RawUrl;
            if (!Common.isAuthorize())
                Response.Redirect("/Login");

            string SelectedSO = ViewBag.SelectedSO = !string.IsNullOrEmpty(Request.Form["SO"]) ? Request.Form["SO"] : "my";
            //string SelectedOrigin = ViewBag.SelectedOrigin = !string.IsNullOrEmpty(Request.Form["Origin"]) ? Request.Form["Origin"] : "0";
            //string SelectedSize = ViewBag.SelectedSize = !string.IsNullOrEmpty(Request.Form["Size"]) ? Request.Form["Size"] : "0";
            //string SelectedVessel = ViewBag.SelectedVessel = !string.IsNullOrEmpty(Request.Form["Vessel"]) ? Request.Form["Vessel"] : "0";
            //string SelectedCustomer = ViewBag.SelectedCustomer = !string.IsNullOrEmpty(Request.Form["Customer"]) ? Request.Form["Customer"] : "0";

            List<SaleOrder> allSO = SaleManager.AllSOs;
            List<SaleOrder> filteredSO = new List<SaleOrder>();

            foreach (SaleOrder so in allSO)
            {
                bool include = false;
                /*switch (SelectedPO)
                {
                    default:
                    case "my":
                        if (po.Lead.Id == Common.CurrentUser.Id)
                            include = true;
                        break;
                    case "all":
                        include = true;
                        break;
                    case "inprocess":
                        if (po.Status == POStatus.InProcess)
                            include = true;
                        break;
                    case "complete":
                        if (po.Status == POStatus.Completed)
                            include = true;
                        break;
                    case "cancelled":
                        if (po.Status == POStatus.Cancelled)
                            include = true;
                        break;
                    case "new":
                        if (po.Status == POStatus.Created || po.Status == POStatus.PendingApproval)
                            include = true;
                        break;
                }
                if (include && SelectedOrigin != "0")
                {
                    if (po.Origin.Index != int.Parse(SelectedOrigin))
                        include = false;
                }
                if (include && SelectedSize != "0")
                {
                    if (po.Size.Index != int.Parse(SelectedSize))
                        include = false;
                }
                if (include && SelectedVessel != "0")
                {
                    if (po.Vessel.Index != int.Parse(SelectedVessel))
                        include = false;
                }
                if (include)*/
                    filteredSO.Add(so);
            }

            ViewBag.SOs = filteredSO;

            return View();
        }
        public ActionResult OrderDetail()
        {
            return View();
        }
        public ActionResult UpdateOrder()
        {
            return View();
        }
        #region CreateOrder
        public ActionResult CreateOrder()
        {
            Common.MyUrl = Request.RawUrl;
            if (!Common.isAuthorize())
                Response.Redirect("/Login");

             ViewBag.ThisCust = !string.IsNullOrEmpty(Request.QueryString["cust"]) ?CustomerManager.GetCustomer(Guid.Parse( Request.QueryString["cust"])) : null;
             ViewBag.Ordertype = !string.IsNullOrEmpty(Request.QueryString["type"]) ? int.Parse(Request.QueryString["type"]) : 0;
            return View();
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreateOrder(FormCollection form)
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
                string formErrors = SaleManager.ValidateCreateSOForm(values);
                if (formErrors == "")
                {
                    SaleOrder SO = SaleManager.CreateSO(values);
                    if (SO != null)
                        Response.Redirect("/SaleOrder/UpdateOrder/" + SO.SONumber);
                }
                else
                    ExceptionHandler.Warning("Validation! " + formErrors);
            }
            catch (Exception ex)
            {
                ExceptionHandler.Error("Error! " + ex.Message, ex);
            }
            ViewBag.ThisCust = !string.IsNullOrEmpty(Request.Form["Customer"]) ? CustomerManager.GetCustomer(Guid.Parse(Request.Form["Customer"])) : null;
            ViewBag.Ordertype = !string.IsNullOrEmpty(Request.Form["ordertype"]) ? int.Parse(Request.Form["ordertype"]) : 0;
            return View(form);
        }

        #endregion
        public ActionResult DODetail()
        {
            return View();
        }
        public ActionResult ApprovedSO()
        {
            return View();
        }
        public ActionResult UpdateDO()
        {
            return View();
        }
        public ActionResult DeliveryOrder()
        {
            return View();
        }
        public ActionResult CreateDO()
        {
            return View();
        }
        public ActionResult CreateDC()
        {
            return View();
        }
        public ActionResult DeliveryChalan()
        {
            return View();
        }
    }
}