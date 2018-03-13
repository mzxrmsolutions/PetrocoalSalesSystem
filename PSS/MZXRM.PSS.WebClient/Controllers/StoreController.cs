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
        public ActionResult StoreInOut()
        {
            try
            {
                Common.MyUrl = Request.RawUrl;
                if (!Common.isAuthorize())
                    Response.Redirect("/Login");
                List<StoreInOut> allStoreIO = StoreManager.AllStoreInOut;

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
    }
}