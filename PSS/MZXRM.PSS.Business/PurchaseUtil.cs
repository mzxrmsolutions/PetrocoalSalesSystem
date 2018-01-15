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
    public static class PurchaseUtil
    {
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

        public static PurchaseOrder CreatePO(Dictionary<string, string> keyvalues)
        {
            PurchaseOrder PO = NewPO();
            PO.Lead = UserUtil.GetUserRef(keyvalues["Lead"]);
            PO.PODate = keyvalues.ContainsKey("PODate") ? DateTime.Parse(keyvalues["PODate"]) : DateTime.Now;
            PO.Origin = Common.GetOrigin(keyvalues["Origin"]);
            PO.Size = Common.GetSize(keyvalues["Size"]);
            PO.Vessel = Common.GetVessel(keyvalues["Vessel"]);
            PO.TargetDays = keyvalues.ContainsKey("TargetDays") ? int.Parse(keyvalues["TargetDays"]) : 0;
            PO.Supplier = Common.GetSupplier(keyvalues["Supplier"]);
            PO.TermsOfPayment = keyvalues["PaymentTerms"];

            for (int i = 1; i <= 10; i++)
            {
                if (keyvalues.ContainsKey("Customer_" + i.ToString()))
                    if (keyvalues["Customer_" + i.ToString()] != "0")
                    {
                        PODetail pod = NewPODetail(PO);
                        pod.Customer = CustomerUtil.GetCustomerRef(keyvalues["Customer_" + i.ToString()]);
                        pod.Quantity = keyvalues.ContainsKey("Quantity_" + i.ToString()) ? decimal.Parse(keyvalues["Quantity_" + i.ToString()]) : 0;
                        pod.Rate = keyvalues.ContainsKey("Rate_" + i.ToString()) ? decimal.Parse(keyvalues["Rate_" + i.ToString()]) : 0;
                        pod.AllowedWaistage = keyvalues.ContainsKey("AllowedWastage_" + i.ToString()) ? decimal.Parse(keyvalues["AllowedWastage_" + i.ToString()]) : 0;
                        pod.CostPerTon = keyvalues.ContainsKey("CostPerTon_" + i.ToString()) ? decimal.Parse(keyvalues["CostPerTon_" + i.ToString()]) : 0;
                        pod.TargetDate = keyvalues.ContainsKey("TargetDate_" + i.ToString()) ? DateTime.Parse(keyvalues["TargetDate_" + i.ToString()]) : DateTime.MaxValue;
                        PO.PODetailsList.Add(pod);
                    }
            }
            PO = CalculatePO(PO);
            //PurchaseDataManager.SavePO(PO);
            PurchaseDataManager.CreatePO(PO);
            return PO;
        }
        public static PurchaseOrder UpdatePO(Dictionary<string, string> keyvalues)
        {
            string PONumber = keyvalues["ponumber"];
            PurchaseOrder PO = GetPO(PONumber);
            PO.Lead = UserUtil.GetUserRef(keyvalues["Lead"]);
            PO.PODate = keyvalues.ContainsKey("PODate") ? DateTime.Parse(keyvalues["PODate"]) : DateTime.Now;
            PO.Origin = Common.GetOrigin(keyvalues["Origin"]);
            PO.Size = Common.GetSize(keyvalues["Size"]);
            PO.Vessel = Common.GetVessel(keyvalues["Vessel"]);
            PO.TargetDays = keyvalues.ContainsKey("TargetDays") ? int.Parse(keyvalues["TargetDays"]) : 0;
            PO.Supplier = Common.GetSupplier(keyvalues["Supplier"]);
            PO.TermsOfPayment = keyvalues["PaymentTerms"];
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
                        pod.Customer = CustomerUtil.GetCustomerRef(keyvalues["Customer_" + i.ToString()]);
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
            PO.ModifiedBy = UserUtil.GetUserRef(Common.CurrentUser.Id.ToString());
            PO = CalculatePO(PO);
            PurchaseDataManager.SavePO(PO);
            return PO;
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
                Grn.ModifiedBy = UserUtil.GetUserRef(Common.CurrentUser.Id.ToString());
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
                PO = CalculatePO(PO);
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
                Dcl.ModifiedBy = UserUtil.GetUserRef(Common.CurrentUser.Id.ToString());
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
                PO = CalculatePO(PO);
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
            PO = CalculatePO(PO);
            PurchaseDataManager.SavePO(PO);
            return PO;
        }
        public static PurchaseOrder ApprovePO(Dictionary<string, string> keyvalues)
        {
            string PONumber = keyvalues["ponumber"];
            PurchaseOrder PO = GetPO(PONumber);
            PO.Status = POStatus.InProcess;
            PO = CalculatePO(PO);
            PurchaseDataManager.SavePO(PO);
            return PO;
        }
        private static PODetail NewPODetail(PurchaseOrder PO)
        {
            PODetail pod = new PODetail();
            pod.Id = Guid.NewGuid();
            pod.PONumber = PO.PONumber;
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
        private static PurchaseOrder NewPO()
        {
            PurchaseOrder PO = new PurchaseOrder();
            Reference currUser = new Reference() { Id = Common.CurrentUser.Id, Name = Common.CurrentUser.Name };
            PO.Id = Guid.NewGuid();
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
        public static void CompleteOrder(PurchaseOrder PO)
        {
            PO.Status = POStatus.Completed;
            PO = CalculatePO(PO);
            PurchaseDataManager.SavePO(PO);
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
                    Grn.PODetail = new Reference() { Id = POD.Id, Name = POD.PONumber };
                    if (keyvalues.ContainsKey("Store"))
                        Grn.Store = Common.GetStore(keyvalues["Store"]);
                    Grn.Quantity = keyvalues.ContainsKey("Quantity") ? decimal.Parse(keyvalues["Quantity"]) : 0;
                    Grn.InvoiceNo = keyvalues.ContainsKey("Invoice") ? keyvalues["Invoice"] : "";
                    Grn.AdjPrice = keyvalues.ContainsKey("Price") ? decimal.Parse(keyvalues["Price"]) : 0;
                    Grn.Remarks = keyvalues.ContainsKey("Remarks") ? keyvalues["Remarks"] : "";

                    POD.GRNsList.Add(Grn);
                    PO = CalculatePO(PO);
                    PurchaseDataManager.SavePO(PO);
                    return Grn;
                }
            }
            return null;
        }
        private static GRN NewGRN()
        {
            GRN Grn = new GRN();
            Reference currUser = new Reference() { Id = Common.CurrentUser.Id, Name = Common.CurrentUser.Name };
            Grn.Id = Guid.NewGuid();
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
            dcl.Id = Guid.NewGuid();
            dcl.CreatedOn = dcl.ModifiedOn = dcl.DCLDate = DateTime.Now;
            dcl.CreatedBy = dcl.ModifiedBy = currUser;
            dcl.DCLNumber = GenerateNextDCLNumber();
            dcl.PO = dcl.PODetail = dcl.Store = new Reference() { Id = Guid.Empty, Name = "" };
            return dcl;
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
                    Dcl.PODetail = new Reference() { Id = POD.Id, Name = POD.PONumber };
                    if (keyvalues.ContainsKey("Store"))
                        Dcl.Store = Common.GetStore(keyvalues["Store"]);
                    Dcl.Quantity = keyvalues.ContainsKey("Quantity") ? decimal.Parse(keyvalues["Quantity"]) : 0;
                    Dcl.Remarks = keyvalues.ContainsKey("Remarks") ? keyvalues["Remarks"] : "";
                    POD.DutyClearsList.Add(Dcl);

                    PO = CalculatePO(PO);
                    PurchaseDataManager.SavePO(PO);
                    return Dcl;
                }
            }
            return null;
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
        private static PurchaseOrder CalculatePO(PurchaseOrder PO)
        {
            if (PO == null)
                return null;
            if (PO.Id == Guid.Empty) PO.Id = Guid.NewGuid();
            PO.TotalQuantity = 0;
            PO.Received = 0;
            PO.DutyCleared = 0;
            foreach (PODetail pod in PO.PODetailsList)
            {
                pod.Received = 0;
                pod.DutyCleared = 0;
                pod.Wastage = 0;

                foreach (GRN grn in pod.GRNsList)
                {
                    pod.Received += grn.Quantity;
                }
                foreach (DutyClear dc in pod.DutyClearsList)
                {
                    pod.DutyCleared += dc.Quantity;
                }
                pod.Wastage = pod.DutyCleared * pod.AllowedWaistage / 100;
                pod.Remaining = pod.Quantity - pod.Received;
                pod.DutyRemaining = pod.Received - pod.DutyCleared;

                PO.TotalQuantity += pod.Quantity;
                PO.Received += pod.Received;
                PO.DutyCleared += pod.DutyCleared;
            }

            return PO;
        }
    }
}