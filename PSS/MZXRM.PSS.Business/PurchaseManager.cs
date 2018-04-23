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
        public static string ValidateCreateGRNForm(Dictionary<string, string> values)
        {
            try
            {
                foreach (KeyValuePair<string, string> keyValue in values)
                {
                    string key = keyValue.Key;
                    string value = keyValue.Value;
                    if (key == "PO" && value == "0") throw new Exception("PO is required");
                    if (key == "Customer" && value == "0") throw new Exception("Customer is required");
                    if (key == "GRNDate" && string.IsNullOrEmpty(value)) throw new Exception("GRN Date is required");
                    if (key == "Store" && value == "0") throw new Exception("Store is required");
                    if (key == "Quantity" && value == "0") throw new Exception("Quantity is required");
                    //if (key == "Invoice" && string.IsNullOrEmpty(value)) throw new Exception("Supplier is required");
                    //if (key == "Price" && string.IsNullOrEmpty(value)) throw new Exception("Lead is required");
                    // if (key == "PaymentTerms" && string.IsNullOrEmpty(value)) throw new Exception("PaymentTerms is required");
                    //if (key == "Remarks" && string.IsNullOrEmpty(value)) throw new Exception("Buffer Min is required");

                }
                PurchaseOrder po = GetPO(values["PO"]);
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public static string ValidateCreateDutyClearForm(Dictionary<string, string> values)
        {
            try
            {
                foreach (KeyValuePair<string, string> keyValue in values)
                {
                    string key = keyValue.Key;
                    string value = keyValue.Value;
                    if (key == "PO" && value == "0") throw new Exception("PO is required");
                    if (key == "Customer" && value == "0") throw new Exception("Customer is required");
                    if (key == "GRNDate" && string.IsNullOrEmpty(value)) throw new Exception("GRN Date is required");
                    if (key == "Store" && value == "0") throw new Exception("Store is required");
                    if (key == "Quantity" && value == "0") throw new Exception("Quantity is required");
                    //if (key == "Invoice" && string.IsNullOrEmpty(value)) throw new Exception("Supplier is required");
                    //if (key == "Price" && string.IsNullOrEmpty(value)) throw new Exception("Lead is required");
                    // if (key == "PaymentTerms" && string.IsNullOrEmpty(value)) throw new Exception("PaymentTerms is required");
                    //if (key == "Remarks" && string.IsNullOrEmpty(value)) throw new Exception("Buffer Min is required");

                }
                //PurchaseOrder po = GetPO(values["PO"]);
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        #endregion

        #region Create Operations
        public static bool CreatePO(PurchaseOrder PO)
        {
            try
            {
                Guid poid = PurchaseDataManager.CreatePO(PO);
                if (poid != Guid.Empty)
                {
                    PO.Id = poid;
                    foreach (PODetail pod in PO.PODetailsList)
                    {
                        if (pod.Id == Guid.Empty)
                        {
                            pod.PO = new Reference() { Id = PO.Id, Name = PO.PONumber };
                            Guid newPODId = PurchaseDataManager.CreatePODetail(pod);
                            pod.Id = newPODId;
                        }
                        else
                        {
                            PurchaseDataManager.UpdatePODetail(pod);
                        }
                    }
                }
                return true;

            }
            catch (Exception ex)
            {
                ExceptionHandler.Error("Something went wrong. Details: " + ex.Message, ex);
            }
            return false;
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
                    Grn.GRNDate = keyvalues.ContainsKey("GRNDate") ? DateTime.Parse(keyvalues["GRNDate"]) : DateTime.Now;
                    if (keyvalues.ContainsKey("Store"))
                        Grn.Store = Common.GetStore(keyvalues["Store"]);
                    Grn.Quantity = keyvalues.ContainsKey("Quantity") ? decimal.Parse(keyvalues["Quantity"]) : 0;
                    Grn.InvoiceNo = keyvalues.ContainsKey("Invoice") ? keyvalues["Invoice"] : "";
                    Grn.AdjPrice = keyvalues.ContainsKey("Price") ? decimal.Parse(keyvalues["Price"]) : 0;
                    Grn.Remarks = keyvalues.ContainsKey("Remarks") ? keyvalues["Remarks"] : "";

                    POD.GRNsList.Add(Grn);
                    PurchaseDataManager.CalculatePO(PO);
                    if (PO.isValid)
                        PurchaseDataManager.CreateGRN(Grn);
                    else
                    {
                        ExceptionHandler.Error("Something went wrong! PO Quantity is not valid.");
                        PurchaseDataManager.ResetCache();
                        return null;
                    }
                    PurchaseDataManager.ResetCache();
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
                        Dcl.Store = StoreDataManager.GetStoreRef(keyvalues["Store"].ToString());
                    Dcl.Quantity = keyvalues.ContainsKey("Quantity") ? decimal.Parse(keyvalues["Quantity"]) : 0;
                    Dcl.Remarks = keyvalues.ContainsKey("Remarks") ? keyvalues["Remarks"] : "";
                    POD.DutyClearsList.Add(Dcl);

                    PurchaseDataManager.CalculatePO(PO);
                    if (PO.isValid)
                        PurchaseDataManager.CreateDCL(PO, Dcl);
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
                PurchaseDataManager.CalculatePO(PO);
                if (PO.isValid)
                    PurchaseDataManager.UpdateGRN(Grn);
                else
                {
                    ExceptionHandler.Error("Something went wrong! PO Quantity is not valid.");
                    return null;
                }
                PurchaseDataManager.ResetCache();

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

                PurchaseDataManager.CalculatePO(PO);
                if (PO.isValid)
                    PurchaseDataManager.UpdateDCL(Dcl);
                else
                {
                    ExceptionHandler.Error("Something went wrong! PO Quantity is not valid.");
                    return null;
                }
                PurchaseDataManager.ResetCache();


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
            PO.ApprovedDate = DateTime.Now;
            PO.ApprovedBy = new Reference() { Id = Common.CurrentUser.Id, Name = Common.CurrentUser.Name };
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