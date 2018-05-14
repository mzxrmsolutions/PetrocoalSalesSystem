using MZXRM.PSS.Data;
using MZXRM.PSS.DataManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MZXRM.PSS.Business.DBMap
{
   public class POMap
    {
        
        public static List<PurchaseOrder> MapPOData(DataTable DTpo, DataTable dTpod, DataTable dTgrn, DataTable dTdcl)
        {
            List<PurchaseOrder> AllPOs = new List<PurchaseOrder>();
            foreach (DataRow DRpo in DTpo.Rows)
            {
                PurchaseOrder PO = new PurchaseOrder();

                PO.Id = DRpo["Id"] != null ? new Guid(DRpo["Id"].ToString()) : Guid.Empty;
                PO.Status = DRpo["Status"] != null ? MapPOStatus(DRpo["Status"].ToString()) : POStatus.Created;
                PO.CreatedOn = DRpo["CreatedOn"] != DBNull.Value ? DateTime.Parse(DRpo["CreatedOn"].ToString()) : DateTime.MinValue;
                PO.CreatedBy = DRpo["CreatedBy"] != null ? UserManager.GetUserRef(DRpo["CreatedBy"].ToString()) : UserManager.GetDefaultRef();
                PO.ModifiedOn = DRpo["ModifiedOn"] != DBNull.Value ? DateTime.Parse(DRpo["ModifiedOn"].ToString()) : DateTime.MinValue;
                PO.ModifiedBy = DRpo["ModifiedBy"] != null ? UserManager.GetUserRef(DRpo["ModifiedBy"].ToString()) : UserManager.GetDefaultRef();

                PO.CompletedOn = DRpo["CompletedOn"] != DBNull.Value ? DateTime.Parse(DRpo["CompletedOn"].ToString()) : DateTime.MinValue;
                PO.Lead = DRpo["LeadId"] != null ? UserManager.GetUserRef(DRpo["LeadId"].ToString()) : UserManager.GetDefaultRef();
                PO.ApprovedDate = DRpo["ApprovedDate"] != DBNull.Value ? DateTime.Parse(DRpo["ApprovedDate"].ToString()) : DateTime.MinValue;
                PO.ApprovedBy = DRpo["ApprovedBy"] != null ? UserManager.GetUserRef(DRpo["LeadId"].ToString()) : UserManager.GetDefaultRef();

                PO.PONumber = DRpo["PONumber"] != null ? DRpo["PONumber"].ToString() : "";
                PO.PODate = DRpo["PODate"] != DBNull.Value ? DateTime.Parse(DRpo["PODate"].ToString()) : DateTime.MinValue;
                PO.Origin = DRpo["Origin"] != null ? CommonDataManager.GetOrigin(DRpo["Origin"].ToString()) : CommonDataManager.GetDefaultRef();
                PO.Size = DRpo["Size"] != null ? CommonDataManager.GetSize(DRpo["Size"].ToString()) : CommonDataManager.GetDefaultRef();
                PO.Vessel = DRpo["Vessel"] != null ? CommonDataManager.GetVessel(DRpo["Vessel"].ToString()) : CommonDataManager.GetDefaultRef();

                PO.TargetDays = DRpo["TargetDays"] != null ? int.Parse(DRpo["TargetDays"].ToString()) : 0;
                PO.Supplier = DRpo["Supplier"] != null ? CommonDataManager.GetSupplier(DRpo["Supplier"].ToString()) : CommonDataManager.GetDefaultRef();
                PO.TermsOfPayment = DRpo["TermsOfPayment"] != null ? DRpo["TermsOfPayment"].ToString() : "";
                PO.BufferQuantityMax = DRpo["BufferQuantityMax"] != null ? decimal.Parse(DRpo["BufferQuantityMax"].ToString()) : 10;
                PO.BufferQuantityMin = DRpo["BufferQuantityMin"] != null ? decimal.Parse(DRpo["BufferQuantityMin"].ToString()) : 10;

                PO.PODetailsList = new List<PODetail>();
                foreach (DataRow DRpod in dTpod.Rows)
                {
                    Guid poId = DRpod["POId"] != null ? new Guid(DRpod["POId"].ToString()) : Guid.Empty;
                    if (poId != Guid.Empty && poId == PO.Id)
                    {
                        PODetail PODetail = new PODetail();
                        PODetail.Id = DRpod["Id"] != null ? new Guid(DRpod["Id"].ToString()) : Guid.Empty;
                        PODetail.PO = new Reference() { Id = PO.Id, Name = PO.PONumber };
                        PODetail.Customer = DRpod["CustomerId"] != null ? CustomerManager.GetCustomerRef(DRpod["CustomerId"].ToString()) : CustomerManager.GetDefaultRef();
                        PODetail.Quantity = DRpod["Quantity"] != null ? decimal.Parse(DRpod["Quantity"].ToString()) : 0;
                        PODetail.Rate = DRpod["Rate"] != null ? decimal.Parse(DRpod["Rate"].ToString()) : 0;
                        PODetail.CostPerTon = DRpod["CostPerTon"] != null ? decimal.Parse(DRpod["CostPerTon"].ToString()) : 0;
                        PODetail.AllowedWaistage = DRpod["AllowedWastage"] != null ? decimal.Parse(DRpod["AllowedWastage"].ToString()) : 0;
                        PODetail.TargetDate = DRpod["TargetDate"] != DBNull.Value ? DateTime.Parse(DRpod["TargetDate"].ToString()) : DateTime.MinValue;
                        PODetail.Remarks = DRpod["Remarks"] != null ? DRpod["Remarks"].ToString() : "";

                        PODetail.GRNsList = new List<GRN>();
                        foreach (DataRow DRgrn in dTgrn.Rows)
                        {
                            Guid podId = DRgrn["PODetailId"] != null ? new Guid(DRgrn["PODetailId"].ToString()) : Guid.Empty;
                            if (podId != Guid.Empty && podId == PODetail.Id)
                            {
                                GRN Grn = new GRN();
                                Grn.Id = DRgrn["Id"] != null ? new Guid(DRgrn["Id"].ToString()) : Guid.Empty;
                                Grn.Status = DRgrn["Status"] != null ? MapGRNStatus(DRgrn["Status"].ToString()) : GRNStatus.Recieved;
                                Grn.CreatedOn = DRgrn["CreatedOn"] != DBNull.Value ? DateTime.Parse(DRgrn["CreatedOn"].ToString()) : DateTime.MinValue;
                                Grn.CreatedBy = DRgrn["CreatedBy"] != null ? UserManager.GetUserRef(DRgrn["CreatedBy"].ToString()) : UserManager.GetDefaultRef();
                                Grn.ModifiedOn = DRgrn["ModifiedOn"] != DBNull.Value ? DateTime.Parse(DRgrn["ModifiedOn"].ToString()) : DateTime.MinValue;
                                Grn.ModifiedBy = DRgrn["ModifiedBy"] != null ? UserManager.GetUserRef(DRgrn["ModifiedBy"].ToString()) : UserManager.GetDefaultRef();
                                Grn.CompletedOn = DRgrn["CompletedOn"] != DBNull.Value ? DateTime.Parse(DRgrn["CompletedOn"].ToString()) : DateTime.MinValue;

                                Grn.GRNNumber = DRgrn["GRNNumber"] != null ? DRgrn["GRNNumber"].ToString() : "";
                                Grn.GRNDate = DRgrn["GRNDate"] != DBNull.Value ? DateTime.Parse(DRgrn["GRNDate"].ToString()) : DateTime.MinValue;
                                Grn.PO = new Reference() { Id = PO.Id, Name = PO.PONumber };
                                Grn.PODetail = new Reference() { Id = PODetail.Id, Name = PO.PONumber };
                                Grn.Store = DRgrn["Store"] != null ? StoreManager.GetStoreRef(DRgrn["Store"].ToString()) : StoreManager.GetDefaultRef();
                                Grn.InvoiceNo = DRgrn["InvoiceNo"] != null ? DRgrn["InvoiceNo"].ToString() : "";
                                Grn.AdjPrice = DRgrn["AdjPrice"] != null ? decimal.Parse(DRgrn["AdjPrice"].ToString()) : 0;
                                Grn.Quantity = DRgrn["Quantity"] != null ? decimal.Parse(DRgrn["Quantity"].ToString()) : 0;
                                Grn.Remarks = DRpod["Remarks"] != null ? DRpod["Remarks"].ToString() : "";

                                PODetail.GRNsList.Add(Grn);
                            }
                        }

                        PODetail.DutyClearsList = new List<DutyClear>();
                        foreach (DataRow DRdcl in dTdcl.Rows)
                        {
                            Guid podId = DRdcl["PODetailId"] != null ? new Guid(DRdcl["PODetailId"].ToString()) : Guid.Empty;
                            if (podId != Guid.Empty && podId == PODetail.Id)
                            {
                                DutyClear Dcl = new DutyClear();
                                Dcl.Id = DRdcl["Id"] != null ? new Guid(DRdcl["Id"].ToString()) : Guid.Empty;
                                Dcl.CreatedOn = DRdcl["CreatedOn"] != DBNull.Value ? DateTime.Parse(DRdcl["CreatedOn"].ToString()) : DateTime.MinValue;
                                Dcl.CreatedBy = DRdcl["CreatedBy"] != null ? UserManager.GetUserRef(DRdcl["CreatedBy"].ToString()) : UserManager.GetDefaultRef();
                                Dcl.ModifiedOn = DRdcl["ModifiedOn"] != DBNull.Value ? DateTime.Parse(DRdcl["ModifiedOn"].ToString()) : DateTime.MinValue;
                                Dcl.ModifiedBy = DRdcl["ModifiedBy"] != null ? UserManager.GetUserRef(DRdcl["ModifiedBy"].ToString()) : UserManager.GetDefaultRef();

                                Dcl.DCLNumber = DRdcl["DCLNumber"] != null ? DRdcl["DCLNumber"].ToString() : "";
                                Dcl.DCLDate = DRdcl["DCLDate"] != DBNull.Value ? DateTime.Parse(DRdcl["DCLDate"].ToString()) : DateTime.MinValue;
                                Dcl.PO = new Reference() { Id = PO.Id, Name = PO.PONumber };
                                Dcl.PODetail = new Reference() { Id = PODetail.Id, Name = PO.PONumber };
                                Dcl.Store = DRdcl["Store"] != null ? StoreManager.GetStoreRef(DRdcl["Store"].ToString()) : StoreManager.GetDefaultRef();
                                Dcl.Quantity = DRdcl["Quantity"] != null ? decimal.Parse(DRdcl["Quantity"].ToString()) : 0;
                                Dcl.Remarks = DRpod["Remarks"] != null ? DRpod["Remarks"].ToString() : "";

                                PODetail.DutyClearsList.Add(Dcl);
                            }
                        }

                        PO.PODetailsList.Add(PODetail);
                    }
                }

                AllPOs.Add(PO);
            }
            return AllPOs;
        }
        public static Dictionary<string, object> reMapPOData(PurchaseOrder PO)
        {
            Dictionary<string, object> keyValues = new Dictionary<string, object>();
            if (PO.Id != Guid.Empty)
                keyValues.Add("@id", PO.Id);
            keyValues.Add("@Status", reMapPOStatus(PO.Status));
            if (PO.CreatedOn == DateTime.MinValue)
                keyValues.Add("@CreatedOn", DBNull.Value);
            else
                keyValues.Add("@CreatedOn", PO.CreatedOn);
            keyValues.Add("@CreatedBy", PO.CreatedBy == null ? Guid.Empty : PO.CreatedBy.Id);
            if (PO.ModifiedOn == DateTime.MinValue)
                keyValues.Add("@ModifiedOn", DBNull.Value);
            else
                keyValues.Add("@ModifiedOn", PO.ModifiedOn);
            keyValues.Add("@ModifiedBy", PO.ModifiedBy == null ? Guid.Empty : PO.ModifiedBy.Id);
            if (PO.CompletedOn == DateTime.MinValue)
                keyValues.Add("@CompletedOn", DBNull.Value);
            else
                keyValues.Add("@CompletedOn", PO.CompletedOn);
            keyValues.Add("@LeadId", PO.Lead == null ? Guid.Empty : PO.Lead.Id);
            if (PO.ApprovedDate == DateTime.MinValue)
                keyValues.Add("@ApprovedDate", DBNull.Value);
            else
                keyValues.Add("@ApprovedDate", PO.ApprovedDate);
            keyValues.Add("@ApprovedBy", PO.ApprovedBy == null ? Guid.Empty : PO.ApprovedBy.Id);
            keyValues.Add("@PONumber", PO.PONumber);
            if (PO.PODate == DateTime.MinValue)
                keyValues.Add("@PODate", DBNull.Value);
            else
                keyValues.Add("@PODate", PO.PODate);
            keyValues.Add("@Origin", PO.Origin == null ? 0 : PO.Origin.Index);
            keyValues.Add("@Size", PO.Size == null ? 0 : PO.Size.Index);
            keyValues.Add("@Vessel", PO.Vessel == null ? 0 : PO.Vessel.Index);
            keyValues.Add("@TargetDays", PO.TargetDays);
            keyValues.Add("@Supplier", PO.Supplier == null ? 0 : PO.Supplier.Index);
            keyValues.Add("@TermsOfPayment", PO.TermsOfPayment);
            keyValues.Add("@BufferQuantityMax", PO.BufferQuantityMax);
            keyValues.Add("@BufferQuantityMin", PO.BufferQuantityMin);
            return keyValues;
        }
        public static Dictionary<string, object> reMapDCLData(DutyClear DCL)
        {
            Dictionary<string, object> keyValues = new Dictionary<string, object>();
            if (DCL.Id != Guid.Empty)
                keyValues.Add("@id", DCL.Id);
            keyValues.Add("@Status", 1);
            if (DCL.CreatedOn == DateTime.MinValue)
                keyValues.Add("@CreatedOn", DBNull.Value);
            else
                keyValues.Add("@CreatedOn", DCL.CreatedOn);
            keyValues.Add("@CreatedBy", DCL.CreatedBy == null ? Guid.Empty : DCL.CreatedBy.Id);
            if (DCL.ModifiedOn == DateTime.MinValue)
                keyValues.Add("@ModifiedOn", DBNull.Value);
            else
                keyValues.Add("@ModifiedOn", DCL.ModifiedOn);
            keyValues.Add("@ModifiedBy", DCL.ModifiedBy == null ? Guid.Empty : DCL.ModifiedBy.Id);
            //if (DCL.CompletedOn == DateTime.MinValue)
            keyValues.Add("@CompletedOn", DBNull.Value);
            //else
            //    keyValues.Add("@CompletedOn", GRN.CompletedOn);
            keyValues.Add("@DCLNumber", DCL.DCLNumber);
            if (DCL.DCLDate == DateTime.MinValue)
                keyValues.Add("@DCLDate", DBNull.Value);
            else
                keyValues.Add("@DCLDate", DCL.DCLDate);
            keyValues.Add("@PODetailId", DCL.PODetail == null ? Guid.Empty : DCL.PODetail.Id);
            keyValues.Add("@Store", DCL.Store == null ? Guid.Empty : DCL.Store.Id);
            keyValues.Add("@Quantity", DCL.Quantity);
            keyValues.Add("@Remarks", DCL.Remarks);
            return keyValues;
        }

        public static Dictionary<string, object> reMapGRNData(GRN GRN)
        {
            Dictionary<string, object> keyValues = new Dictionary<string, object>();
            if (GRN.Id != Guid.Empty)
                keyValues.Add("@id", GRN.Id);
            keyValues.Add("@Status", GRN.Status);
            if (GRN.CreatedOn == DateTime.MinValue)
                keyValues.Add("@CreatedOn", DBNull.Value);
            else
                keyValues.Add("@CreatedOn", GRN.CreatedOn);
            keyValues.Add("@CreatedBy", GRN.CreatedBy == null ? Guid.Empty : GRN.CreatedBy.Id);
            if (GRN.ModifiedOn == DateTime.MinValue)
                keyValues.Add("@ModifiedOn", DBNull.Value);
            else
                keyValues.Add("@ModifiedOn", GRN.ModifiedOn);
            keyValues.Add("@ModifiedBy", GRN.ModifiedBy == null ? Guid.Empty : GRN.ModifiedBy.Id);
            if (GRN.CompletedOn == DateTime.MinValue)
                keyValues.Add("@CompletedOn", DBNull.Value);
            else
                keyValues.Add("@CompletedOn", GRN.CompletedOn);
            keyValues.Add("@GRNNumber", GRN.GRNNumber);
            if (GRN.GRNDate == DateTime.MinValue)
                keyValues.Add("@GRNDate", DBNull.Value);
            else
                keyValues.Add("@GRNDate", GRN.GRNDate);
            keyValues.Add("@PODetailId", GRN.PODetail == null ? Guid.Empty : GRN.PODetail.Id);
            keyValues.Add("@Store", GRN.Store == null ? Guid.Empty : GRN.Store.Id);
            keyValues.Add("@InvoiceNo", GRN.InvoiceNo);
            keyValues.Add("@AdjPrice", GRN.AdjPrice);
            keyValues.Add("@Quantity", GRN.Quantity);
            keyValues.Add("@Remarks", GRN.Remarks);
            return keyValues;
        }

        public static Dictionary<string, object> reMapPODetailData(PODetail PODetail)
        {
            Dictionary<string, object> keyValues = new Dictionary<string, object>();
            if (PODetail.Id != Guid.Empty)
                keyValues.Add("@id", PODetail.Id);
            keyValues.Add("@POId", PODetail.PO == null ? Guid.Empty : PODetail.PO.Id);
            keyValues.Add("@CustomerId", PODetail.Customer == null ? Guid.Empty : PODetail.Customer.Id);
            keyValues.Add("@Quantity", PODetail.Quantity);
            keyValues.Add("@Rate", PODetail.Rate);
            keyValues.Add("@CostPerTon", PODetail.CostPerTon);
            keyValues.Add("@AllowedWastage", PODetail.AllowedWaistage);
            keyValues.Add("@TargetDate", PODetail.TargetDate);
            keyValues.Add("@Remarks", PODetail.Remarks);
            return keyValues;
        }
        private static GRNStatus MapGRNStatus(string status)
        {
            switch (status)
            {
                case "0":
                    return GRNStatus.Cancelled;
                case "1":
                default:
                    return GRNStatus.Recieved;
                case "2":
                    return GRNStatus.PendingRecieve;
                case "3":
                    return GRNStatus.Loan;
            }
        }
        private static POStatus MapPOStatus(string status)
        {
            switch (status)
            {
                case "0":
                    return POStatus.Cancelled;
                case "1":
                default:
                    return POStatus.Created;
                case "2":
                    return POStatus.PendingApproval;
                case "3":
                    return POStatus.InProcess;
                case "4":
                    return POStatus.Completed;
            }
        }
        private static int reMapPOStatus(POStatus status)
        {
            switch (status)
            {
                case POStatus.Cancelled:
                    return 0;
                case POStatus.Created:
                default:
                    return 1;
                case POStatus.PendingApproval:
                    return 2;
                case POStatus.InProcess:
                    return 3;
                case POStatus.Completed:
                    return 4;
            }
        }


    }
}
