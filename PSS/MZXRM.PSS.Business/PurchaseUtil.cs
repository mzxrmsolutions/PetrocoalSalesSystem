using MZXRM.PSS.Common;
using MZXRM.PSS.Data;
using MZXRM.PSS.DataManager;
using System;
using System.Collections;
using System.Collections.Generic;
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
                _AllPOs = PurchaseDataManager.ReadAllPO();
                return _AllPOs;
            }
            set
            {
                _AllPOs = value;
            }
        }
        private static List<GRN> _AllGRNs;
        public static List<GRN> AllGRNs
        {
            get
            {
                //if (_AllGRNs == null || _AllGRNs.Count == 0)
                {
                    _AllGRNs = new List<GRN>();
                    foreach (PurchaseOrder po in AllPOs)
                    {
                        foreach (PODetail pod in po.PODetailsList)
                        {
                            foreach (GRN grn in pod.GRNsList)
                                _AllGRNs.Add(grn);
                        }
                    }
                }
                return _AllGRNs;
            }
            set
            {
                _AllGRNs = value;
            }
        }
        private static List<DutyClear> _AllDCLs;
        public static List<DutyClear> AllDCLs
        {
            get
            {
                //if (_AllDCLs == null || _AllDCLs.Count == 0)
                {
                    _AllDCLs = new List<DutyClear>();
                    foreach (PurchaseOrder po in AllPOs)
                    {
                        foreach (PODetail pod in po.PODetailsList)
                        {
                            foreach (DutyClear dcl in pod.DutyClearsList)
                                _AllDCLs.Add(dcl);
                        }
                    }
                }
                return _AllDCLs;
            }
            set
            {
                _AllDCLs = value;
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
            foreach (PurchaseOrder po in AllPOs)
            {
                if (po.PONumber == ponumber)
                    return po;
            }
            return null;
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
        public static string ValidateCreatePOForm(Dictionary<string, string> values)
        {
            try
            {
                foreach (KeyValuePair<string, string> keyValue in values)
                {
                    string key = keyValue.Key;
                    string value = keyValue.Value;
                    if (key == "Origin" && value == "0") throw new Exception("Origin is required");
                    if (key == "Size" && value == "0") throw new Exception("Size is required");
                    if (key == "Vessel" && value == "0") throw new Exception("Vessel is required");
                    if (key == "PODate" && string.IsNullOrEmpty(value)) throw new Exception("PODate is required");
                    if (key == "TargetDays" && string.IsNullOrEmpty(value)) throw new Exception("TargetDays is required");
                    if (key == "Supplier" && string.IsNullOrEmpty(value)) throw new Exception("Supplier is required");
                    if (key == "Lead" && string.IsNullOrEmpty(value)) throw new Exception("Lead is required");
                    // if (key == "PaymentTerms" && string.IsNullOrEmpty(value)) throw new Exception("PaymentTerms is required");
                    if (key == "BufferMin" && string.IsNullOrEmpty(value)) throw new Exception("Buffer Min is required");
                    if (key == "BufferMax" && string.IsNullOrEmpty(value)) throw new Exception("Buffer Max is required");
                }
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }
        #endregion

        #region Create Operations
        public static PurchaseOrder CreatePO(Dictionary<string, string> keyvalues)
        {
            try
            {
                PurchaseOrder PO = NewPO();
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
                for (int i = 1; i <= 10; i++)
                {
                    if (keyvalues.ContainsKey("Customer_" + i.ToString()))
                        if (keyvalues["Customer_" + i.ToString()] != "0")
                        {
                            PODetail pod = NewPODetail(PO);
                            pod.Customer = CustomerManager.GetCustomerRef(keyvalues["Customer_" + i.ToString()]);
                            pod.Quantity = keyvalues.ContainsKey("Quantity_" + i.ToString()) ? decimal.Parse(keyvalues["Quantity_" + i.ToString()]) : 0;
                            pod.Rate = keyvalues.ContainsKey("Rate_" + i.ToString()) ? decimal.Parse(keyvalues["Rate_" + i.ToString()]) : 0;
                            pod.AllowedWaistage = keyvalues.ContainsKey("AllowedWastage_" + i.ToString()) ? decimal.Parse(keyvalues["AllowedWastage_" + i.ToString()]) : 0;
                            pod.CostPerTon = keyvalues.ContainsKey("CostPerTon_" + i.ToString()) ? decimal.Parse(keyvalues["CostPerTon_" + i.ToString()]) : 0;
                            pod.TargetDate = keyvalues.ContainsKey("TargetDate_" + i.ToString()) ? DateTime.Parse(keyvalues["TargetDate_" + i.ToString()]) : DateTime.MaxValue;
                            PO.PODetailsList.Add(pod);
                        }
                }
                //PO = CalculatePO(PO);
                //PurchaseDataManager.SavePO(PO);
                PurchaseDataManager.SavePO(PO);
                return PO;
            }
            catch (Exception ex)
            {
                ExceptionHandler.Error("Something went wrong. Details: " + ex.Message, ex);
            }
            return null;
        }
        public static GRN CreateGRN(Dictionary<string, string> keyvalues)
        {

            string poNumber = keyvalues.ContainsKey("PO") ? keyvalues["PO"] : "";
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
                    GRN Grn = NewGRN();
                    Grn.PO = new Reference() { Id = PO.Id, Name = PO.PONumber };
                    Grn.PODetail = new Reference() { Id = POD.Id, Name = PO.PONumber };
                    if (keyvalues.ContainsKey("Store"))
                        Grn.Store = Common.GetStore(keyvalues["Store"]);
                    Grn.Quantity = keyvalues.ContainsKey("Quantity") ? decimal.Parse(keyvalues["Quantity"]) : 0;
                    Grn.InvoiceNo = keyvalues.ContainsKey("Invoice") ? keyvalues["Invoice"] : "";
                    Grn.AdjPrice = keyvalues.ContainsKey("Price") ? decimal.Parse(keyvalues["Price"]) : 0;
                    Grn.Remarks = keyvalues.ContainsKey("Remarks") ? keyvalues["Remarks"] : "";

                    POD.GRNsList.Add(Grn);
                    PurchaseDataManager.SavePO(PO);
                    return Grn;
                }
            }
            return null;
        }
        public static DutyClear CreateDutyClear(Dictionary<string, string> keyvalues)
        {
            string poNumber = keyvalues.ContainsKey("PO") ? keyvalues["PO"] : "";
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
                        Dcl.Store = Common.GetStore(keyvalues["Store"]);
                    Dcl.Quantity = keyvalues.ContainsKey("Quantity") ? decimal.Parse(keyvalues["Quantity"]) : 0;
                    Dcl.Remarks = keyvalues.ContainsKey("Remarks") ? keyvalues["Remarks"] : "";
                    POD.DutyClearsList.Add(Dcl);

                    PurchaseDataManager.SavePO(PO);
                    return Dcl;
                }
            }
            return null;
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
                PurchaseDataManager.SavePO(PO);
                return PO;
            }
            catch (Exception ex)
            {
                ExceptionHandler.Error("Something went wrong. Details: " + ex.Message, ex);
            }
            return null;
        }
        public static GRN UpdateGRN(Dictionary<string, string> keyvalues)
        {
            string PONumber = keyvalues["ponumber"];
            string GRNNumber = keyvalues["grnnumber"];
            string GRNId = keyvalues["grnid"];

            PurchaseOrder PO = GetPO(PONumber);
            GRN Grn = GetGRN(GRNNumber);
            if (PO != null && Grn != null)
            {
                Grn.GRNDate = keyvalues.ContainsKey("GRNDate") ? DateTime.Parse(keyvalues["GRNDate"]) : DateTime.Now;
                Grn.PODetail = new Reference() { Id = new Guid(keyvalues["Customer"]), Name = PO.PONumber };
                Grn.Store = Common.GetStore(keyvalues["Store"]);
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
                PurchaseDataManager.SavePO(PO);
                return Grn;
            }
            return null;
        }
        public static DutyClear UpdateDCL(Dictionary<string, string> keyvalues)
        {
            string PONumber = keyvalues["ponumber"];
            string DCLNumber = keyvalues["dclnumber"];
            string DCLId = keyvalues["dclid"];

            PurchaseOrder PO = GetPO(PONumber);
            DutyClear Dcl = GetDCL(DCLNumber);
            if (PO != null && Dcl != null)
            {
                Dcl.DCLDate = keyvalues.ContainsKey("DCLDate") ? DateTime.Parse(keyvalues["DCLDate"]) : DateTime.Now;
                Dcl.PODetail = new Reference() { Id = new Guid(keyvalues["Customer"]), Name = PO.PONumber };
                Dcl.Store = Common.GetStore(keyvalues["Store"]);
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
                PurchaseDataManager.SavePO(PO);
                return Dcl;
            }
            return null;
        }
        public static PurchaseOrder SubmitPO(Dictionary<string, string> keyvalues)
        {
            string PONumber = keyvalues["ponumber"];
            PurchaseOrder PO = GetPO(PONumber);
            PO.Status = POStatus.PendingApproval;
            PurchaseDataManager.SavePO(PO);
            return PO;
        }
        public static PurchaseOrder ApprovePO(Dictionary<string, string> keyvalues)
        {
            string PONumber = keyvalues["ponumber"];
            PurchaseOrder PO = GetPO(PONumber);
            PO.Status = POStatus.InProcess;
            PurchaseDataManager.SavePO(PO);
            return PO;
        }
        public static void CompleteOrder(PurchaseOrder PO)
        {

            PO = PurchaseDataManager.CalculatePO(PO);
            if (PO.isValid)
            {
                PO.Status = POStatus.Completed;
                PurchaseDataManager.SavePO(PO);
            }
            else
                ExceptionHandler.Error("PO not valid");
        }
        #endregion

        #region Move to Purchase Manager
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