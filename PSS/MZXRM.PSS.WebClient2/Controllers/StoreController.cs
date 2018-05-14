using MZXRM.PSS.Business;
using MZXRM.PSS.Common;
using MZXRM.PSS.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PatrocoalSalesSystem.Controllers
{
    public class StoreController : Controller
    {
        // GET: Store
        public ActionResult Index()
        {
            try
            {
                Common.MyUrl = Request.RawUrl;
                if (!Common.isAuthorize())
                    Response.Redirect("/Login");
                List<Store> allStores = StoreManager.AllStore;

                ViewBag.Stores = allStores;

            }
            catch (Exception ex)
            {
                ExceptionHandler.Error("Error! " + ex.Message, ex);
            }
            return View();
        }
        public FileResult Download(string id)
        {
            try
            {
                Store thisStore = StoreManager.GetStore(new Guid(id));

                //Get result file stream
                MemoryStream fileForDownload = ExcelUtil.GenerateStoreReport(thisStore);
                return File(fileForDownload.ToArray(), "application/vnd.ms-excel", thisStore.Name + ".xls");
            }
            catch (Exception ex)
            {
                ExceptionHandler.Error("Error! " + ex.Message, ex);
            }
            return null;
        }
        public ActionResult StoreInOut()
        {
            try
            {
                Common.MyUrl = Request.RawUrl;
                if (!Common.isAuthorize())
                    Response.Redirect("/Login");
                List<StoreTransfer> allStoreIO = StoreManager.AllStoreInOut;

                ViewBag.StoresInOuts = allStoreIO;

            }
            catch (Exception ex)
            {
                ExceptionHandler.Error("Error! " + ex.Message, ex);
            }
            return View();
        }
        public ActionResult Seiving()
        {
            return View();
        }

        #region /Purchase/CreateGRN
        public ActionResult CreateStoreTransfer()
        {
            Common.MyUrl = Request.RawUrl;
            if (!Common.isAuthorize())
                Response.Redirect("/Login");
            return View();
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreateStoreTransfer(FormCollection form)
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
                string formErrors = StoreManager.ValidateCreateStoreTransferForm(values);
                if (formErrors == "")
                {
                    StoreTransfer storeio = StoreManager.CreateStoreTransfer(values);
                    Response.Redirect("/Store/StoreInOut");
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

        public ActionResult StoreDetail(string id)
        {
            try
            {
                Common.MyUrl = Request.RawUrl;
                if (!Common.isAuthorize())
                    Response.Redirect("/Login");
                if (string.IsNullOrEmpty(id))
                    ExceptionHandler.Error("Error! No Store Found");
                Guid StoreId = Guid.Empty;
                if (Guid.TryParse(id, out StoreId))
                {
                    Store ThisStore = StoreManager.GetStore(StoreId);
                    ViewBag.ThisStore = ThisStore;
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.Error("Error! " + ex.Message, ex);
            }
            return View();
        }
        #region /store/updatestoretransfer
        public ActionResult UpdateStoreTransfer(string id)
        {
            Common.MyUrl = Request.RawUrl;
            if (!Common.isAuthorize())
                return Redirect("/Login");
            if (string.IsNullOrEmpty(id))
                return Redirect("/Purchase");
            StoreTransfer ThisST = StoreManager.GetStoreTransfer(id);
            if (ThisST == null)
                return Redirect("/Store");
            ViewBag.ThisST = ThisST;
            return View();
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UpdateStoreTransfer(FormCollection form)
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
            string formErrors = StoreManager.ValidateCreateStoreTransferForm(values);
            if (formErrors == "")
            {
                StoreTransfer ST = StoreManager.UpdateStoreTransfer(values);
                if (ST != null)
                    Response.Redirect("/Store/StoreInout");
            }
            else
                ExceptionHandler.Warning("Validation! " + formErrors);
            return RedirectToAction("Index");
        }
        #endregion
    }
}