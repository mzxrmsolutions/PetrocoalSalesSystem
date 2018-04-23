using MZXRM.PSS.Business;
using MZXRM.PSS.Common;
using MZXRM.PSS.Data;
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
        #region /Purchase/Index
        // GET: Purchase
        //[AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Index()
        {
            try
            {
                Common.MyUrl = Request.RawUrl;
                if (!Common.isAuthorize())
                    Response.Redirect("/Login");

                string SelectedPO = ViewBag.SelectedPO = !string.IsNullOrEmpty(Request.Form["PO"]) ? Request.Form["PO"] : "my";
                string SelectedOrigin = ViewBag.SelectedOrigin = !string.IsNullOrEmpty(Request.Form["Origin"]) ? Request.Form["Origin"] : "0";
                string SelectedSize = ViewBag.SelectedSize = !string.IsNullOrEmpty(Request.Form["Size"]) ? Request.Form["Size"] : "0";
                string SelectedVessel = ViewBag.SelectedVessel = !string.IsNullOrEmpty(Request.Form["Vessel"]) ? Request.Form["Vessel"] : "0";
                string SelectedCustomer = ViewBag.SelectedCustomer = !string.IsNullOrEmpty(Request.Form["Customer"]) ? Request.Form["Customer"] : "0";
                string PODate = ViewBag.PODate = !string.IsNullOrEmpty(Request.Form["PODate"]) ? Request.Form["PODate"] : "";

                List<PurchaseOrder> allPO = PurchaseManager.AllPOs;
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
                    if (include && SelectedCustomer == "PSL")
                    {
                        if (SelectedCustomer == "PSL")
                            if (!po.isPSL)
                                include = false;
                    }
                    if (include && SelectedCustomer != "0" && SelectedCustomer != "PSL")
                    {
                        bool flg = false;
                        foreach (PODetail pod in po.PODetailsList)
                        {
                            if (pod.Customer.Id.ToString() == SelectedCustomer)
                            {
                                flg = true;
                                break;
                            }
                        }
                        if (flg == false)
                            include = false;
                    }
                    if (include && PODate != "")
                    {
                        string[] splitdate = PODate.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                        DateTime fromDate = DateTime.Parse(splitdate[0]);
                        DateTime toDate = DateTime.Parse(splitdate[1]);
                        if (!(fromDate == DateTime.Now.Date && toDate == DateTime.Now.Date))
                            if (po.PODate < fromDate || po.PODate > toDate)
                            {
                                include = false;
                            }
                    }
                    if (include)
                        filteredPO.Add(po);
                }

                ViewBag.POs = filteredPO;
            }
            catch (Exception ex)
            {
                ExceptionHandler.Error("Error! " + ex.Message, ex);
            }
            return View();
        }
        #endregion

        #region /Purchase/OrderDetail
        public ActionResult OrderDetail(string id)
        {
            Common.MyUrl = Request.RawUrl;
            if (!Common.isAuthorize())
                Response.Redirect("/Login");

            PurchaseOrder PO = PurchaseManager.GetPO(id);
            if (PO == null)
                return Redirect("/Purchase");
            ViewBag.ThisPO = PO;
            ViewBag.CanEdit = false;
            if (PO.Status == POStatus.Created || PO.Status == POStatus.PendingApproval)
                ViewBag.CanEdit = true;
            if (!PO.isValid)
                ExceptionHandler.Warning("Quantity limit exceed");
            return View();
        }
        #endregion

        #region /Purchase/CreateOrder
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
            //List<FormMetaData> formItems = GetFormItems();
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
                PurchaseOrder PO = PurchaseManager.ValidatePOForm(values);
                if (PO != null)
                {
                    bool createSuccess = PurchaseManager.CreatePO(PO);
                    if (createSuccess)
                        Response.Redirect("/Purchase/UpdateOrder/" + PO.PONumber);
                    else
                        ExceptionHandler.Warning("Error creating PO");
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.Error("Error! " + ex.Message, ex);
            }
            return View(Request.Form);
        }
        #endregion

        #region /Purchase/UpdateOrder
        public ActionResult UpdateOrder(string id)
        {
            Common.MyUrl = Request.RawUrl;
            if (!Common.isAuthorize())
                Response.Redirect("/Login");
            if (string.IsNullOrEmpty(id))
                Response.Redirect("/Purchase/CreateOrder");
            PurchaseOrder PO = PurchaseManager.GetPO(id);
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

            Dictionary<string, string> values = new Dictionary<string, string>();

            foreach (string Key in form.Keys)
            {
                values.Add(Key, form[Key]);
            }
            PurchaseOrder po = PurchaseManager.ValidatePOForm(values);
            if (po != null)
            {
                if (form["btn"] != null && form["btn"] == "Save")
                {
                    PurchaseOrder PO = PurchaseManager.UpdatePO(values);
                    return RedirectToAction("UpdateOrder", new { id = PO.PONumber });
                }
                if (form["btn"] != null && form["btn"] == "ApprovalSubmit")
                {
                    PurchaseOrder PO = PurchaseManager.SubmitPO(values);
                    return RedirectToAction("OrderDetail", new { id = PO.PONumber });
                }
                if (form["btn"] != null && form["btn"] == "Approve")
                {
                    PurchaseOrder PO = PurchaseManager.ApprovePO(values);
                    return RedirectToAction("OrderDetail", new { id = PO.PONumber });
                }
            }
            return RedirectToAction("UpdateOrder", new { id = values["ponumber"] });
        }
        public ActionResult CompleteOrder(string id)
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

        #region /Purchase/CreateGRN
        public ActionResult CreateGRN()
        {
            Common.MyUrl = Request.RawUrl;
            if (!Common.isAuthorize())
                Response.Redirect("/Login");
            string grntype = !string.IsNullOrEmpty(Request.QueryString["type"]) ? Request.QueryString["type"] : "po";
            string poNumber = !string.IsNullOrEmpty(Request.QueryString["po"]) ? Request.QueryString["po"] : "";
            string poDetail = !string.IsNullOrEmpty(Request.QueryString["cust"]) ? Request.QueryString["cust"] : "";
            if (grntype == "po")
            {
                if (poNumber != "")
                {
                    PurchaseOrder PO = PurchaseManager.GetPO(poNumber);
                    ViewBag.ThisPO = PO;

                    if (poDetail != "")
                    {
                        foreach (PODetail pod in PO.PODetailsList)
                        {
                            if (pod.Customer.Id.ToString() == poDetail)
                            {
                                ViewBag.ThisPOD = pod;
                            }
                        }
                    }
                }
            }
            ViewBag.GRNType = grntype;
            return View();
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreateGRN(FormCollection form)
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
                string formErrors = PurchaseManager.ValidateCreateGRNForm(values);
                if (formErrors == "")
                {
                    GRN GRN = PurchaseManager.CreateGRN(values);
                    if (GRN != null)
                        Response.Redirect("/Purchase/OrderDetail/" + GRN.PO.Name);
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

        #region /Purchase/UpdateGRN
        public ActionResult UpdateGRN(string id)
        {
            Common.MyUrl = Request.RawUrl;
            if (!Common.isAuthorize())
                return Redirect("/Login");
            if (string.IsNullOrEmpty(id))
                return Redirect("/Purchase");
            GRN Grn = PurchaseManager.GetGRN(id);
            if (Grn == null)
                return Redirect("/Purchase");
            ViewBag.ThisGRN = Grn;
            ViewBag.ThisPO = PurchaseManager.GetPO(Grn.PO.Name);
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

            Dictionary<string, string> values = new Dictionary<string, string>();
            foreach (string Key in form.Keys)
            {
                values.Add(Key, form[Key]);
            }
            string formErrors = PurchaseManager.ValidateCreateGRNForm(values);
            if (formErrors == "")
            {
                GRN GRN = PurchaseManager.UpdateGRN(values);
                if (GRN != null)
                    Response.Redirect("/Purchase/OrderDetail/" + GRN.PO.Name);
            }
            else
                ExceptionHandler.Warning("Validation! " + formErrors);
            return RedirectToAction("Index");
        }
        #endregion

        #region /Purchase/CreateDCL
        public ActionResult CreateDCL()
        {
            Common.MyUrl = Request.RawUrl;
            if (!Common.isAuthorize())
                Response.Redirect("/Login");
            string poNumber = !string.IsNullOrEmpty(Request.QueryString["po"]) ? Request.QueryString["po"] : "";
            string pod = !string.IsNullOrEmpty(Request.QueryString["cust"]) ? Request.QueryString["cust"] : "";
            if (poNumber != "")
            {
                PurchaseOrder PO = PurchaseManager.GetPO(poNumber);
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

            Dictionary<string, string> values = new Dictionary<string, string>();
            foreach (string Key in form.Keys)
            {
                values.Add(Key, form[Key]);
            }
            string formErrors = PurchaseManager.ValidateCreateDutyClearForm(values);
            if (formErrors == "")
            {
                DutyClear DCL = PurchaseManager.CreateDutyClear(values);
                if (DCL != null)
                    Response.Redirect("/Purchase/OrderDetail/" + DCL.PO.Name);
            }
            else
                ExceptionHandler.Warning("Validation! " + formErrors);

            return View(form);
        }

        #endregion

        #region /Purchase/UpdateDCL
        public ActionResult UpdateDCL(string id)
        {
            Common.MyUrl = Request.RawUrl;
            if (!Common.isAuthorize())
                return Redirect("/Login");
            if (string.IsNullOrEmpty(id))
                return Redirect("/Purchase");
            DutyClear Dcl = PurchaseManager.GetDCL(id);
            if (Dcl == null)
                return Redirect("/Purchase");
            ViewBag.ThisDCL = Dcl;
            ViewBag.ThisPO = PurchaseManager.GetPO(Dcl.PO.Name);
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

            Dictionary<string, string> values = new Dictionary<string, string>();
            foreach (string Key in form.Keys)
            {
                values.Add(Key, form[Key]);
            }
            string formErrors = PurchaseManager.ValidateCreateDutyClearForm(values);
            if (formErrors == "")
            {
                DutyClear DCL = PurchaseManager.UpdateDCL(values);
                if (DCL != null)
                    Response.Redirect("/Purchase/OrderDetail/" + DCL.PO.Name);
            }
            else
                ExceptionHandler.Warning("Validation! " + formErrors);
            return RedirectToAction("Index");
        }
        #endregion

        public ActionResult GRNList()
        {
            Common.MyUrl = Request.RawUrl;
            if (!Common.isAuthorize())
                Response.Redirect("/Login");

            string SelectedGRN = ViewBag.SelectedGRN = !string.IsNullOrEmpty(Request.Form["GRN"]) ? Request.Form["GRN"] : "my";
            string SelectedPO = ViewBag.SelectedPO = !string.IsNullOrEmpty(Request.Form["PO"]) ? Request.Form["PO"] : "0";
            string SelectedStore = ViewBag.SelectedStore = !string.IsNullOrEmpty(Request.Form["Store"]) ? Request.Form["Store"] : "0";

            List<GRN> allGRN = PurchaseManager.AllGRNs;
            List<GRN> filteredGRN = new List<GRN>();

            foreach (GRN grn in allGRN)
            {
                PurchaseOrder thisPO = PurchaseManager.GetPO(grn.PO.Name);
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
                    if (grn.PO.Name != SelectedPO)
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
            ViewBag.AllPO = PurchaseManager.AllPOs;
            //PurchaseUtil.WriteToXmlFile<PurchaseOrder>("E:/temp/po1001.txt", PurchaseUtil.GetPO("PO-1001"));

            return View();
        }

        public ActionResult OrderDetails()
        {
            return View();
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