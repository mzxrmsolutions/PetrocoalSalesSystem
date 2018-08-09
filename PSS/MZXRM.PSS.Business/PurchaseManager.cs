using MZXRM.PSS.Business.DBMap;
using MZXRM.PSS.Common;
using MZXRM.PSS.Data;
using MZXRM.PSS.DataManager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace MZXRM.PSS.Business
{
    public static class PurchaseManager
    {
        #region Common Lists
        private static List<PurchaseOrder> _AllPOs;
        public static List<PurchaseOrder> AllPOs
        {
            get
            {
                if (HttpContext.Current.Session[SessionManager.POSession] != null)
                    _AllPOs = HttpContext.Current.Session[SessionManager.POSession] as List<PurchaseOrder>;
                if (_AllPOs == null || _AllPOs.Count == 0)
                    _AllPOs = ReadAllPO();
                return _AllPOs;
            }
        }
        public static List<PurchaseOrder> ReadAllPO()
        {
            DataTable DTpo = PurchaseDataManager.GetAllPOs();
            DataTable DTpod = PurchaseDataManager.GetAllPODs();
            DataTable DTgrn = PurchaseDataManager.GetAllGRNs();
            DataTable DTdcl = PurchaseDataManager.GetAllDCLs();
            List<PurchaseOrder> allPOs = POMap.MapPOData(DTpo, DTpod, DTgrn, DTdcl);
            List<PurchaseOrder> calculatedPOs = new List<PurchaseOrder>();
            foreach (PurchaseOrder PO in allPOs)
            {
                PurchaseOrder po = PurchaseDataManager.CalculatePO(PO);
                calculatedPOs.Add(po);
            }
            HttpContext.Current.Session.Add(SessionManager.POSession, calculatedPOs);
            _AllPOs = calculatedPOs;
            return calculatedPOs;
        }
        public static void ResetCache()
        {
            _AllPOs = null;
            HttpContext.Current.Session[SessionManager.POSession] = null;
        }
        public static bool SavePO(PurchaseOrder PO)
        {
            if (PO.Id == Guid.Empty)
            {
                Guid newPOId = PurchaseDataManager.CreatePO(POMap.reMapPOData(PO));
                PO.Id = newPOId;
            }
            else
                PurchaseDataManager.UpdatePO(POMap.reMapPOData(PO));
            foreach (PODetail pod in PO.PODetailsList)
            {
                if (pod.Id == Guid.Empty)
                {
                    pod.PO = new Reference() { Id = PO.Id, Name = PO.PONumber };
                    Guid newPODId = PurchaseDataManager.CreatePODetail(POMap.reMapPODetailData(pod));
                    pod.Id = newPODId;
                }
                else
                {
                    PurchaseDataManager.UpdatePODetail(POMap.reMapPODetailData(pod));
                }
            }
            ResetCache();
            return true;
        }

        public static List<GRN> AllGRNs
        {
            get
            {
                List<GRN> allGRNs = new List<GRN>();
                foreach (PurchaseOrder po in AllPOs)
                {
                    foreach (PODetail pod in po.PODetailsList)
                    {
                        foreach (GRN grn in pod.GRNsList)
                            allGRNs.Add(grn);
                    }
                }
                return allGRNs;
            }
        }
        public static List<DutyClear> AllDCLs
        {
            get
            {
                List<DutyClear> _AllDCLs = new List<DutyClear>();
                foreach (PurchaseOrder po in AllPOs)
                {
                    foreach (PODetail pod in po.PODetailsList)
                    {
                        foreach (DutyClear dcl in pod.DutyClearsList)
                            _AllDCLs.Add(dcl);
                    }
                }
                return _AllDCLs;
            }
        }
        public static List<PurchaseOrder> ApprovedPOs
        {
            get
            {
                return AllPOs.Where(p => p.Status == POStatus.InProcess).ToList();
            }
        }
        #endregion

        #region Get Operations
        public static PurchaseOrder GetPO(string ponumber)
        {
            Guid poid = Guid.Empty;
            if (Guid.TryParse(ponumber, out poid))
            {
                foreach (PurchaseOrder po in AllPOs)
                {
                    if (po.Id == poid)
                        return po;
                }
            }
            foreach (PurchaseOrder po in AllPOs)
            {
                if (po.PONumber == ponumber)
                    return po;
            }
            return null;
        }
        public static Reference GetPOByDCL(string DCLId)
        {
            foreach (PurchaseOrder po in AllPOs)
            {
                foreach (PODetail pod in po.PODetailsList)
                    foreach (DutyClear dcl in pod.DutyClearsList)

                        if (dcl.Id.ToString() == DCLId)
                            return new Reference() { Id = po.Id, Name = dcl.DCLNumber };
            }
            return new Reference() { Id = Guid.Empty, Name = "" };
        }
        public static GRN GetGRN(string grnnumber)
        {
            foreach (GRN grn in AllGRNs)
            {
                if (grn.GRNNumber == grnnumber)
                    return grn;
            }
            return null;
        }
        public static DutyClear GetDCL(string dclnumber)
        {
            foreach (DutyClear dcl in AllDCLs)
            {
                if (dcl.DCLNumber == dclnumber)
                    return dcl;
            }
            return null;
        }
        #endregion

        #region Validate Forms TODO:
        public static PurchaseOrder ValidatePOForm(Dictionary<string, string> values)
        {
            try
            {
                string ErrorMessage = string.Empty;
                if (values["Origin"] == "0") ErrorMessage += "Origin, ";
                if (values["Size"] == "0") ErrorMessage += "Size, ";
                if (values["Vessel"] == "0") ErrorMessage += "Vessel, ";
                if (string.IsNullOrEmpty(values["PODate"])) ErrorMessage += "PO Date, ";
                if (string.IsNullOrEmpty(values["TargetDays"])) ErrorMessage += "Target Days, ";
                if (string.IsNullOrEmpty(values["Supplier"])) ErrorMessage += "Supplier, ";
                if (string.IsNullOrEmpty(values["Lead"])) ErrorMessage += "Lead, ";
                // if (key == "PaymentTerms" && string.IsNullOrEmpty(value)) throw new Exception("PaymentTerms is required");
                if (string.IsNullOrEmpty(values["BufferMin"])) ErrorMessage += "Buffer Min %, ";
                if (string.IsNullOrEmpty(values["BufferMax"])) ErrorMessage += "Buffer Max %, ";
                if (values["Customer_1"] == "0") ErrorMessage += "Customer, ";
                if (string.IsNullOrEmpty(ErrorMessage))
                {
                    bool error = false;
                    for (int i = 1; i <= 10; i++)
                    {
                        if (values["Customer_" + i] == "0")
                            continue;
                        if (string.IsNullOrEmpty(values["Quantity_" + i])) error = true;
                        if (string.IsNullOrEmpty(values["Rate_" + i])) error = true;
                        if (string.IsNullOrEmpty(values["AllowedWastage_" + i])) error = true;
                        if (string.IsNullOrEmpty(values["CostPerTon_" + i])) error = true;
                        DateTime tdate = new DateTime();
                        if (!DateTime.TryParse(values["TargetDate_" + i], out tdate)) error = true;
                        if (error)
                        {
                            ErrorMessage += "PO Detail";
                            break;
                        }
                    }

                }
                if (string.IsNullOrEmpty(ErrorMessage))
                {
                    PurchaseOrder PO = NewPO();
                    PO.Lead = UserManager.GetUserRef(values["Lead"]);
                    PO.PODate = values.ContainsKey("PODate") ? DateTime.Parse(values["PODate"]) : DateTime.Now;
                    PO.Origin = Common.GetOrigin(values["Origin"]);
                    PO.Size = Common.GetSize(values["Size"]);
                    PO.Vessel = Common.GetVessel(values["Vessel"]);
                    PO.TargetDays = values.ContainsKey("TargetDays") ? int.Parse(values["TargetDays"]) : 0;
                    PO.Supplier = Common.GetSupplier(values["Supplier"]);
                    PO.TermsOfPayment = values["PaymentTerms"];
                    PO.BufferQuantityMin = values.ContainsKey("BufferMin") ? decimal.Parse(values["BufferMin"]) : 10;
                    PO.BufferQuantityMax = values.ContainsKey("BufferMax") ? decimal.Parse(values["BufferMax"]) : 10;
                    for (int i = 1; i <= 10; i++)
                    {
                        if (values.ContainsKey("Customer_" + i.ToString()))
                            if (values["Customer_" + i.ToString()] != "0")
                            {
                                PODetail pod = NewPODetail(PO);
                                pod.Customer = CustomerManager.GetCustomerRef(values["Customer_" + i.ToString()]);
                                pod.Quantity = values.ContainsKey("Quantity_" + i.ToString()) ? decimal.Parse(values["Quantity_" + i.ToString()]) : 0;
                                pod.Rate = values.ContainsKey("Rate_" + i.ToString()) ? decimal.Parse(values["Rate_" + i.ToString()]) : 0;
                                pod.AllowedWaistage = values.ContainsKey("AllowedWastage_" + i.ToString()) ? decimal.Parse(values["AllowedWastage_" + i.ToString()]) : 0;
                                pod.CostPerTon = values.ContainsKey("CostPerTon_" + i.ToString()) ? decimal.Parse(values["CostPerTon_" + i.ToString()]) : 0;
                                pod.TargetDate = values.ContainsKey("TargetDate_" + i.ToString()) ? DateTime.Parse(values["TargetDate_" + i.ToString()]) : DateTime.MaxValue;
                                PO.PODetailsList.Add(pod);
                            }
                    }
                    PO = PurchaseDataManager.CalculatePO(PO);
                    if (PO.isValid)
                        return PO;
                }
                throw new Exception("Enter required fields: " + ErrorMessage);
            }
            catch (Exception ex)
            {
                ExceptionHandler.Error(ex.Message);
            }
            return null;
        }
        public static GRN ValidateCreateGRNForm(Dictionary<string, string> values)
        {
            try
            {
                string ErrorMessage = string.Empty;

                if (values["PO"] == "0") ErrorMessage += "PO, ";
                if (values["Customer"] == "0") ErrorMessage += "Customer, ";
                if (string.IsNullOrEmpty(values["GRNDate"])) ErrorMessage += "GRN Date, ";
                if (values["Store"] == "0") ErrorMessage += "Store, ";
                if (values["Quantity"] == "0") ErrorMessage += "Quantity, ";

                PurchaseOrder po = GetPO(values["PO"]);
                if (po != null && po.PODetailsList.Count > 0)
                {
                    foreach (PODetail pod in po.PODetailsList)
                    {
                        if (pod.Customer.Id.ToString() == values["Customer"])
                        {
                            GRN Grn = NewGRN();
                            Grn.PO = new Reference() { Id = po.Id, Name = po.PONumber };
                            Grn.PODetail = new Reference() { Id = pod.Id, Name = po.PONumber };
                            Grn.GRNDate = values.ContainsKey("GRNDate") ? DateTime.Parse(values["GRNDate"]) : DateTime.Now;
                            if (values.ContainsKey("Store"))
                                Grn.Store = StoreManager.GetStoreRef(values["Store"].ToString());
                            Grn.Quantity = values.ContainsKey("Quantity") ? decimal.Parse(values["Quantity"]) : 0;
                            Grn.InvoiceNo = values.ContainsKey("Invoice") ? values["Invoice"] : "";
                            Grn.AdjPrice = values.ContainsKey("Price") ? decimal.Parse(values["Price"]) : 0;
                            Grn.Remarks = values.ContainsKey("Remarks") ? values["Remarks"] : "";

                            return Grn;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.Error(ex.Message);
            }
            return null;
        }
        public static DutyClear ValidateCreateDutyClearForm(Dictionary<string, string> values)
        {
            try
            {
                string ErrorMessage = string.Empty;

                if (values["PO"] == "0") ErrorMessage += "PO, ";
                if (values["Customer"] == "0") ErrorMessage += "Customer, ";
                if (string.IsNullOrEmpty(values["DCLDate"])) ErrorMessage += "DCL Date, ";
                if (values["Store"] == "0") ErrorMessage += "Store, ";
                if (values["Quantity"] == "0") ErrorMessage += "Quantity, ";

                PurchaseOrder po = GetPO(values["PO"]);
                if (po != null && po.PODetailsList.Count > 0)
                {
                    foreach (PODetail pod in po.PODetailsList)
                    {
                        if (pod.Customer.Id.ToString() == values["Customer"])
                        {
                            DutyClear Dcl = NewDCL();
                            Dcl.PO = new Reference() { Id = po.Id, Name = po.PONumber };
                            Dcl.PODetail = new Reference() { Id = pod.Id, Name = po.PONumber };
                            Dcl.DCLDate = values.ContainsKey("DCLDate") ? DateTime.Parse(values["DCLDate"]) : DateTime.Now;
                            if (values.ContainsKey("Store"))
                                Dcl.Store = StoreManager.GetStoreRef(values["Store"].ToString());
                            Dcl.Quantity = values.ContainsKey("Quantity") ? decimal.Parse(values["Quantity"]) : 0;
                            Dcl.Remarks = values.ContainsKey("Remarks") ? values["Remarks"] : "";

                            return Dcl;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.Error(ex.Message);
            }
            return null;

        }
        #endregion

        #region Create Operations
        public static bool CreatePO(PurchaseOrder PO)
        {
            try
            {
                Guid poid = PurchaseDataManager.CreatePO(POMap.reMapPOData(PO));
                if (poid != Guid.Empty)
                {
                    PO.Id = poid;
                    foreach (PODetail pod in PO.PODetailsList)
                    {
                        if (pod.Id == Guid.Empty)
                        {
                            pod.PO = new Reference() { Id = PO.Id, Name = PO.PONumber };
                            Guid newPODId = PurchaseDataManager.CreatePODetail(POMap.reMapPODetailData(pod));
                            pod.Id = newPODId;
                        }
                        else
                        {
                            PurchaseDataManager.UpdatePODetail(POMap.reMapPODetailData(pod));
                        }
                    }
                }
                ResetCache();
                return true;

            }
            catch (Exception ex)
            {
                ExceptionHandler.Error("Something went wrong. Details: " + ex.Message, ex);
            }
            return false;
        }
        public static bool CreateGRN(GRN grn)
        {
            try
            {

                if (grn.PO != null && grn.PO.Id != Guid.Empty && grn.PODetail != null && grn.PODetail.Id != Guid.Empty)
                {
                    PurchaseDataManager.CreateGRN(POMap.reMapGRNData(grn));
                    ResetCache();
                    /*PurchaseOrder PO = GetPO(grn.PO.Name);
                    PODetail POD = null;
                    foreach (PODetail pod in PO.PODetailsList)
                    {
                        if (pod.Id == grn.PODetail.Id)
                            POD = pod;
                    }
                    if (POD != null)
                    {
                        GRN Grn = NewGRN();
                        Grn.PO = new Reference() { Id = PO.Id, Name = PO.PONumber };
                        Grn.PODetail = new Reference() { Id = POD.Id, Name = PO.PONumber };
                        Grn.GRNDate = keyvalues.ContainsKey("GRNDate") ? DateTime.Parse(keyvalues["GRNDate"]) : DateTime.Now;
                        if (keyvalues.ContainsKey("Store"))
                            Grn.Store = StoreManager.GetStoreRef(keyvalues["Store"].ToString());
                        Grn.Quantity = keyvalues.ContainsKey("Quantity") ? decimal.Parse(keyvalues["Quantity"]) : 0;
                        Grn.InvoiceNo = keyvalues.ContainsKey("Invoice") ? keyvalues["Invoice"] : "";
                        Grn.AdjPrice = keyvalues.ContainsKey("Price") ? decimal.Parse(keyvalues["Price"]) : 0;
                        Grn.Remarks = keyvalues.ContainsKey("Remarks") ? keyvalues["Remarks"] : "";

                        POD.GRNsList.Add(Grn);
                        PurchaseDataManager.CalculatePO(PO);
                        if (PO.isValid)
                            PurchaseDataManager.CreateGRN(POMap.reMapGRNData(Grn));
                        else
                        {
                            ExceptionHandler.Error("Something went wrong! PO Quantity is not valid.");
                            PurchaseDataManager.ResetCache();
                            return null;
                        }
                        PurchaseDataManager.ResetCache();
                        return Grn;
                    }*/
                }
                return true;

            }
            catch (Exception ex)
            {
                ExceptionHandler.Error("Something went wrong. Details: " + ex.Message, ex);
            }
            return false;
        }
        public static bool CreateDutyClear(DutyClear dcl)
        {
            try
            {

                if (dcl.PO != null && dcl.PO.Id != Guid.Empty && dcl.PODetail != null && dcl.PODetail.Id != Guid.Empty)
                {
                    Guid dclId = PurchaseDataManager.CreateDCL(POMap.reMapDCLData(dcl));
                    dcl.Id = dclId;
                    PurchaseOrder po = GetPO(dcl.PO.Name);
                    StoreDataManager.CreateStockMovement(StoreMap.reMapStockMovementData(po, dcl)); //TODO
                    ResetCache();
                }
                return true;
            }
            catch (Exception ex)
            {
                ExceptionHandler.Error("Something went wrong. Details: " + ex.Message, ex);
            }
            return false;


            /*string poNumber = keyvalues.ContainsKey("PO") ? keyvalues["PO"] : "";
            string cusId = keyvalues.ContainsKey("Customer") ? keyvalues["Customer"] : "";

            if (poNumber != "" && cusId != "")
            {
                PurchaseOrder PO = GetPO(poNumber);
                PODetail POD = null;
                foreach (PODetail pod in PO.PODetailsList)
                {
                    if (pod.Customer.Id == new Guid(cusId))
                        POD = pod;
                }
                if (POD != null)
                {
                    if (POD.DutyClearsList == null)
                        POD.DutyClearsList = new List<DutyClear>();
                    DutyClear Dcl = NewDCL();
                    Dcl.PO = new Reference() { Id = PO.Id, Name = PO.PONumber };
                    Dcl.PODetail = new Reference() { Id = POD.Id, Name = PO.PONumber };
                    if (keyvalues.ContainsKey("Store"))
                        Dcl.Store = StoreManager.GetStoreRef(keyvalues["Store"].ToString());
                    Dcl.Quantity = keyvalues.ContainsKey("Quantity") ? decimal.Parse(keyvalues["Quantity"]) : 0;
                    Dcl.Remarks = keyvalues.ContainsKey("Remarks") ? keyvalues["Remarks"] : "";
                    POD.DutyClearsList.Add(Dcl);

                    PurchaseDataManager.CalculatePO(PO);
                    if (PO.isValid)
                    {
                        PurchaseDataManager.CreateDCL(POMap.reMapDCLData(Dcl));
                        //todo stock movement
                    }
                    else
                    {
                        ExceptionHandler.Error("Something went wrong! PO Quantity is not valid.");
                        PurchaseDataManager.ResetCache();
                        return null;
                    }
                    PurchaseDataManager.ResetCache();
                    return Dcl;
                }
            }
            return null;*/
        }

        #endregion

        #region Update Operations
        public static PurchaseOrder UpdatePO(Dictionary<string, string> keyvalues)
        {
            try
            {
                string PONumber = keyvalues["ponumber"];
                PurchaseOrder PO = GetPO(PONumber);
                PO.Lead = UserManager.GetUserRef(keyvalues["Lead"]);
                PO.PODate = keyvalues.ContainsKey("PODate") ? DateTime.Parse(keyvalues["PODate"]) : DateTime.Now;
                PO.Origin = Common.GetOrigin(keyvalues["Origin"]);
                PO.Size = Common.GetSize(keyvalues["Size"]);
                PO.Vessel = Common.GetVessel(keyvalues["Vessel"]);
                PO.TargetDays = keyvalues.ContainsKey("TargetDays") ? int.Parse(keyvalues["TargetDays"]) : 0;
                PO.Supplier = Common.GetSupplier(keyvalues["Supplier"]);
                PO.TermsOfPayment = keyvalues["PaymentTerms"];
                PO.BufferQuantityMin = keyvalues.ContainsKey("BufferMin") ? decimal.Parse(keyvalues["BufferMin"]) : 10;
                PO.BufferQuantityMax = keyvalues.ContainsKey("BufferMax") ? decimal.Parse(keyvalues["BufferMax"]) : 10;
                List<PODetail> updatedPOD = new List<PODetail>();
                for (int i = 1; i <= 10; i++)
                {
                    if (keyvalues.ContainsKey("Customer_" + i.ToString()))
                        if (keyvalues["Customer_" + i.ToString()] != "0")
                        {
                            PODetail pod = null;
                            if (keyvalues.ContainsKey("PODetailId_" + i.ToString()) && keyvalues["PODetailId_" + i.ToString()] != "")
                                foreach (PODetail pd in PO.PODetailsList)
                                {
                                    if (pd.Id == new Guid(keyvalues["PODetailId_" + i.ToString()]))
                                    {
                                        pod = pd;
                                    }
                                }
                            if (pod == null)
                                pod = NewPODetail(PO);
                            pod.Customer = CustomerManager.GetCustomerRef(keyvalues["Customer_" + i.ToString()]);
                            pod.Quantity = keyvalues.ContainsKey("Quantity_" + i.ToString()) ? decimal.Parse(keyvalues["Quantity_" + i.ToString()]) : 0;
                            pod.Rate = keyvalues.ContainsKey("Rate_" + i.ToString()) ? decimal.Parse(keyvalues["Rate_" + i.ToString()]) : 0;
                            pod.AllowedWaistage = keyvalues.ContainsKey("AllowedWastage_" + i.ToString()) ? decimal.Parse(keyvalues["AllowedWastage_" + i.ToString()]) : 0;
                            pod.CostPerTon = keyvalues.ContainsKey("CostPerTon_" + i.ToString()) ? decimal.Parse(keyvalues["CostPerTon_" + i.ToString()]) : 0;
                            pod.TargetDate = keyvalues.ContainsKey("TargetDate_" + i.ToString()) ? DateTime.Parse(keyvalues["TargetDate_" + i.ToString()]) : DateTime.MaxValue;
                            updatedPOD.Add(pod);
                        }
                }
                PO.PODetailsList = updatedPOD;
                PO.ModifiedOn = DateTime.Now;
                PO.ModifiedBy = UserManager.GetUserRef(Common.CurrentUser.Id.ToString());
                SavePO(PO);
                return PO;
            }
            catch (Exception ex)
            {
                ExceptionHandler.Error("Something went wrong. Details: " + ex.Message, ex);
            }
            return null;
        }
        public static PurchaseOrder GetPObyId(Guid Id)
        {
            try
            {
                List<PurchaseOrder> AllPO = ReadAllPO();
                foreach (PurchaseOrder PO in AllPO)
                    if (PO.Id == Id)
                        return PO;
            }
            catch (Exception ex)
            {
                throw new Exception("Error! Get all PO from DataBase", ex);
            }
            return null;
        }


        public static bool UpdateGRN(GRN grn)
        {
            try
            {

                if (grn.Id != Guid.Empty && grn.PO != null && grn.PO.Id != Guid.Empty && grn.PODetail != null && grn.PODetail.Id != Guid.Empty)
                {
                    PurchaseDataManager.UpdateGRN(POMap.reMapGRNData(grn));
                    ResetCache();
                }
                return true;
            }
            catch (Exception ex)
            {
                ExceptionHandler.Error("Something went wrong. Details: " + ex.Message, ex);
            }
            return false;

            /* string PONumber = keyvalues["ponumber"];
             string GRNNumber = keyvalues["grnnumber"];
             string GRNId = keyvalues["grnid"];

             PurchaseOrder PO = GetPO(PONumber);
             GRN Grn = GetGRN(GRNNumber);
             if (PO != null && Grn != null)
             {
                 Grn.GRNDate = keyvalues.ContainsKey("GRNDate") ? DateTime.Parse(keyvalues["GRNDate"]) : DateTime.Now;
                 Grn.PODetail = new Reference() { Id = new Guid(keyvalues["Customer"]), Name = PO.PONumber };
                 Grn.Store = StoreManager.GetStoreRef(keyvalues["Store"]);
                 Grn.Quantity = keyvalues.ContainsKey("Quantity") ? decimal.Parse(keyvalues["Quantity"]) : 0;
                 Grn.InvoiceNo = keyvalues.ContainsKey("Invoice") ? keyvalues["Invoice"] : "";
                 Grn.AdjPrice = keyvalues.ContainsKey("Price") ? decimal.Parse(keyvalues["Price"]) : 0;
                 Grn.Remarks = keyvalues.ContainsKey("Remarks") ? keyvalues["Remarks"] : "";
                 Grn.ModifiedOn = DateTime.Now;
                 Grn.ModifiedBy = UserManager.GetUserRef(Common.CurrentUser.Id.ToString());
                 foreach (PODetail pod in PO.PODetailsList)
                 {
                     foreach (GRN grn in pod.GRNsList)
                         if (grn.Id.ToString() == GRNId || grn.GRNNumber == GRNNumber)
                         {
                             pod.GRNsList.Remove(grn);
                             break;
                         }
                     if (pod.Id == Grn.PODetail.Id)
                     {
                         pod.GRNsList.Add(Grn);
                         break;
                     }
                 }
                 PurchaseDataManager.CalculatePO(PO);
                 if (PO.isValid)
                     PurchaseDataManager.UpdateGRN(POMap.reMapGRNData(Grn));
                 else
                 {
                     ExceptionHandler.Error("Something went wrong! PO Quantity is not valid.");
                     return null;
                 }
                 ResetCache();

                 return Grn;
             }
             return null;*/
        }
        public static bool UpdateDCL(DutyClear dcl)
        {
            try
            {

                if (dcl.Id != Guid.Empty && dcl.PO != null && dcl.PO.Id != Guid.Empty && dcl.PODetail != null && dcl.PODetail.Id != Guid.Empty)
                {
                    PurchaseDataManager.UpdateDCL(POMap.reMapDCLData(dcl));
                    ResetCache();
                }
                return true;
            }
            catch (Exception ex)
            {
                ExceptionHandler.Error("Something went wrong. Details: " + ex.Message, ex);
            }
            return false;
            /*string PONumber = keyvalues["ponumber"];
            string DCLNumber = keyvalues["dclnumber"];
            string DCLId = keyvalues["dclid"];

            PurchaseOrder PO = GetPO(PONumber);
            DutyClear Dcl = GetDCL(DCLNumber);
            if (PO != null && Dcl != null)
            {
                Dcl.DCLDate = keyvalues.ContainsKey("DCLDate") ? DateTime.Parse(keyvalues["DCLDate"]) : DateTime.Now;
                Dcl.PODetail = new Reference() { Id = new Guid(keyvalues["Customer"]), Name = PO.PONumber };
                Dcl.Store = StoreManager.GetStoreRef(keyvalues["Store"]);
                Dcl.Quantity = keyvalues.ContainsKey("Quantity") ? decimal.Parse(keyvalues["Quantity"]) : 0;
                Dcl.Remarks = keyvalues.ContainsKey("Remarks") ? keyvalues["Remarks"] : "";
                Dcl.ModifiedOn = DateTime.Now;
                Dcl.ModifiedBy = UserManager.GetUserRef(Common.CurrentUser.Id.ToString());
                foreach (PODetail pod in PO.PODetailsList)
                {
                    foreach (DutyClear dcl in pod.DutyClearsList)
                        if (dcl.Id.ToString() == DCLId || dcl.DCLNumber == DCLNumber)
                        {
                            pod.DutyClearsList.Remove(dcl);
                            break;
                        }
                    if (pod.Id == Dcl.PODetail.Id)
                    {
                        pod.DutyClearsList.Add(Dcl);
                        break;
                    }
                }

                PurchaseDataManager.CalculatePO(PO);
                if (PO.isValid)
                    PurchaseDataManager.UpdateDCL(POMap.reMapDCLData(Dcl));
                else
                {
                    ExceptionHandler.Error("Something went wrong! PO Quantity is not valid.");
                    return null;
                }
                ResetCache();


                return Dcl;
            }
            return null;*/
        }
        public static PurchaseOrder SubmitPO(Dictionary<string, string> keyvalues)
        {
            string PONumber = keyvalues["ponumber"];
            PurchaseOrder PO = GetPO(PONumber);
            PO.Status = POStatus.PendingApproval;
            SavePO(PO);
            return PO;
        }
        public static PurchaseOrder ApprovePO(Dictionary<string, string> keyvalues)
        {
            string PONumber = keyvalues["ponumber"];
            PurchaseOrder PO = GetPO(PONumber);
            PO.Status = POStatus.InProcess;
            PO.ApprovedDate = DateTime.Now;
            PO.ApprovedBy = new Reference() { Id = Common.CurrentUser.Id, Name = Common.CurrentUser.Name };
            SavePO(PO);
            return PO;
        }
        public static void CompleteOrder(PurchaseOrder PO)
        {

            PO = PurchaseDataManager.CalculatePO(PO);
            if (PO.isValid)
            {
                PO.Status = POStatus.Completed;
                SavePO(PO);
            }
            else
                ExceptionHandler.Error("PO not valid");
        }
        #endregion

        #region Move to Purchase DataManager
        private static PurchaseOrder NewPO()
        {
            PurchaseOrder PO = new PurchaseOrder();
            Reference currUser = new Reference() { Id = Common.CurrentUser.Id, Name = Common.CurrentUser.Name };
            PO.Id = Guid.Empty;
            PO.Status = POStatus.Created;
            PO.CreatedOn = PO.ModifiedOn = PO.PODate = DateTime.Now;
            PO.CreatedBy = PO.ModifiedBy = PO.Lead = currUser;
            PO.CompletedOn = PO.ApprovedDate = DateTime.MinValue;
            PO.ApprovedBy = null;
            PO.PONumber = GenerateNextPONumber();
            PO.Origin = PO.Size = PO.Vessel = PO.Supplier = new Item() { Index = 0, Value = "" };
            PO.TargetDays = 0;
            PO.TermsOfPayment = "";
            PO.PODetailsList = new List<PODetail>();
            return PO;
        }
        private static PODetail NewPODetail(PurchaseOrder PO)
        {
            PODetail pod = new PODetail();
            pod.Id = Guid.Empty;
            pod.PO = new Reference() { Id = PO.Id, Name = PO.PONumber };
            pod.Customer = new Reference() { Id = Guid.Empty, Name = "" };
            pod.Quantity = 0;
            pod.Rate = 0;
            pod.CostPerTon = 0;
            pod.AllowedWaistage = 0;
            pod.TargetDate = DateTime.MaxValue;
            pod.Remarks = "";
            pod.GRNsList = new List<GRN>();
            pod.DutyClearsList = new List<DutyClear>();
            return pod;
        }
        private static GRN NewGRN()
        {
            GRN Grn = new GRN();
            Reference currUser = new Reference() { Id = Common.CurrentUser.Id, Name = Common.CurrentUser.Name };
            Grn.Id = Guid.Empty;
            Grn.Status = GRNStatus.Recieved;
            Grn.CreatedOn = Grn.ModifiedOn = Grn.GRNDate = Grn.CompletedOn = DateTime.Now;
            Grn.CreatedBy = Grn.ModifiedBy = currUser;
            Grn.GRNNumber = GenerateNextGRNNumber();
            Grn.PO = Grn.PODetail = Grn.Store = new Reference() { Id = Guid.Empty, Name = "" };
            Grn.InvoiceNo = Grn.Remarks = "";
            Grn.AdjPrice = 0;
            return Grn;
        }
        private static DutyClear NewDCL()
        {
            DutyClear dcl = new DutyClear();
            Reference currUser = new Reference() { Id = Common.CurrentUser.Id, Name = Common.CurrentUser.Name };
            dcl.Id = Guid.Empty;
            dcl.CreatedOn = dcl.ModifiedOn = dcl.DCLDate = DateTime.Now;
            dcl.CreatedBy = dcl.ModifiedBy = currUser;
            dcl.DCLNumber = GenerateNextDCLNumber();
            dcl.PO = dcl.PODetail = dcl.Store = new Reference() { Id = Guid.Empty, Name = "" };
            return dcl;
        }
        private static string GenerateNextGRNNumber()
        {
            int lNumber = 1001;
            foreach (GRN grn in AllGRNs)
            {
                string grnNumber = grn.GRNNumber.Replace("GRN-", "");
                int No = int.Parse(grnNumber);
                if (No > lNumber)
                    lNumber = No;
            }
            lNumber++;
            return "GRN-" + lNumber.ToString();
        }
        private static string GenerateNextDCLNumber()
        {
            int lNumber = 1001;
            foreach (DutyClear dcl in AllDCLs)
            {
                string grnNumber = dcl.DCLNumber.Replace("DCL-", "");
                int No = int.Parse(grnNumber);
                if (No > lNumber)
                    lNumber = No;
            }
            lNumber++;
            return "DCL-" + lNumber.ToString();
        }
        private static string GenerateNextPONumber()
        {
            int lNumber = 1001;
            foreach (PurchaseOrder po in AllPOs)
            {
                string poNumber = po.PONumber.Replace("PO-", "");
                int poNo = int.Parse(poNumber);
                if (poNo > lNumber)
                    lNumber = poNo;
            }
            lNumber++;
            return "PO-" + lNumber.ToString();
        }

        #endregion

    }
}