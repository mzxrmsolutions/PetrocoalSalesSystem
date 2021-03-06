﻿using MZXRM.PSS.Business.DBMap;
using MZXRM.PSS.Common;
using MZXRM.PSS.Data;
using MZXRM.PSS.DataManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;

namespace MZXRM.PSS.Business
{
    public class StoreManager
    {
        private static List<Store> _allStores;
        public static List<Store> AllStore
        {
            get
            {
                if (HttpContext.Current.Session[SessionManager.StoreSession] != null)
                    _allStores = HttpContext.Current.Session[SessionManager.StoreSession] as List<Store>;
                if (_allStores == null || _allStores.Count == 0)
                    _allStores = ReadAllStore();
                return _allStores;
            }
        }
        private static List<StoreTransfer> _allStoreInOuts;
        public static List<StoreTransfer> AllStoreInOut
        {
            get
            {
                if (HttpContext.Current.Session[SessionManager.StoreIOSession] != null)
                    _allStoreInOuts = HttpContext.Current.Session[SessionManager.StoreIOSession] as List<StoreTransfer>;
                if (_allStoreInOuts == null || _allStoreInOuts.Count == 0)
                    _allStoreInOuts = ReadAllStoreIO();
                return _allStoreInOuts;
            }
        }
        #region Business Need
        public static List<Store> ReadAllStore()
        {
            DataTable DTstore =StoreDataManager. GetAllStore();
            DataTable DTcustStock = StoreDataManager.GetAllCustomerStock();
            DataTable DTstockMovement = StoreDataManager.GetAllStockMovements();
            List<Store> allStores = StoreMap.MapStoreData(DTstore, DTcustStock, DTstockMovement);
            List<Store> calculatedStores = new List<Store>();
            foreach (Store store in allStores)
            {
                Store CalculatedStore = CalculateStoreQuantity(store);
                calculatedStores.Add(CalculatedStore);
            }
            HttpContext.Current.Session.Add(SessionManager.StoreSession, calculatedStores);
            _allStores = calculatedStores;
            return calculatedStores;
        }

        public static List<Store> ReadStoreBySaleStation(Guid saleStationId)
        {
            List<Store> myStores = new List<Store>();
            foreach (Store store in AllStore)
                if (store.SaleStationId.Id == saleStationId)
                    myStores.Add(store);
            return myStores;
        }
        public static Store CalculateStoreQuantity(Store Store)
        {
            decimal quantity = 0;
            foreach (CustomerStock custStock in Store.Stock)
            {
                quantity += custStock.Quantity;
            }
            Store.TotalStock = quantity;
            return Store;
        }
        public static void ResetCache()
        {
            _allStores = null;
            _allStoreInOuts = null;
            HttpContext.Current.Session.Remove(SessionManager.StoreIOSession);
            HttpContext.Current.Session.Remove(SessionManager.StoreSession);
        }
        public static List<StoreTransfer> ReadAllStoreIO()
        {
                DataTable DTstoreIO =StoreDataManager. GetAllStoreInOut();
                List<StoreTransfer> allStoreIOs = StoreMap.MapStoreTransferData(DTstoreIO);
                //foreach (StoreInOut storeIO in allStoreIOs)
                //{
                //    StoreInOut CalculatedStore = CalculateStoreQuantity(storeIO);
                //    AllStores.Add(CalculatedStore);
                //}
                HttpContext.Current.Session.Add(SessionManager.StoreIOSession, allStoreIOs);
                return allStoreIOs;
        }
        #endregion
        

        public static Store GetStore(Guid storeId)
        {
            foreach (Store store in AllStore)
            {
                if (store.Id == storeId)
                    return store;
            }
            return null;
        }

        public static List<Store> GetAllStore()
        {
           
            return AllStore;
        }

        public static string ValidateCreateStoreTransferForm(Dictionary<string, string> values)
        {
            try
            {
                foreach (KeyValuePair<string, string> keyValue in values)
                {
                    string key = keyValue.Key;
                    string value = keyValue.Value;
                    DateTime dtTemp = DateTime.Now.Date;
                    decimal decTemp = 0;
                    if (key == "FromStore" && value == "0") throw new Exception("From Store is required");
                    if (key == "date" && !DateTime.TryParse(value,out dtTemp)) throw new Exception("Date is required");
                    if (key == "Customer" && value == "0") throw new Exception("Customer is required");
                    if (key == "Origin" && value == "0") throw new Exception("Origin is required");
                    if (key == "Vessel" && value == "0") throw new Exception("Vessel is required");
                    if (key == "Size" && value == "0") throw new Exception("Size is required");
                    if (key == "Quantity" && string.IsNullOrEmpty(value) && !decimal.TryParse(value,out decTemp)) throw new Exception("Quantity is required");
                    if (key == "ToStore" && value == "0") throw new Exception("ToStore is required");
                    if (key == "VehicleNo" && string.IsNullOrEmpty(value)) throw new Exception("VehicleNo is required");
                    if (key == "BiltyNo" && string.IsNullOrEmpty(value)) throw new Exception("BiltyNo is required");
                    if (key == "BiltyDate" && !DateTime.TryParse(value, out dtTemp)) throw new Exception("BiltyDate is required");
                    if (key == "Invoice" && string.IsNullOrEmpty(value)) throw new Exception("Invoice is required");
                    if (key == "CCMNo" && string.IsNullOrEmpty(value)) throw new Exception("CCMNo is required");
                    if (key == "Transporter" && string.IsNullOrEmpty(value)) throw new Exception("Transporter is required");
                    //if (key == "Remarks" && string.IsNullOrEmpty(value)) throw new Exception("Remarks is required");

                }
                //PurchaseOrder po = GetPO(values["PO"]);
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public static StoreTransfer NewStoreTransfer()
        {
            StoreTransfer St = new StoreTransfer();


            St.InOut = StoreMovementType.Active;
            St.Status = StoreTransferStatus.InTransit;
            St.CreatedOn = St.ModifiedOn = St.STDate = DateTime.Now;
            St.CreatedBy = St.ModifiedBy = St.LeadId = new Reference() { Id = Common.CurrentUser.Id, Name = Common.CurrentUser.Name }; ;
            St.STNumber = GenerateNextSTNumber();


            return St;
        }
        private static string GenerateNextSTNumber()
        {
            int lNumber = 1001;
            foreach (StoreTransfer st in AllStoreInOut)
            {
                string stNumber = st.STNumber.Replace("ST-", "");
                int No = int.Parse(stNumber);
                if (No > lNumber)
                    lNumber = No;
            }
            lNumber++;
            return "ST-" + lNumber.ToString();
        }
        public static StoreTransfer CreateStoreTransfer(Dictionary<string, string> values)
        {
            StoreTransfer ST = NewStoreTransfer();
            ST.STDate = values.ContainsKey("Date") ? DateTime.Parse(values["Date"]) : DateTime.Now;
            ST.Customer = values.ContainsKey("Customer") ? CustomerManager.GetCustomerRef(values["Customer"]) : CustomerManager.GetDefaultRef();
            ST.Origin = values.ContainsKey("Origin") ? CommonDataManager.GetOrigin(values["Origin"]) : CommonDataManager.GetDefaultRef();
            ST.Size = values.ContainsKey("Size") ? CommonDataManager.GetOrigin(values["Size"]) : CommonDataManager.GetDefaultRef();
            ST.Vessel = values.ContainsKey("Vessel") ? CommonDataManager.GetOrigin(values["Vessel"]) : CommonDataManager.GetDefaultRef();
            ST.Quantity = values.ContainsKey("Quantity") ? decimal.Parse(values["Quantity"]) : 0;
            ST.FromStoreId = values.ContainsKey("FromStore") ? StoreManager.GetStoreRef(values["FromStore"]) : StoreManager.GetDefaultRef();
            ST.ToStoreId = values.ContainsKey("ToStore") ? StoreManager.GetStoreRef(values["ToStore"]) : StoreManager.GetDefaultRef();
            ST.VehicleNo = values.ContainsKey("VehicleNo") ? values["VehicleNo"] : "";
            ST.BiltyNo = values.ContainsKey("BiltyNo") ? values["BiltyNo"] : "";
            ST.BiltyDate = values.ContainsKey("BiltyDate") ? DateTime.Parse(values["BiltyDate"]) : DateTime.MinValue;
            ST.RRInvoice = values.ContainsKey("Invoice") ? values["Invoice"] : "";
            ST.CCMNumber = values.ContainsKey("CCMNo") ? values["CCMNo"] : "";
            ST.Transporter = values.ContainsKey("Transporter") ? CommonDataManager.GetTrader(values["Transporter"]) : CommonDataManager.GetDefaultRef();
            ST.StoreInDate = DateTime.MinValue;
            ST.StoreInQuantity = 0;
            ST.Remarks = values.ContainsKey("Remarks") ? values["Remarks"] : "";

            StoreDataManager.CreateStoreTransfer(StoreMap.reMapStoreTransferData( ST));

            // ztodo: create store movement record


            ResetCache();
            return ST;
        }

        public static StoreTransfer GetStoreTransfer(string STNumber)
        {
            foreach (StoreTransfer st in AllStoreInOut)
            {
                if (st.STNumber == STNumber)
                {
                    return st;
                }
            }
            return null;
        }

        public static StoreTransfer UpdateStoreTransfer(Dictionary<string, string> values)
        {
            string stNumber = values["STNumber"];
            string stId = values["STID"];

            StoreTransfer ST = GetStoreTransfer(stNumber);
            ST.Id = string.IsNullOrEmpty(stId) ? 0 : int.Parse(stId);
            ST.STDate = values.ContainsKey("Date") ? DateTime.Parse(values["Date"]) : DateTime.Now;
            ST.Customer = values.ContainsKey("Customer") ? CustomerManager.GetCustomerRef(values["Customer"]) : CustomerManager.GetDefaultRef();
            ST.Origin = values.ContainsKey("Origin") ? CommonDataManager.GetOrigin(values["Origin"]) : CommonDataManager.GetDefaultRef();
            ST.Size = values.ContainsKey("Size") ? CommonDataManager.GetOrigin(values["Size"]) : CommonDataManager.GetDefaultRef();
            ST.Vessel = values.ContainsKey("Vessel") ? CommonDataManager.GetOrigin(values["Vessel"]) : CommonDataManager.GetDefaultRef();
            ST.Quantity = values.ContainsKey("Quantity") ? decimal.Parse(values["Quantity"]) : 0;
            ST.FromStoreId = values.ContainsKey("FromStore") ? GetStoreRef(values["FromStore"]) : GetDefaultRef();
            ST.ToStoreId = values.ContainsKey("ToStore") ? GetStoreRef(values["ToStore"]) : GetDefaultRef();
            ST.VehicleNo = values.ContainsKey("VehicleNo") ? values["VehicleNo"] : "";
            ST.BiltyNo = values.ContainsKey("BiltyNo") ? values["BiltyNo"] : "";
            ST.BiltyDate = values.ContainsKey("BiltyDate") ? DateTime.Parse(values["BiltyDate"]) : DateTime.MinValue;
            ST.RRInvoice = values.ContainsKey("Invoice") ? values["Invoice"] : "";
            ST.CCMNumber = values.ContainsKey("CCMNo") ? values["CCMNo"] : "";
            ST.Transporter = values.ContainsKey("Transporter") ? CommonDataManager.GetTrader(values["Transporter"]) : CommonDataManager.GetDefaultRef();
            ST.StoreInDate = DateTime.MinValue;
            ST.StoreInQuantity = 0;
            ST.Remarks = values.ContainsKey("Remarks") ? values["Remarks"] : "";

            StoreDataManager.UpdateStoreTransfer(StoreMap.reMapStoreTransferData( ST));
            ResetCache();
            return ST;
        }

       
        #region Get Reference
        public static Reference GetStoreRef(string id)
        {
            Guid storeId = !string.IsNullOrEmpty(id) ? new Guid(id) : Guid.Empty;
            Store store = GetStore(storeId);
            if (store != null)
                return new Reference() { Id = store.Id, Name = store.Name };
            return GetDefaultRef();
        }

        public static Reference GetDefaultRef()
        {
            return new Reference() { Id = Guid.Empty, Name = "" };
        }


        #endregion

        public static Seiving CreateSeving(Seiving seiving)
        {


            StoreDataManager.CreateSeving(StoreMap.reMapSeivingData(seiving));
            
            StoreDataManager.CreateSevingQuantity(StoreMap.reMapSeivingQuantityData(seiving.seivingSizeQty));

            // ztodo: create store movement record


            ResetCache();
            return seiving;
        }

    }
}
