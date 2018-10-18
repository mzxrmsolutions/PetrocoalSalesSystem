using MZXRM.PSS.Business;
using MZXRM.PSS.Common;
using MZXRM.PSS.Data;
using MZXRM.PSS.WebClient.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MZXRM.PSS.WebClient.Models;

namespace PatrocoalSalesSystem.Controllers
{
    public class StoreController : Controller
    {
        PetrocoalEntities db = new PetrocoalEntities();
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
            MZXRM.PSS.WebClient.Models.Seiving seiving = new MZXRM.PSS.WebClient.Models.Seiving();
            ViewBag.ThisCust = !string.IsNullOrEmpty(Request.QueryString["cust"]) ? CustomerManager.GetCustomer(Guid.Parse(Request.QueryString["cust"])) : null;


            return View(seiving);
            //return View();
        }

        [HttpPost]
        public ActionResult Seiving(MZXRM.PSS.WebClient.Models.Seiving seiving, FormCollection fomr)
        {
            List<MZXRM.PSS.WebClient.Models.SeivingSizeQty> seivingSizeQtyList = new List<MZXRM.PSS.WebClient.Models.SeivingSizeQty>();
            var size = Common.Size;
            foreach (var m in size)
            {
                string value = Request.Form["Qty_" + m.Index.ToString()];

                if (value != null)
                {
                    decimal qty = decimal.Parse(value);

                    MZXRM.PSS.WebClient.Models.SeivingSizeQty seivingSizeQty = new MZXRM.PSS.WebClient.Models.SeivingSizeQty();

                    seivingSizeQty.SizeId = m.Index;
                    seivingSizeQty.SizeQuantity = qty;
                    seivingSizeQtyList.Add(seivingSizeQty);
                }
            }
            seiving.SeivingSizeQties = seivingSizeQtyList;


            if (ModelState.IsValid)
            {
                db.Seivings.Add(seiving);
                db.SaveChanges();

            }
            return View(seiving);
        }

        public ActionResult SizeList()
        {
            List<Size> sizeList = new List<Size>();

            //Size size1 = new Size()
            //{ID=1,
            //Name = "Test1"

            //};

            //size.Add(size1);
            var size = Common.Size;

            foreach (var item in size)
            {
                Size size1 = new Size()
                {
                    Id = item.Index,
                    Name = item.Value
                };
                sizeList.Add(size1);

            }

            return PartialView("SizeListing", sizeList);
        }
        public ActionResult SeivingCreate()
        {
            MZXRM.PSS.WebClient.Models.Seiving seiving = new MZXRM.PSS.WebClient.Models.Seiving();


            return View(seiving);
        }
        [HttpPost]
        public ActionResult SeivingCreate(MZXRM.PSS.WebClient.Models.Seiving seiving, FormCollection fomr)
        {
            List<MZXRM.PSS.WebClient.Models.SeivingSizeQty> seivingSizeQtyList = new List<MZXRM.PSS.WebClient.Models.SeivingSizeQty>();
            var size = Common.Size;
            foreach (var m in size)
            {
                string value = Request.Form["Qty_" + m.Index.ToString()];

                if (value != null)
                {
                    decimal qty = decimal.Parse(value);

                    MZXRM.PSS.WebClient.Models.SeivingSizeQty seivingSizeQty = new MZXRM.PSS.WebClient.Models.SeivingSizeQty();

                    seivingSizeQty.SizeId = m.Index;
                    seivingSizeQty.SizeQuantity = qty;
                    seivingSizeQtyList.Add(seivingSizeQty);
                }
            }
            seiving.SeivingSizeQties = seivingSizeQtyList;
            

            if (ModelState.IsValid)
            {
                db.Seivings.Add(seiving);
                db.SaveChanges();

            }
            return View(seiving);
        }


        public ActionResult SeivingList()
        {
            var sevingList = (from m in db.Seivings
                              select m).ToList();

            foreach (var j in sevingList) {

                var svQtyListByID = (from n in db.SeivingSizeQties
                         where n.SeivingID == j.ID
                         select n).ToList();

                j.SeivingSizeQties = svQtyListByID;
                        }
            return PartialView("SeivingList", sevingList);
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