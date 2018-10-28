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
            ViewBag.ThisCust = !string.IsNullOrEmpty(Request.Form["Customer"]) ? CustomerManager.GetCustomer(Guid.Parse(Request.Form["Customer"])) : null;
            ViewBag.Ordertype = !string.IsNullOrEmpty(Request.Form["ordertype"]) ? int.Parse(Request.Form["ordertype"]) : 0;
            return View();
        }
        public ActionResult OrderDetail(String id)
        {
            Common.MyUrl = Request.RawUrl;
            if (!Common.isAuthorize())
                Response.Redirect("/Login");
            if (string.IsNullOrEmpty(id))
                Response.Redirect("/SaleOrder/CreateOrder");
            SaleOrder SO = SaleManager.GetSOBySONumber(id);
            if (SO == null)
                return Redirect("/SaleOrder/CreateOrder");
            ViewBag.ThisSO = SO;
            ViewBag.ThisCust = CustomerManager.GetCustomer(SO.Customer.Id);
            ViewBag.Ordertype = !string.IsNullOrEmpty(Request.Form["ordertype"]) ? int.Parse(Request.Form["ordertype"]) : 0;
            return View();
        }
        public ActionResult UpdateOrder(string id)
        {
            Common.MyUrl = Request.RawUrl;
            if (!Common.isAuthorize())
                Response.Redirect("/Login");
            if (string.IsNullOrEmpty(id))
                Response.Redirect("/SaleOrder/CreateOrder");
            SaleOrder SO = SaleManager.GetSOBySONumber(id);
            if (SO == null)
                return Redirect("/SaleOrder/CreateOrder");
            ViewBag.ThisSO = SO;
            ViewBag.ThisCust = CustomerManager.GetCustomer(SO.Customer.Id);
            ViewBag.Ordertype = !string.IsNullOrEmpty(Request.Form["ordertype"]) ? int.Parse(Request.Form["ordertype"]) : (int)SO.OrderType;
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UpdateOrder(FormCollection form)
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
                if (form["btn"] != null && form["btn"] == "Update")
                {
                    SaleOrder SO = SaleManager.UpdateSO(values);
                    return RedirectToAction("OrderDetail", new { id = SO.SONumber });
                }
                //if (form["btn"] != null && form["btn"] == "ApprovalSubmit")
                //{
                //    SaleOrder SO = SaleManager.UpdateSO(values);
                //    return RedirectToAction("OrderDetail", new { id = SO.SONumber });
                //}
                //if (form["btn"] != null && form["btn"] == "Approve")
                //{
                //    PurchaseOrder PO = PurchaseManager.ApprovePO(values);
                //    return RedirectToAction("OrderDetail", new { id = PO.PONumber });
                //}
            }
            else
            {
                ExceptionHandler.Error("Validation! " + formErrors);
            }
            return RedirectToAction("SaleOrder/OrderDetail", new { id = values["ponumber"] });
        }
        #region CreateOrder
        public ActionResult CreateOrder()
        {
            Common.MyUrl = Request.RawUrl;
            if (!Common.isAuthorize())
                Response.Redirect("/Login");

            ViewBag.ThisCust = !string.IsNullOrEmpty(Request.QueryString["cust"]) ? CustomerManager.GetCustomer(Guid.Parse(Request.QueryString["cust"])) : null;
            ViewBag.Ordertype = !string.IsNullOrEmpty(Request.QueryString["type"]) ? int.Parse(Request.QueryString["type"]) : 0;
            ViewData["Success"] = "";
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
                    {
                        ViewData["Success"] = "Sale Order Created Successfully!";
                        //Response.Redirect("/SaleOrder/UpdateOrder/" + SO.SONumber);
                        //return View("OrderDetail", new { id = SO.SONumber });
                        return RedirectToAction( "OrderDetail", new { id = SO.SONumber.ToString()});
                    }
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
        public ActionResult DODetail(String id)
        {
            Common.MyUrl = Request.RawUrl;
            if (!Common.isAuthorize())
                Response.Redirect("/Login");
            if (string.IsNullOrEmpty(id))
                Response.Redirect("/SaleOrder/Createdo");
            DeliveryOrder DO = SaleManager.GetDOByDONumber(id);
            if (DO == null)
                return Redirect("/SaleOrder/CreateOrder");
            ViewBag.ThisSO = SaleManager.GetSOById(DO.SaleOrder.Index);
            ViewBag.ThisDO = DO;
            ViewBag.ThisDC = SaleManager.GetDCByDOID(DO.Id);
            return View();
        }
        
        //added by kashif abbas starts here
        public ActionResult UpdateDO(string id)
        {
            Common.MyUrl = Request.RawUrl;
            if (!Common.isAuthorize())
                Response.Redirect("/Login");
            if (string.IsNullOrEmpty(id))
                Response.Redirect("/SaleOrder/Createdo");
            DeliveryOrder DO = SaleManager.GetDOByDONumber(id);
            if (DO == null)
                return Redirect("/SaleOrder/CreateOrder");
            ViewBag.ThisSO = SaleManager.GetSOById(DO.SaleOrder.Index);
            ViewBag.ThisDO = DO;
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UpdateDO(FormCollection form)
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
                string formErrors = SaleManager.ValidateCreateDOForm(values);
                if (formErrors == "")
                {
                    DeliveryOrder DO = SaleManager.UpdateDO(values);
                    if (DO != null)
                       Response.Redirect("/SaleOrder/DODetail/" + DO.DONumber);
                }
                else
                    ExceptionHandler.Warning("Validation! " + formErrors);
            }
            catch (Exception ex)
            {
                ExceptionHandler.Error("Error! " + ex.Message, ex);
            }
            return View(form);
        }
        //added by kashif abbas ends here
        public ActionResult DeliveryOrder()
        {
            return View();
        }
       
        //added by kashif abbas 
        public ActionResult CompleteDO(string id)
        {

            Common.MyUrl = Request.RawUrl;
            if (!Common.isAuthorize())
                Response.Redirect("/Login");
            if (string.IsNullOrEmpty(id))
                Response.Redirect("/SaleOrder");
            try
            {
                DeliveryOrder DO = SaleManager.GetDOByDONumber(id);
                if (DO == null)
                    return Redirect("/SaleOrder");
                SaleManager.CompleteDO(DO);
                return RedirectToAction("DODetail", new { id = DO.DONumber });
            }
            catch (Exception ex)
            {
                ExceptionHandler.Error("Unable to complete order", ex);
            }
            return RedirectToAction("index");
        }
        public ActionResult ApproveDO(string id)
        {

            Common.MyUrl = Request.RawUrl;
            if (!Common.isAuthorize())
                Response.Redirect("/Login");
            if (string.IsNullOrEmpty(id))
                Response.Redirect("/SaleOrder/CreateOrder");
            try
            {
                DeliveryOrder DO = SaleManager.GetDOByDONumber(id);
                if (DO == null)
                    return Redirect("/SaleOrder/CreateOrder");
                SaleManager.ApproveDO(DO);
                return RedirectToAction("DODetail", new { id = DO.DONumber });
            }
            catch (Exception ex)
            {
                ExceptionHandler.Error("Unable to complete order", ex);
            }
            return RedirectToAction("index");
        }

        public ActionResult StopDOLoading(string id)
        {

            Common.MyUrl = Request.RawUrl;
            if (!Common.isAuthorize())
                Response.Redirect("/Login");
            if (string.IsNullOrEmpty(id))
                Response.Redirect("/SaleOrder/CreateOrder");
            try
            {
                DeliveryOrder DO = SaleManager.GetDOByDONumber(id);
                if (DO == null)
                    return Redirect("/SaleOrder/CreateOrder");
                SaleManager.StopDOLoading(DO);
                return RedirectToAction("DODetail", new { id = DO.DONumber });
            }
            catch (Exception ex)
            {
                ExceptionHandler.Error("Unable to complete order", ex);
            }
            return RedirectToAction("index");
        }

        public ActionResult StartDOLoading(string id)
        {

            Common.MyUrl = Request.RawUrl;
            if (!Common.isAuthorize())
                Response.Redirect("/Login");
            if (string.IsNullOrEmpty(id))
                Response.Redirect("/SaleOrder/CreateOrder");
            try
            {
                DeliveryOrder DO = SaleManager.GetDOByDONumber(id);
                if (DO == null)
                    return Redirect("/SaleOrder/CreateOrder");
                SaleManager.StartDOLoading(DO);
                return RedirectToAction("DODetail", new { id = DO.DONumber });
            }
            catch (Exception ex)
            {
                ExceptionHandler.Error("Unable to complete order", ex);
            }
            return RedirectToAction("index");
        }



        #region " CreateDO ActionResult "

        public ActionResult CreateDO()
        {
            //ViewBag.ThisSO
            Common.MyUrl = Request.RawUrl;
            if (!Common.isAuthorize())
                Response.Redirect("/Login");
            String SONubmer = !string.IsNullOrEmpty(Request.QueryString["so"]) ? Request.QueryString["so"] : String.Empty;
           
            if (!String.IsNullOrEmpty(SONubmer))
            {
                SaleOrder SO = null;
                int i;
                bool success = int.TryParse(SONubmer, out i);
                if (success)
                {
                    SO = SaleManager.GetSOById(int.Parse(SONubmer));
                }
                else
                {
                    SO = SaleManager.GetSOBySONumber(SONubmer);
                }
                ViewBag.ThisSO = SO;


            }
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreateDO(FormCollection form)

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
                string formErrors = SaleManager.ValidateCreateDOForm(values);
                if (formErrors == "")
                {
                    DeliveryOrder DO = SaleManager.CreateDO(values);
                    if (DO != null)
                        Response.Redirect("/SaleOrder/orderdetail/" + DO.SaleOrder.Value);
                }
                else
                    ExceptionHandler.Warning("Validation! " + formErrors);
            }
            catch (Exception ex)
            {
                ExceptionHandler.Error("Error! " + ex.Message, ex);
            }
            return View(form);
        }
        #endregion


        #region " CreateDC ActionResult "

        public ActionResult CreateDC(string id)
        {
            //ViewBag.ThisSO
            Common.MyUrl = Request.RawUrl;
           
            if (!String.IsNullOrEmpty(id))
            {
                var DO = SaleManager.GetDOByDONumber(id);
                ViewBag.ThisDO = DO;
                ViewBag.DONumber = id;
            }
            if (!Common.isAuthorize())
                Response.Redirect("/Login");
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreateDC(FormCollection form)

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
                string formErrors = SaleManager.ValidateCreateDCForm(values);
                if (formErrors == "")
                {
                    DeliveryChalan DC = SaleManager.CreateDC(values);
                    if (DC != null)
                        Response.Redirect("/SaleOrder/DODetail/" + DC.DeliveryOrder.Value);
                }
                else
                    ExceptionHandler.Warning("Validation! " + formErrors);
            }
            catch (Exception ex)
            {
                ExceptionHandler.Error("Error! " + ex.Message, ex);
            }
            return View(form);
        }
        #endregion

        #region " UpdateDC Section "
        public ActionResult UpdateDC(string id)
        {
            Common.MyUrl = Request.RawUrl;
            if (!Common.isAuthorize())
                Response.Redirect("/Login");
            if (string.IsNullOrEmpty(id))
                Response.Redirect("/SaleOrder/Createdc");
            DeliveryChalan DC = SaleManager.GetDCByDCNumber(id);
            DeliveryOrder DO = SaleManager.GetDOByDONumber(DC.DeliveryOrder.Value);
            if (DC == null)
                return Redirect("/SaleOrder/CreateDC");
            ViewBag.ThisDC = DC;
            ViewBag.ThisDO = DO;
            //ZTODO: Kashif Abbas need to udpate the redirectotaction method
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UpdateDC(FormCollection form)

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
                string formErrors = SaleManager.ValidateUpdateDCForm(values);
                if (formErrors == "")
                {
                    DeliveryChalan DC = SaleManager.UpdateDC(values);
                    if (DC != null)
                        Response.Redirect("/SaleOrder/DODetail/" + DC.DeliveryOrder.Value);
                }
                else
                    ExceptionHandler.Warning("Validation! " + formErrors);
            }
            catch (Exception ex)
            {
                ExceptionHandler.Error("Error! " + ex.Message, ex);
            }
            return View(form);
        }
        #endregion
        public ActionResult DeliveryChalan()
        {
            return View();
        }
        public ActionResult ApprovedSO(String id)
        {
            Common.MyUrl = Request.RawUrl;
            if (!Common.isAuthorize())
                Response.Redirect("/Login");
            if (string.IsNullOrEmpty(id))
                Response.Redirect("/SaleOrder/CreateOrder");
            try
            {
                SaleOrder SO = SaleManager.GetSOBySONumber(id);
                if (SO == null)
                    return Redirect("/SaleOrder/CreateOrder");
                SaleManager.ApprovedSO(SO);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ExceptionHandler.Error("Unable to complete order", ex);
            }
            return RedirectToAction("index");
        }

        public ActionResult CompleteOrder(String id)
        {
            Common.MyUrl = Request.RawUrl;
            if (!Common.isAuthorize())
                Response.Redirect("/Login");
            if (string.IsNullOrEmpty(id))
                Response.Redirect("/SaleOrder/CreateOrder");
            try
            {
                SaleOrder SO = SaleManager.GetSOBySONumber(id);
                if (SO == null)
                    return Redirect("/SaleOrder/CreateOrder");
                SaleManager.CompleteSO(SO);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ExceptionHandler.Error("Unable to complete order", ex);
            }
            return RedirectToAction("index");
        }

        public ActionResult CancelSO(String id)
        {
            Common.MyUrl = Request.RawUrl;
            if (!Common.isAuthorize())
                Response.Redirect("/Login");
            if (string.IsNullOrEmpty(id))
                Response.Redirect("/SaleOrder/CreateOrder");
            try
            {
                SaleOrder SO = SaleManager.GetSOBySONumber(id);
                if (SO == null)
                    return Redirect("/SaleOrder/CreateOrder");
                SaleManager.CancelSO(SO);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ExceptionHandler.Error("Unable to complete order", ex);
            }
            return RedirectToAction("index");
        }
    }
}