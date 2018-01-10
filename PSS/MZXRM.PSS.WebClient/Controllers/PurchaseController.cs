using PatrocoalSalesSystem.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PatrocoalSalesSystem.Controllers
{
    public class PurchaseController : Controller
    {
        // GET: Purchase
        //[AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Index()
        {
            Common.MyUrl = Request.RawUrl;
            if (!Common.isAuthorize())
                Response.Redirect("/Login");

            string SelectedPO = ViewBag.SelectedPO = !string.IsNullOrEmpty(Request.Form["PO"]) ? Request.Form["PO"] : "my";
            string SelectedOrigin = ViewBag.SelectedOrigin = !string.IsNullOrEmpty(Request.Form["Origin"]) ? Request.Form["Origin"] : "0";
            string SelectedSize = ViewBag.SelectedSize = !string.IsNullOrEmpty(Request.Form["Size"]) ? Request.Form["Size"] : "0";
            string SelectedVessel = ViewBag.SelectedVessel = !string.IsNullOrEmpty(Request.Form["Vessel"]) ? Request.Form["Vessel"] : "0";
            string SelectedCustomer = ViewBag.SelectedCustomer = !string.IsNullOrEmpty(Request.Form["Customer"]) ? Request.Form["Customer"] : "0";

            List<PurchaseOrder> allPO = PurchaseUtil.AllPOs;
            List<PurchaseOrder> filteredPO = new List<PurchaseOrder>();

            foreach (PurchaseOrder po in allPO)
            {
                bool include = false;
                switch (SelectedPO)
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
                if (include)
                    filteredPO.Add(po);
            }

            ViewBag.POs = filteredPO;
            return View();
        }
        public ActionResult OrderDetail(string id)
        {
            PurchaseOrder PO = PurchaseUtil.GetPO(id);
            if (PO == null)
                return Redirect("/Purchase");
            ViewBag.ThisPO = PO;
            return View();
        }
        #region CreateOrder
        public ActionResult CreateOrder()
        {
            Common.MyUrl = Request.RawUrl;
            if (!Common.isAuthorize())
                Response.Redirect("/Login");
            return View();
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreateOrder(FormCollection form)
        {
            if (!Common.isAuthorize())
            {
                ExceptionHandler.Error("Session Timeout");
                return View();
            }
            if (form["btn"] != null && form["btn"] == "Reset")
                return View();
            if (ValidateCreatePOForm(form))
            {
                Dictionary<string, string> values = new Dictionary<string, string>();
                foreach (string Key in form.Keys)
                {
                    values.Add(Key, form[Key]);
                }
                PurchaseOrder PO = PurchaseUtil.CreatePO(values);
                if (PO != null)
                    Response.Redirect("/Purchase/UpdateOrder/" + PO.PONumber);
            }
            return View();
        }
        #endregion
        #region UpdateOrder
        public ActionResult UpdateOrder(string id)
        {
            Common.MyUrl = Request.RawUrl;
            if (!Common.isAuthorize())
                Response.Redirect("/Login");
            if (string.IsNullOrEmpty(id))
                Response.Redirect("/Purchase/CreateOrder");
            PurchaseOrder PO = PurchaseUtil.GetPO(id);
            if (PO == null)
                return Redirect("/Purchase/CreateOrder");
            ViewBag.ThisPO = PO;
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
            if (ValidateCreatePOForm(form))
            {
                Dictionary<string, string> values = new Dictionary<string, string>();
                foreach (string Key in form.Keys)
                {
                    values.Add(Key, form[Key]);
                }
                if (form["btn"] != null && form["btn"] == "Save")
                {
                    PurchaseOrder PO = PurchaseUtil.UpdatePO(values);
                    return RedirectToAction("UpdateOrder", new { id = PO.PONumber });
                }
                if (form["btn"] != null && form["btn"] == "ApprovalSubmit")
                {
                    PurchaseOrder PO = PurchaseUtil.SubmitPO(values);
                    return RedirectToAction("OrderDetail", new { id = PO.PONumber });
                }
                if (form["btn"] != null && form["btn"] == "Approve")
                {
                    PurchaseOrder PO = PurchaseUtil.ApprovePO(values);
                    return RedirectToAction("OrderDetail", new { id = PO.PONumber });
                }
            }
            return RedirectToAction("Index");
        }
        public ActionResult CompleteOrder(string id)
        {
            Common.MyUrl = Request.RawUrl;
            if (!Common.isAuthorize())
                Response.Redirect("/Login");
            if (string.IsNullOrEmpty(id))
                Response.Redirect("/Purchase");
            PurchaseOrder PO = PurchaseUtil.GetPO(id);
            if (PO == null)
                return Redirect("/Purchase");
            PurchaseUtil.CompleteOrder(PO);
            return RedirectToAction("OrderDetail", new { id = PO.PONumber });
        }
        #endregion
        #region CreateGRN
        public ActionResult CreateGRN()
        {
            Common.MyUrl = Request.RawUrl;
            if (!Common.isAuthorize())
                Response.Redirect("/Login");
            string grntype = !string.IsNullOrEmpty(Request.QueryString["type"]) ? Request.QueryString["type"] : "po";
            string poNumber = !string.IsNullOrEmpty(Request.QueryString["po"]) ? Request.QueryString["po"] : "";
            if (grntype == "po")
            {
                if (poNumber != "")
                {
                    PurchaseOrder PO = PurchaseUtil.GetPO(poNumber);
                    ViewBag.ThisPO = PO;
                }
            }
            ViewBag.GRNType = grntype;
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreateGRN(FormCollection form)
        {
            if (!Common.isAuthorize())
            {
                ExceptionHandler.Error("Session Timeout");
                return View();
            }
            if (form["btn"] != null && form["btn"] == "Reset")
                return View();
            if (ValidateCreateGRNForm(form))
            {
                Dictionary<string, string> values = new Dictionary<string, string>();
                foreach (string Key in form.Keys)
                {
                    values.Add(Key, form[Key]);
                }
                GRN GRN = PurchaseUtil.CreateGRN(values);
                if (GRN != null)
                    Response.Redirect("/Purchase/OrderDetail/" + GRN.PO.Name);
            }
            return View();
        }
        public ActionResult UpdateGRN(string id)
        {
            Common.MyUrl = Request.RawUrl;
            if (!Common.isAuthorize())
                return Redirect("/Login");
            if (string.IsNullOrEmpty(id))
                return Redirect("/Purchase");
            GRN Grn = PurchaseUtil.GetGRN(id);
            if (Grn == null)
                return Redirect("/Purchase");
            ViewBag.ThisGRN = Grn;
            ViewBag.ThisPO = PurchaseUtil.GetPO(Grn.PO.Name);
            return View();
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UpdateGRN(FormCollection form)
        {
            if (!Common.isAuthorize())
            {
                ExceptionHandler.Error("Session Timeout");
                return View();
            }
            if (form["btn"] != null && form["btn"] == "Reset")
                return View();
            if (ValidateCreateGRNForm(form))
            {
                Dictionary<string, string> values = new Dictionary<string, string>();
                foreach (string Key in form.Keys)
                {
                    values.Add(Key, form[Key]);
                }
                GRN GRN = PurchaseUtil.UpdateGRN(values);
                if (GRN != null)
                    Response.Redirect("/Purchase/OrderDetail/" + GRN.PO.Name);
            }
            return RedirectToAction("Index");
        }
        #endregion
        #region CreateDutyClear
        public ActionResult CreateDCL()
        {
            Common.MyUrl = Request.RawUrl;
            if (!Common.isAuthorize())
                Response.Redirect("/Login");
            string poNumber = !string.IsNullOrEmpty(Request.QueryString["po"]) ? Request.QueryString["po"] : "";
            string pod = !string.IsNullOrEmpty(Request.QueryString["cust"]) ? Request.QueryString["cust"] : "";
            if (poNumber != "")
            {
                PurchaseOrder PO = PurchaseUtil.GetPO(poNumber);
                ViewBag.ThisPO = PO;
                if (pod != "")
                {
                    PODetail POD = PO.PODetailsList.Where(n => n.Customer.Name == pod).First();
                    ViewBag.ThisPOD = POD;
                }
            }
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreateDCL(FormCollection form)
        {
            if (!Common.isAuthorize())
            {
                ExceptionHandler.Error("Session Timeout");
                return View();
            }
            if (form["btn"] != null && form["btn"] == "Reset")
                return View();
            if (ValidateCreateDutyClearForm(form))
            {
                Dictionary<string, string> values = new Dictionary<string, string>();
                foreach (string Key in form.Keys)
                {
                    values.Add(Key, form[Key]);
                }
                DutyClear DClear = PurchaseUtil.CreateDutyClear(values);
                if (DClear != null)
                    Response.Redirect("/Purchase/OrderDetail/" + DClear.PO.Name);
            }
            return View();
        }

        private bool ValidateCreateDutyClearForm(FormCollection form)
        {
            // ZTODO
            return true;
        }
        #endregion

        public ActionResult UpdateDCL(string id)
        {
            Common.MyUrl = Request.RawUrl;
            if (!Common.isAuthorize())
                return Redirect("/Login");
            if (string.IsNullOrEmpty(id))
                return Redirect("/Purchase");
            DutyClear Dcl = PurchaseUtil.GetDCL(id);
            if (Dcl == null)
                return Redirect("/Purchase");
            ViewBag.ThisDCL = Dcl;
            ViewBag.ThisPO = PurchaseUtil.GetPO(Dcl.PO.Name);
            return View();
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UpdateDCL(FormCollection form)
        {
            if (!Common.isAuthorize())
            {
                ExceptionHandler.Error("Session Timeout");
                return View();
            }
            if (form["btn"] != null && form["btn"] == "Reset")
                return View();
            if (ValidateCreateDutyClearForm(form))
            {
                Dictionary<string, string> values = new Dictionary<string, string>();
                foreach (string Key in form.Keys)
                {
                    values.Add(Key, form[Key]);
                }
                DutyClear DCL = PurchaseUtil.UpdateDCL(values);
                if (DCL != null)
                    Response.Redirect("/Purchase/OrderDetail/" + DCL.PO.Name);
            }
            return RedirectToAction("Index");
        }
        public ActionResult GRNList()
        {
            Common.MyUrl = Request.RawUrl;
            if (!Common.isAuthorize())
                Response.Redirect("/Login");

            string SelectedGRN = ViewBag.SelectedGRN = !string.IsNullOrEmpty(Request.Form["GRN"]) ? Request.Form["GRN"] : "my";
            string SelectedPO = ViewBag.SelectedPO = !string.IsNullOrEmpty(Request.Form["PO"]) ? Request.Form["PO"] : "0";
            string SelectedStore = ViewBag.SelectedStore = !string.IsNullOrEmpty(Request.Form["Store"]) ? Request.Form["Store"] : "0";

            List<GRN> allGRN = PurchaseUtil.AllGRNs;
            List<GRN> filteredGRN = new List<GRN>();

            foreach (GRN grn in allGRN)
            {
                PurchaseOrder thisPO = PurchaseUtil.GetPO(grn.PO.Name);
                bool include = false;
                switch (SelectedGRN)
                {
                    default:
                    case "my":
                        if (thisPO.Lead.Id == Common.CurrentUser.Id)
                            include = true;
                        break;
                    case "all":
                        include = true;
                        break;
                    case "loan":
                        if (grn.Status == GRNStatus.Loan)
                            include = true;
                        break;
                }
                if (include && SelectedPO != "0")
                {
                    if (grn.PO.Name !=SelectedPO)
                        include = false;
                }
                if (include && SelectedStore != "0")
                {
                    if (grn.Store.Id.ToString() != SelectedStore)
                        include = false;
                }
                if (include)
                    filteredGRN.Add(grn);
            }

            ViewBag.GRNs = filteredGRN;
            return View();
        }





        public ActionResult Index2()
        {
            ViewBag.AllPO = PurchaseUtil.AllPOs;
            //PurchaseUtil.WriteToXmlFile<PurchaseOrder>("E:/temp/po1001.txt", PurchaseUtil.GetPO("PO-1001"));

            return View();
        }

        public ActionResult OrderDetails()
        {
            return View();
        }


        private bool ValidateCreatePOForm(FormCollection form)
        {
            // ZTODO
            return true;
        }
        private bool ValidateCreateGRNForm(FormCollection form)
        {
            // ZTODO
            return true;
        }


        

        public ActionResult ApprovedOrder()
        {
            return View();
        }
    }
}