using MZXRM.PSS.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MZXRM.PSS.DataManager
{
    public class DataMap
    {
        //ADDED BY KASHIF ABBAS ON 13TH MARCH 2018 TO REMAPDO
        public static Dictionary<string, object> reMapDOData(DeliveryOrder DO)
        {
            Dictionary<string, object> keyValues = new Dictionary<string, object>();
            if (!String.IsNullOrEmpty(DO.Id.ToString()) && DO.Id != 0)
            {
                keyValues.Add("@Id", DO.Id);
            }

            if (!String.IsNullOrEmpty(DO.DONumber))
            {
                keyValues.Add("@DONumber", DO.DONumber);
            }
            keyValues.Add("@StoreId", DO.Store.Id);
            keyValues.Add("@SOId", DO.SaleOrder.Index);
            keyValues.Add("@LeadId", DO.Lead.Id);
            keyValues.Add("@Status", DO.Status);
            if (DO.CompletedOn != null && DO.CompletedOn != DateTime.MinValue)
            {
                keyValues.Add("@CompletedOn", DO.CompletedOn);
            }
            else
            {
                keyValues.Add("@CompletedOn", DBNull.Value);
            }
            keyValues.Add("@DODate", DO.DODate);
            keyValues.Add("@Quantity", DO.Quantity);
            keyValues.Add("@LiftingStartDate", DO.LiftingStartDate);
            keyValues.Add("@LiftingEndDate", DO.LiftingEndDate);
            keyValues.Add("@DeliveryDestination", DO.DeliveryDestination);
            keyValues.Add("@TransporterId", DO.Transportor.Index);

            if (DO.ApprovedBy != null)
            {
                keyValues.Add("@ApprovedBy", DO.ApprovedBy.Id);
            }
            else
            {
                keyValues.Add("@ApprovedBy", DBNull.Value);
            }
            if (DO.ApprovedDate != null && DO.ApprovedDate != DateTime.MinValue)
            {
                keyValues.Add("@ApprovedDate", DO.ApprovedDate);
            }
            else
            {
                keyValues.Add("@ApprovedDate", DBNull.Value);
            }
            keyValues.Add("@DumperRate", DO.DumperRate);
            keyValues.Add("@FreightPaymentTerms", DO.FreightPaymentTerms);
            keyValues.Add("@FreightPerTon", DO.FreightPerTon);
            keyValues.Add("@FreightTaxPerTon", DO.FreightTaxPerTon);
            keyValues.Add("@FreightComissionPSL", DO.FreightComissionPSL);
            keyValues.Add("@FreightComissionAgent", DO.FreightComissionAgent);
            if (DO.CreatedOn != null && DO.CreatedOn != DateTime.MinValue)
            {
                keyValues.Add("@CreatedOn", DO.CreatedOn);
            }
            if (DO.CreatedBy != null)
            {
                keyValues.Add("@CreatedBy", DO.CreatedBy.Id);
            }
            keyValues.Add("@ModifiedBy", DO.ModifiedBy.Id);
            keyValues.Add("@ModifiedOn", DO.ModifiedOn);
            keyValues.Add("@Remarks", DO.Remarks);
            return keyValues;
        }
        public static Dictionary<string, object> reMapDCData(DeliveryChalan DC)
        {
            Dictionary<string, object> keyValues = new Dictionary<string, object>();
            if (DC.Id != 0)
            {
                keyValues.Add("@Id", DC.Id);
            }
            keyValues.Add("@DOId", DC.DeliveryOrder.Index);
            keyValues.Add("@LeadId", DC.Lead.Id);
            keyValues.Add("@TransporterId", DC.Transporter.Index);
            keyValues.Add("@Status", DC.Status);

            if (!String.IsNullOrEmpty(DC.DCNumber))
            {
                keyValues.Add("@DCNumber", DC.DCNumber);
            }

            keyValues.Add("@DCDate", DC.DCDate);
            keyValues.Add("@Quantity", DC.Quantity);
            keyValues.Add("@TruckNo", DC.TruckNo);
            keyValues.Add("@BiltyNo", DC.BiltyNo);
            keyValues.Add("@SlipNo", DC.SlipNo);
            keyValues.Add("@Weight", DC.Weight);
            keyValues.Add("@NetWeight", DC.NetWeight);
            keyValues.Add("@DriverName", DC.DriverName);
            keyValues.Add("@DriverPhone", DC.DriverPhone);

            if (DC.CreatedOn != null && DC.CreatedOn != DateTime.MinValue)
            {
                keyValues.Add("@CreatedOn", DC.CreatedOn);
            }
            if (DC.CreatedBy != null)
            {
                keyValues.Add("@CreatedBy", DC.CreatedBy.Id);
            }
            keyValues.Add("@ModifiedBy", DC.ModifiedBy.Id);
            keyValues.Add("@ModifiedOn", DC.ModifiedOn);
            keyValues.Add("@Remarks", DC.Remarks);
            return keyValues;
        }

        public static Dictionary<string, object> reMapCustData(Customer c)
        {
            Dictionary<string, object> keyValues = new Dictionary<string, object>();
            if (c.Id != Guid.Empty)
                keyValues.Add("@id", c.Id);
            keyValues.Add("@Status", c.Status);
            if (c.CreatedOn == DateTime.MinValue)
                keyValues.Add("@CreatedOn", DBNull.Value);
            else
                keyValues.Add("@CreatedOn", c.CreatedOn);
            keyValues.Add("@CreatedBy", c.CreatedBy == null ? Guid.Empty : c.CreatedBy.Id);
            if (c.ModifiedOn == DateTime.MinValue)
                keyValues.Add("@ModifiedOn", DBNull.Value);
            else
                keyValues.Add("@ModifiedOn", c.ModifiedOn);
            keyValues.Add("@ModifiedBy", c.ModifiedBy == null ? Guid.Empty : c.ModifiedBy.Id);
            keyValues.Add("@Lead", c.Lead == null ? Guid.Empty : c.Lead.Id);
            keyValues.Add("@FullName", c.Name);
            keyValues.Add("@ShortName", c.ShortName);
            keyValues.Add("@NTN", c.NTN);
            keyValues.Add("@STRN", c.STRN);
            keyValues.Add("@Address", c.Address);
            keyValues.Add("@InvoiceAddress", c.InvoiceAddress);
            keyValues.Add("@Email", c.Email);
            keyValues.Add("@Phone", c.Phone);
            keyValues.Add("@ContactPerson", c.ContactPerson);
            keyValues.Add("@HeadOffice", c.HeadOffice);
            keyValues.Add("@Remarks", c.Remarks);

            return keyValues;
        }

        public static List<StockMovement> MapStockMovementData(DataTable dt)
        {
            List<StockMovement> StMovements = new List<StockMovement>();
            return StMovements;
        }

        public static List<PurchaseOrder> MapPOData(DataTable DTpo, DataTable dTpod, DataTable dTgrn, DataTable dTdcl)
        {
            List<PurchaseOrder> AllPOs = new List<PurchaseOrder>();
            foreach (DataRow DRpo in DTpo.Rows)
            {
                PurchaseOrder PO = new PurchaseOrder();

                PO.Id = DRpo["Id"] != null ? new Guid(DRpo["Id"].ToString()) : Guid.Empty;
                PO.Status = DRpo["Status"] != null ? MapPOStatus(DRpo["Status"].ToString()) : POStatus.Created;
                PO.CreatedOn = DRpo["CreatedOn"] != DBNull.Value ? DateTime.Parse(DRpo["CreatedOn"].ToString()) : DateTime.MinValue;
                PO.CreatedBy = DRpo["CreatedBy"] != null ? UserDataManager.GetUserRef(DRpo["CreatedBy"].ToString()) : UserDataManager.GetDefaultRef();
                PO.ModifiedOn = DRpo["ModifiedOn"] != DBNull.Value ? DateTime.Parse(DRpo["ModifiedOn"].ToString()) : DateTime.MinValue;
                PO.ModifiedBy = DRpo["ModifiedBy"] != null ? UserDataManager.GetUserRef(DRpo["ModifiedBy"].ToString()) : UserDataManager.GetDefaultRef();

                PO.CompletedOn = DRpo["CompletedOn"] != DBNull.Value ? DateTime.Parse(DRpo["CompletedOn"].ToString()) : DateTime.MinValue;
                PO.Lead = DRpo["LeadId"] != null ? UserDataManager.GetUserRef(DRpo["LeadId"].ToString()) : UserDataManager.GetDefaultRef();
                PO.ApprovedDate = DRpo["ApprovedDate"] != DBNull.Value ? DateTime.Parse(DRpo["ApprovedDate"].ToString()) : DateTime.MinValue;
                PO.ApprovedBy = DRpo["ApprovedBy"] != null ? UserDataManager.GetUserRef(DRpo["LeadId"].ToString()) : UserDataManager.GetDefaultRef();

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
                        PODetail.Customer = DRpod["CustomerId"] != null ? CustomerDataManager.GetCustRef(DRpod["CustomerId"].ToString()) : CustomerDataManager.GetDefaultRef();
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
                                Grn.CreatedBy = DRgrn["CreatedBy"] != null ? UserDataManager.GetUserRef(DRgrn["CreatedBy"].ToString()) : UserDataManager.GetDefaultRef();
                                Grn.ModifiedOn = DRgrn["ModifiedOn"] != DBNull.Value ? DateTime.Parse(DRgrn["ModifiedOn"].ToString()) : DateTime.MinValue;
                                Grn.ModifiedBy = DRgrn["ModifiedBy"] != null ? UserDataManager.GetUserRef(DRgrn["ModifiedBy"].ToString()) : UserDataManager.GetDefaultRef();
                                Grn.CompletedOn = DRgrn["CompletedOn"] != DBNull.Value ? DateTime.Parse(DRgrn["CompletedOn"].ToString()) : DateTime.MinValue;

                                Grn.GRNNumber = DRgrn["GRNNumber"] != null ? DRgrn["GRNNumber"].ToString() : "";
                                Grn.GRNDate = DRgrn["GRNDate"] != DBNull.Value ? DateTime.Parse(DRgrn["GRNDate"].ToString()) : DateTime.MinValue;
                                Grn.PO = new Reference() { Id = PO.Id, Name = PO.PONumber };
                                Grn.PODetail = new Reference() { Id = PODetail.Id, Name = PO.PONumber };
                                Grn.Store = DRgrn["Store"] != null ? StoreDataManager.GetStoreRef(DRgrn["Store"].ToString()) : StoreDataManager.GetDefaultRef();
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
                                Dcl.CreatedBy = DRdcl["CreatedBy"] != null ? UserDataManager.GetUserRef(DRdcl["CreatedBy"].ToString()) : UserDataManager.GetDefaultRef();
                                Dcl.ModifiedOn = DRdcl["ModifiedOn"] != DBNull.Value ? DateTime.Parse(DRdcl["ModifiedOn"].ToString()) : DateTime.MinValue;
                                Dcl.ModifiedBy = DRdcl["ModifiedBy"] != null ? UserDataManager.GetUserRef(DRdcl["ModifiedBy"].ToString()) : UserDataManager.GetDefaultRef();

                                Dcl.DCLNumber = DRdcl["DCLNumber"] != null ? DRdcl["DCLNumber"].ToString() : "";
                                Dcl.DCLDate = DRdcl["DCLDate"] != DBNull.Value ? DateTime.Parse(DRdcl["DCLDate"].ToString()) : DateTime.MinValue;
                                Dcl.PO = new Reference() { Id = PO.Id, Name = PO.PONumber };
                                Dcl.PODetail = new Reference() { Id = PODetail.Id, Name = PO.PONumber };
                                Dcl.Store = DRdcl["Store"] != null ? StoreDataManager.GetStoreRef(DRdcl["Store"].ToString()) : StoreDataManager.GetDefaultRef();
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

        public static Dictionary<string, object> reMapStockMovementSTOutData(StoreTransfer ST)
        {
            Dictionary<string, object> keyValues = new Dictionary<string, object>();


            keyValues.Add("@Store", ST.FromStoreId.Id);
            keyValues.Add("@CustomerId", ST.Customer.Id);
            keyValues.Add("@Type", StMovType.StoreMovement);
            keyValues.Add("@Quantity", ST.Quantity);
            keyValues.Add("@InOut", false);
            keyValues.Add("@Reference", ST.STNumber);
            keyValues.Add("@Vessel", ST.Vessel.Index);
            keyValues.Add("@Origin", ST.Origin.Index);
            keyValues.Add("@Size", ST.Size.Index);
            keyValues.Add("@Remarks", ST.Remarks);

            return keyValues;
        }

        public static Dictionary<string, object> reMapStoreTransferData(StoreTransfer ST)
        {
            Dictionary<string, object> keyValues = new Dictionary<string, object>();


            keyValues.Add("@Type", ST.InOut);
            keyValues.Add("@Status", ST.Status);

            if (ST.CreatedOn == DateTime.MinValue)
                keyValues.Add("@CreatedOn", DBNull.Value);
            else
                keyValues.Add("@CreatedOn", ST.CreatedOn);
            keyValues.Add("@CreatedBy", ST.CreatedBy == null ? Guid.Empty : ST.CreatedBy.Id);
            if (ST.ModifiedOn == DateTime.MinValue)
                keyValues.Add("@ModifiedOn", DBNull.Value);
            else
                keyValues.Add("@ModifiedOn", ST.ModifiedOn);
            keyValues.Add("@ModifiedBy", ST.ModifiedBy == null ? Guid.Empty : ST.ModifiedBy.Id);
            if (ST.CompletedOn == DateTime.MinValue)
                keyValues.Add("@CompletedOn", DBNull.Value);
            else
                keyValues.Add("@CompletedOn", ST.CompletedOn);
            keyValues.Add("@LeadId", ST.LeadId.Id);
            keyValues.Add("@SMNumber", ST.STNumber);
            if (ST.STDate == DateTime.MinValue)
                keyValues.Add("@SMDate", DBNull.Value);
            else
                keyValues.Add("@SMDate", ST.STDate);
            keyValues.Add("@Origin", ST.Origin.Index);
            keyValues.Add("@Size", ST.Size.Index);
            keyValues.Add("@Vessel", ST.Vessel.Index);
            keyValues.Add("@Quantity", ST.Quantity);
            keyValues.Add("@FromStoreId", ST.FromStoreId.Id);
            keyValues.Add("@ToStoreId", ST.ToStoreId.Id);
            keyValues.Add("@VehicleNo", ST.VehicleNo);
            keyValues.Add("@BiltyNo", ST.BiltyNo);
            keyValues.Add("@BiltyDate", ST.BiltyDate);
            keyValues.Add("@RRInvoice", ST.RRInvoice);
            keyValues.Add("@CCMNumber", ST.CCMNumber);
            keyValues.Add("@Transporter", ST.Transporter.Index);
            if (ST.StoreInDate == DateTime.MinValue)
                keyValues.Add("@StoreInDate", DBNull.Value);
            else
                keyValues.Add("@StoreInDate", ST.StoreInDate);
            keyValues.Add("@StoreInQuantity", ST.StoreInQuantity);
            //keyValues.Add("@Remarks", ST.Remarks);
            return keyValues;
        }

        public static List<StoreTransfer> MapStoreTransferData(DataTable dTstore)
        {
            List<StoreTransfer> ListStores = new List<StoreTransfer>();
            foreach (DataRow dr in dTstore.Rows)
            {
                StoreTransfer mapData = new StoreTransfer();
                mapData.Id = int.Parse(dr["id"].ToString());
                mapData.InOut = (StoreMovementType)Enum.Parse(typeof(StoreMovementType), dr["Type"].ToString());
                mapData.Status = (StoreTransferStatus)Enum.Parse(typeof(StoreTransferStatus), dr["Status"].ToString());

                mapData.CreatedOn = dr["CreatedOn"] != DBNull.Value ? DateTime.Parse(dr["CreatedOn"].ToString()) : DateTime.MinValue;
                mapData.CreatedBy = dr["CreatedBy"] != null ? UserDataManager.GetUserRef(dr["CreatedBy"].ToString()) : UserDataManager.GetDefaultRef();
                mapData.ModifiedOn = dr["ModifiedOn"] != DBNull.Value ? DateTime.Parse(dr["ModifiedOn"].ToString()) : DateTime.MinValue;
                mapData.ModifiedBy = dr["ModifiedBy"] != null ? UserDataManager.GetUserRef(dr["ModifiedBy"].ToString()) : UserDataManager.GetDefaultRef();

                mapData.CompletedOn = dr["CompletedOn"] != DBNull.Value ? DateTime.Parse(dr["CompletedOn"].ToString()) : DateTime.MinValue;
                mapData.LeadId = dr["LeadId"] != null ? UserDataManager.GetUserRef(dr["LeadId"].ToString()) : UserDataManager.GetDefaultRef();
                mapData.STNumber = dr["SMNumber"] != null ? dr["SMNumber"].ToString() : "";
                mapData.STDate = dr["SMDate"] != DBNull.Value ? DateTime.Parse(dr["SMDate"].ToString()) : DateTime.MinValue;

                mapData.Origin = dr["Origin"] != null ? CommonDataManager.GetOrigin(dr["Origin"].ToString()) : CommonDataManager.GetDefaultRef();
                mapData.Size = dr["Size"] != null ? CommonDataManager.GetSize(dr["Size"].ToString()) : CommonDataManager.GetDefaultRef();
                mapData.Vessel = dr["Vessel"] != null ? CommonDataManager.GetVessel(dr["Vessel"].ToString()) : CommonDataManager.GetDefaultRef();

                mapData.Quantity = dr["Quantity"] != null ? decimal.Parse(dr["Quantity"].ToString()) : 0;
                mapData.FromStoreId = dr["FromStoreId"] != null ? StoreDataManager.GetStoreRef(dr["FromStoreId"].ToString()) : StoreDataManager.GetDefaultRef();
                mapData.ToStoreId = dr["ToStoreId"] != null ? StoreDataManager.GetStoreRef(dr["ToStoreId"].ToString()) : StoreDataManager.GetDefaultRef();

                mapData.VehicleNo = dr["VehicleNo"] != null ? dr["VehicleNo"].ToString() : "";
                mapData.BiltyNo = dr["BiltyNo"] != null ? dr["BiltyNo"].ToString() : "";
                mapData.BiltyDate = dr["BiltyDate"] != DBNull.Value ? DateTime.Parse(dr["BiltyDate"].ToString()) : DateTime.MinValue;
                mapData.RRInvoice = dr["RRInvoice"] != null ? dr["RRInvoice"].ToString() : "";
                mapData.CCMNumber = dr["CCMNumber"] != null ? dr["CCMNumber"].ToString() : "";
                mapData.Transporter = dr["Transporter"] != null ? CommonDataManager.GetTrader(dr["Transporter"].ToString()) : CommonDataManager.GetDefaultRef();
                mapData.StoreInDate = dr["StoreInDate"] != DBNull.Value ? DateTime.Parse(dr["StoreInDate"].ToString()) : DateTime.MinValue;
                mapData.StoreInQuantity = dr["StoreInQuantity"] != null ? decimal.Parse(dr["StoreInQuantity"].ToString()) : 0;

                ListStores.Add(mapData);
                mapData = null;
            }
            return ListStores;
        }

        public static Dictionary<string, object> reMapStockMovementData(PurchaseOrder PO, DutyClear DCL)
        {
            Dictionary<string, object> keyValues = new Dictionary<string, object>();


            keyValues.Add("@Store", DCL.Store.Id);
            foreach (PODetail pod in PO.PODetailsList)
            {
                if (pod.Id == DCL.PODetail.Id)
                {
                    keyValues.Add("@CustomerId", pod.Customer.Id);
                    break;
                }

            }
            keyValues.Add("@Type", StMovType.DCSuccess);
            keyValues.Add("@Quantity", DCL.Quantity);
            keyValues.Add("@InOut", false);
            keyValues.Add("@Reference", DCL.Id);
            keyValues.Add("@Date", DCL.DCLDate);
            keyValues.Add("@Vessel", PO.Vessel.Index);
            keyValues.Add("@Origin", PO.Origin.Index);
            keyValues.Add("@Size", PO.Size.Index);
            keyValues.Add("@Remarks", DCL.Remarks);

            return keyValues;
        }
        public static Dictionary<string, object> reMapStockMovementData_DC(SaleOrder SO, DeliveryOrder DO, DeliveryChalan DC)
        {
            Dictionary<string, object> keyValues = new Dictionary<string, object>();


            keyValues.Add("@Store", DO.Store.Id);
            keyValues.Add("@CustomerId", SO.Customer.Id);
            keyValues.Add("@Type", StMovType.DCSuccess);
            keyValues.Add("@Quantity", DC.Quantity);
            keyValues.Add("@InOut", false);
            keyValues.Add("@Date", DC.CreatedOn);
            keyValues.Add("@Reference", DC.DCNumber);
            keyValues.Add("@Vessel", SO.Vessel.Index);
            keyValues.Add("@Origin", SO.Origin.Index);
            keyValues.Add("@Size", SO.Size.Index);
            keyValues.Add("@Remarks", DC.Remarks);

            return keyValues;
        }
        public static List<Store> MapStoreData(DataTable dTstore, DataTable dtCustStock, DataTable dtStockMovement)
        {
            List<Store> ListStores = new List<Store>();
            foreach (DataRow dr in dTstore.Rows)
            {
                Store mapData = new Store();
                mapData.Id = new Guid(dr["id"].ToString());
                mapData.Status = (StoreStatus)Enum.Parse(typeof(StoreStatus), dr["status"].ToString());

                mapData.CreatedOn = dr["CreatedOn"] != DBNull.Value ? DateTime.Parse(dr["CreatedOn"].ToString()) : DateTime.MinValue;
                mapData.CreatedBy = dr["CreatedBy"] != null ? UserDataManager.GetUserRef(dr["CreatedBy"].ToString()) : UserDataManager.GetDefaultRef();
                mapData.ModifiedOn = dr["ModifiedOn"] != DBNull.Value ? DateTime.Parse(dr["ModifiedOn"].ToString()) : DateTime.MinValue;
                mapData.ModifiedBy = dr["ModifiedBy"] != null ? UserDataManager.GetUserRef(dr["ModifiedBy"].ToString()) : UserDataManager.GetDefaultRef();

                mapData.Name = dr["Name"].ToString();
                mapData.Location = dr["Location"].ToString();
                mapData.Capacity = decimal.Parse(dr["Capacity"].ToString());

                mapData.Stock = new List<CustomerStock>();

                foreach (DataRow drCustStock in dtCustStock.Rows)
                {
                    Guid StoreId = drCustStock["StoreId"] != null ? new Guid(drCustStock["StoreId"].ToString()) : Guid.Empty;
                    if (StoreId != Guid.Empty && StoreId == mapData.Id)
                    {
                        CustomerStock CustStock = new CustomerStock();
                        CustStock.Customer = drCustStock["CustomerId"] != null ? CustomerDataManager.GetCustRef(drCustStock["CustomerId"].ToString()) : CustomerDataManager.GetDefaultRef();
                        CustStock.Store = new Reference() { Id = mapData.Id, Name = mapData.Name };
                        CustStock.Vessel = drCustStock["Vessel"] != null ? CommonDataManager.GetVessel(drCustStock["Vessel"].ToString()) : CommonDataManager.GetDefaultRef();
                        CustStock.Origin = drCustStock["Origin"] != null ? CommonDataManager.GetOrigin(drCustStock["Origin"].ToString()) : CommonDataManager.GetDefaultRef();
                        CustStock.Size = drCustStock["Size"] != null ? CommonDataManager.GetSize(drCustStock["Size"].ToString()) : CommonDataManager.GetDefaultRef();
                        CustStock.Quantity = drCustStock["Quantity"] != null ? decimal.Parse(drCustStock["Quantity"].ToString()) : 0;
                        mapData.Stock.Add(CustStock);
                    }
                }

                mapData.ListStockMovement = new List<StockMovement>();
                foreach (DataRow drStMovement in dtStockMovement.Rows)
                {
                    Guid StoreId = drStMovement["StoreId"] != null ? new Guid(drStMovement["StoreId"].ToString()) : Guid.Empty;
                    if (StoreId != Guid.Empty && StoreId == mapData.Id)
                    {
                        StockMovement StMovement = new StockMovement();
                        StMovement.Customer = drStMovement["CustomerId"] != null ? CustomerDataManager.GetCustRef(drStMovement["CustomerId"].ToString()) : CustomerDataManager.GetDefaultRef();
                        StMovement.Store = new Reference() { Id = mapData.Id, Name = mapData.Name };
                        StMovement.Type = (StMovType)Enum.Parse(typeof(StMovType), drStMovement["Type"].ToString());
                        StMovement.HistoryRef = drStMovement["Reference"] != null ? drStMovement["Reference"].ToString() : "";
                        StMovement.Quantity = drStMovement["Quantity"] != null ? decimal.Parse(drStMovement["Quantity"].ToString()) : 0;
                        StMovement.IsIn = drStMovement["InOut"] != null ? bool.Parse(drStMovement["InOut"].ToString()) : true;
                        StMovement.Origin = drStMovement["Origin"] != null ? CommonDataManager.GetOrigin(drStMovement["Origin"].ToString()) : CommonDataManager.GetDefaultRef();
                        StMovement.Vessel = drStMovement["Vessel"] != null ? CommonDataManager.GetVessel(drStMovement["Vessel"].ToString()) : CommonDataManager.GetDefaultRef();
                        StMovement.Size = drStMovement["Size"] != null ? CommonDataManager.GetSize(drStMovement["Size"].ToString()) : CommonDataManager.GetDefaultRef();
                        mapData.ListStockMovement.Add(StMovement);
                    }
                }

                ListStores.Add(mapData);
                mapData = null;
            }
            return ListStores;
        }

        public static List<SaleOrder> MapSOData(DataTable dTso, DataTable dTdo, DataTable dTdc)
        {
            List<SaleOrder> ListSO = new List<SaleOrder>();
            foreach (DataRow dr in dTso.Rows)
            {
                SaleOrder SO = new SaleOrder();

                SO.Id = (int)dr["id"];
                SO.Status = (SOStatus)Enum.Parse(typeof(SOStatus), dr["status"].ToString());
                SO.OrderType = (SOType)Enum.Parse(typeof(SOType), dr["ordertype"].ToString());

                SO.CreatedOn = dr["CreatedOn"] != DBNull.Value ? DateTime.Parse(dr["CreatedOn"].ToString()) : DateTime.MinValue;
                SO.CreatedBy = dr["CreatedBy"] != null ? UserDataManager.GetUserRef(dr["CreatedBy"].ToString()) : UserDataManager.GetDefaultRef();
                SO.ModifiedOn = dr["ModifiedOn"] != DBNull.Value ? DateTime.Parse(dr["ModifiedOn"].ToString()) : DateTime.MinValue;
                SO.ModifiedBy = dr["ModifiedBy"] != null ? UserDataManager.GetUserRef(dr["ModifiedBy"].ToString()) : UserDataManager.GetDefaultRef();

                SO.CompletedOn = dr["CompletedOn"] != DBNull.Value ? DateTime.Parse(dr["CompletedOn"].ToString()) : DateTime.MinValue;
                SO.Lead = dr["LeadId"] != null ? UserDataManager.GetUserRef(dr["LeadId"].ToString()) : UserDataManager.GetDefaultRef();
                SO.ApprovedDate = dr["ApprovedDate"] != DBNull.Value ? DateTime.Parse(dr["ApprovedDate"].ToString()) : DateTime.MinValue;
                SO.ApprovedBy = dr["ApprovedBy"] != null ? UserDataManager.GetUserRef(dr["LeadId"].ToString()) : UserDataManager.GetDefaultRef();


                SO.SONumber = dr["SONumber"].ToString();
                SO.SODate = DateTime.Parse(dr["SODate"].ToString());
                SO.SOExpiry = DateTime.Parse(dr["SOExpiryDate"].ToString());
                SO.Customer = dr["CustomerId"] != null ? CustomerDataManager.GetCustRef(dr["CustomerId"].ToString()) : CustomerDataManager.GetDefaultRef();
                SO.PartyPONumber = dr["PartyPONumber"].ToString();
                SO.PODate = DateTime.Parse(dr["PartyPODate"].ToString());
                SO.POExpiry = DateTime.Parse(dr["PartyPOExpiryDate"].ToString());
                SO.CreditPeriod = int.Parse(dr["CreditPeriod"].ToString());
                SO.Origin = dr["OriginId"] != null ? CommonDataManager.GetOrigin(dr["OriginId"].ToString()) : CommonDataManager.GetDefaultRef();
                SO.Size = dr["SizeId"] != null ? CommonDataManager.GetSize(dr["SizeId"].ToString()) : CommonDataManager.GetDefaultRef();
                SO.Vessel = dr["VesselId"] != null ? CommonDataManager.GetVessel(dr["VesselId"].ToString()) : CommonDataManager.GetDefaultRef();
                SO.Quantity = decimal.Parse(dr["Quantity"].ToString());

                SO.LC = SO.OrderType == SOType.LC;
                SO.Tax = !(SO.AgreedTaxRate == null || SO.AgreedTaxRate.Index == 0);
                SO.AgreedRate = decimal.Parse(dr["AgreedRate"].ToString());
                SO.AgreedTaxRate = dr["TaxRateId"] != null ? CommonDataManager.GetTaxRate(dr["TaxRateId"].ToString()) : CommonDataManager.GetDefaultRef();
                SO.Trader = dr["TraderId"] != null ? CommonDataManager.GetTrader(dr["TraderId"].ToString()) : CommonDataManager.GetDefaultRef();
                SO.TraderCommission = dr["TraderId"] != null ? decimal.Parse(dr["TraderCommision"].ToString()) : 0;
                SO.SaleStation = /*dr["SaleStationId"] != null ? CommonDataManager.GetOrigin(dr["SaleStationId"].ToString()) : */CommonDataManager.GetDefaultRef(); //TODO: have to add salestatdion id
                SO.Remarks = dr["Remarks"] != null ? dr["Remarks"].ToString() : "";
                SO.PartyPOImage = dr["POScannedImage"] != null ? dr["POScannedImage"].ToString() : "";

                //POPULATING DOS
                SO.DOList = new List<DeliveryOrder>();

                #region " Populating DOs "
                foreach (DataRow drDo in dTdo.Rows)
                {
                    int soId = drDo["SOId"] != null ? int.Parse(drDo["SOId"].ToString()) : 0;
                    if (soId != 0 && soId == SO.Id)
                    {
                        DeliveryOrder DO = new DeliveryOrder();

                        DO.Id = (int)drDo["Id"];
                        DO.Store = drDo["StoreId"] != null ? StoreDataManager.GetStoreRef(drDo["StoreId"].ToString()) : StoreDataManager.GetDefaultRef();
                        DO.Lead = drDo["LeadId"] != null ? UserDataManager.GetUserRef(drDo["LeadId"].ToString()) : UserDataManager.GetDefaultRef();
                        DO.Status = (DOStatus)Enum.Parse(typeof(DOStatus), drDo["status"].ToString());

                        DO.CompletedOn = drDo["CompletedOn"] != DBNull.Value ? DateTime.Parse(drDo["CompletedOn"].ToString()) : DateTime.MinValue;
                        DO.ApprovedDate = drDo["ApprovedDate"] != DBNull.Value ? DateTime.Parse(drDo["ApprovedDate"].ToString()) : DateTime.MinValue;
                        DO.ApprovedBy = drDo["ApprovedBy"] != null ? UserDataManager.GetUserRef(drDo["LeadId"].ToString()) : UserDataManager.GetDefaultRef();


                        //TODO: Get sale order reference here
                        DO.SaleOrder = new Item() { Index = SO.Id, Value = SO.SONumber };

                        DO.DONumber = drDo["DONumber"].ToString();
                        DO.DODate = DateTime.Parse(drDo["DODate"].ToString());
                        DO.Quantity = Decimal.Parse(drDo["Quantity"].ToString());
                        DO.LiftingStartDate = DateTime.Parse(drDo["LiftingStartDate"].ToString());
                        DO.LiftingEndDate = DateTime.Parse(drDo["LiftingEndDate"].ToString());
                        DO.DeliveryDestination = drDo["DeliveryDestination"].ToString();
                        //TODO: trader and transporter are different
                        DO.Transportor = drDo["TransporterId"] != null ? CommonDataManager.GetTrader(drDo["TransporterId"].ToString()) : CommonDataManager.GetDefaultRef();
                        DO.DumperRate = Decimal.Parse(drDo["DumperRate"].ToString());
                        DO.FreightPaymentTerms = Decimal.Parse(drDo["FreightPaymentTerms"].ToString());
                        DO.FreightPerTon = Decimal.Parse(drDo["FreightPerTon"].ToString());
                        DO.FreightTaxPerTon = Decimal.Parse(drDo["FreightTaxPerTon"].ToString());
                        DO.FreightComissionPSL = Decimal.Parse(drDo["FreightComissionPSL"].ToString());
                        DO.FreightComissionAgent = Decimal.Parse(drDo["FreightComissionAgent"].ToString());
                        DO.Remarks = drDo["Remarks"] != null ? drDo["Remarks"].ToString() : "";



                        DO.CreatedOn = drDo["CreatedOn"] != DBNull.Value ? DateTime.Parse(drDo["CreatedOn"].ToString()) : DateTime.MinValue;
                        DO.CreatedBy = drDo["CreatedBy"] != null ? UserDataManager.GetUserRef(drDo["CreatedBy"].ToString()) : UserDataManager.GetDefaultRef();
                        DO.ModifiedOn = drDo["ModifiedOn"] != DBNull.Value ? DateTime.Parse(drDo["ModifiedOn"].ToString()) : DateTime.MinValue;

                        DO.ModifiedBy = drDo["ModifiedBy"] != null ? UserDataManager.GetUserRef(drDo["ModifiedBy"].ToString()) : UserDataManager.GetDefaultRef();
                        DO.DCList = new List<DeliveryChalan>();
                        #region " Populating DCs "
                        foreach (DataRow drDC in dTdc.Rows)
                        {
                            int doId = drDC["DOId"] != null ? int.Parse(drDC["DOId"].ToString()) : 0;
                            if (doId != 0 && doId == DO.Id)
                            {
                                DeliveryChalan DC = new DeliveryChalan();

                                DC.Id = (int)drDC["Id"];
                                //TODO
                                DC.DeliveryOrder = new Item() { Index = DO.Id, Value = DO.DONumber };
                                //TODO: trader and transporter are different
                                DC.Lead = drDC["LeadId"] != null ? UserDataManager.GetUserRef(drDC["LeadId"].ToString()) : UserDataManager.GetDefaultRef();
                                DC.Transporter = drDC["TransporterId"] != null ? CommonDataManager.GetTrader(drDC["TransporterId"].ToString()) : CommonDataManager.GetDefaultRef();
                                DC.Status = (DCStatus)Enum.Parse(typeof(DCStatus), drDC["status"].ToString());
                                DC.DCNumber = drDC["DCNumber"].ToString();
                                DC.DCDate = DateTime.Parse(drDC["DCDate"].ToString());
                                DC.Quantity = Decimal.Parse(drDC["Quantity"].ToString());
                                DC.TruckNo = drDC["TruckNo"].ToString();
                                DC.BiltyNo = drDC["BiltyNo"].ToString();
                                DC.SlipNo = drDC["SlipNo"].ToString();
                                DC.Weight = Decimal.Parse(drDC["Weight"].ToString());
                                DC.NetWeight = Decimal.Parse(drDC["NetWeight"].ToString());
                                DC.DriverName = drDC["DriverName"].ToString();
                                DC.DriverPhone = drDC["DriverPhone"] != null ? drDC["DriverPhone"].ToString() : "";

                                DC.Remarks = drDC["Remarks"] != null ? drDC["Remarks"].ToString() : "";
                                DC.CreatedOn = drDC["CreatedOn"] != DBNull.Value ? DateTime.Parse(drDC["CreatedOn"].ToString()) : DateTime.MinValue;
                                DC.CreatedBy = drDC["CreatedBy"] != null ? UserDataManager.GetUserRef(drDC["CreatedBy"].ToString()) : UserDataManager.GetDefaultRef();
                                DC.ModifiedOn = drDC["ModifiedOn"] != DBNull.Value ? DateTime.Parse(drDC["ModifiedOn"].ToString()) : DateTime.MinValue;
                                DC.ModifiedBy = drDC["ModifiedBy"] != null ? UserDataManager.GetUserRef(drDC["ModifiedBy"].ToString()) : UserDataManager.GetDefaultRef();

                                DO.DCList.Add(DC);
                                DC = null;
                            }
                        }
                        #endregion

                        SO.DOList.Add(DO);
                        DO = null;
                    }
                }
                #endregion

                ListSO.Add(SO);
                SO = null;
            }
            return ListSO;
        }
        public static Dictionary<string, object> reMapSOData(SaleOrder SO)
        {
            Dictionary<string, object> keyValues = new Dictionary<string, object>();

            if (!String.IsNullOrEmpty(SO.Id.ToString()) && SO.Id != 0)
            {
                keyValues.Add("@Id", SO.Id);
            }
            keyValues.Add("@Leadid", SO.Lead.Id);
            keyValues.Add("@OriginId", SO.Origin.Index);
            keyValues.Add("@SizeId", SO.Size.Index);
            keyValues.Add("@VesselId", SO.Vessel.Index);

            keyValues.Add("@CustomerId", SO.Customer.Id);
            keyValues.Add("@TaxRateId", SO.AgreedTaxRate.Index);
            keyValues.Add("@TraderId", SO.Trader.Index);
            keyValues.Add("@Status", SO.Status);
            keyValues.Add("@SONumber", SO.SONumber);

            keyValues.Add("@OrderType", SO.OrderType);
            keyValues.Add("@SODate", SO.SODate);
            keyValues.Add("@SOExpiryDate", SO.SOExpiry);
            keyValues.Add("@PartyPONumber", SO.PartyPONumber);
            keyValues.Add("@PartyPODate", SO.PODate);
            keyValues.Add("@PartyPOExpiryDate", SO.POExpiry);
            keyValues.Add("@CreditPeriod", SO.CreditPeriod);
            keyValues.Add("@Quantity", SO.Quantity);
            keyValues.Add("@AgreedRate", SO.AgreedRate);
            keyValues.Add("@TraderCommision", SO.TraderCommission);
            if (SO.CompletedOn != null && SO.CompletedOn != DateTime.MinValue)
            {
                keyValues.Add("@CompletedOn", SO.CompletedOn);
            }
            else
            {
                keyValues.Add("@CompletedOn", DBNull.Value);
            }

            if (SO.ApprovedBy != null)
            {
                keyValues.Add("@ApprovedBy", SO.ApprovedBy.Id);
            }
            else
            {
                keyValues.Add("@ApprovedBy", DBNull.Value);
            }
            if (SO.ApprovedDate != null && SO.ApprovedDate != DateTime.MinValue)
            {
                keyValues.Add("@ApprovedDate", SO.ApprovedDate);
            }
            else
            {
                keyValues.Add("@ApprovedDate", DBNull.Value);
            }
            keyValues.Add("@CreatedOn", SO.CreatedOn);
            keyValues.Add("@CreatedBy", SO.CreatedBy.Id);
            keyValues.Add("@ModifiedOn", SO.ModifiedOn);
            keyValues.Add("@ModifiedBy", SO.ModifiedBy.Id);
            keyValues.Add("@POScannedImage", SO.PartyPOImage);
            keyValues.Add("@Remarks", SO.Remarks);
            return keyValues;
        }

        public static List<User> MapUserData(DataTable DTuser, DataTable DTrole, DataTable DTteam)
        {
            List<User> AllUsers = new List<User>();
            foreach (DataRow DRuser in DTuser.Rows)
            {
                User user = new User();

                user.Id = DRuser["Id"] != null ? new Guid(DRuser["Id"].ToString()) : Guid.Empty;
                user.Status = DRuser["Status"] != null ? MapUserStatus(DRuser["Status"].ToString()) : UserStatus.InActive;
                user.Name = Convert.ToString(DRuser["Name"]);
                user.Login = Convert.ToString(DRuser["LoginName"]);
                user.Designation = Convert.ToString(DRuser["Designation"]);
                user.Email = Convert.ToString(DRuser["Email"]);
                user.Mobile = Convert.ToString(DRuser["Mobile"]);
                user.Office = Convert.ToString(DRuser["Office"]);
                user.Home = Convert.ToString(DRuser["Home"]);
                user.Address = Convert.ToString(DRuser["Address"]);

                user.Roles = new List<Role>();
                foreach (DataRow DRrole in DTrole.Rows)
                {
                    Guid userid = DRrole["UserId"] != null ? new Guid(DRrole["UserId"].ToString()) : Guid.Empty;
                    if (userid != Guid.Empty && userid == user.Id)
                    {
                        Role userRole = new Role();
                        userRole.Id = DRrole["RoleId"] != null ? new Guid(DRrole["RoleId"].ToString()) : Guid.Empty;
                        userRole.Name = DRrole["Name"] != null ? DRrole["Name"].ToString() : "";
                        user.Roles.Add(userRole);
                    }
                }

                user.Teams = new List<Team>();
                foreach (DataRow DRteam in DTteam.Rows)
                {
                    Guid userid = DRteam["UserId"] != null ? new Guid(DRteam["UserId"].ToString()) : Guid.Empty;
                    if (userid != Guid.Empty && userid == user.Id)
                    {
                        Team userTeam = new Team();
                        userTeam.Id = DRteam["TeamId"] != null ? new Guid(DRteam["TeamId"].ToString()) : Guid.Empty;
                        userTeam.Name = DRteam["Name"] != null ? DRteam["Name"].ToString() : "";
                        user.Teams.Add(userTeam);
                    }
                }
                AllUsers.Add(user);
            }
            return AllUsers;
        }

        public static UserStatus MapUserStatus(string status)
        {
            switch (status)
            {
                case "1":
                    return UserStatus.Active;
                default:
                    return UserStatus.InActive;
            }
        }

        public static List<Customer> MapCustomerDataTable(DataTable dtCust, DataTable dtCustStock)
        {
            List<Customer> AllCustomers = new List<Customer>();
            foreach (DataRow drCust in dtCust.Rows)
            {
                Customer Cust = new Customer();

                Cust.Id = drCust["Id"] != null ? new Guid(drCust["Id"].ToString()) : Guid.Empty;
                Cust.Status = drCust["Status"] != null ? CustStatus.Active : CustStatus.InActive;
                Cust.CreatedOn = drCust["CreatedOn"] != DBNull.Value ? DateTime.Parse(drCust["CreatedOn"].ToString()) : DateTime.MinValue;
                Cust.CreatedBy = drCust["CreatedBy"] != null ? UserDataManager.GetUserRef(drCust["CreatedBy"].ToString()) : UserDataManager.GetDefaultRef();
                Cust.ModifiedOn = drCust["ModifiedOn"] != DBNull.Value ? DateTime.Parse(drCust["ModifiedOn"].ToString()) : DateTime.MinValue;
                Cust.ModifiedBy = drCust["ModifiedBy"] != null ? UserDataManager.GetUserRef(drCust["ModifiedBy"].ToString()) : UserDataManager.GetDefaultRef();
                Cust.Lead = drCust["Lead"] != null ? UserDataManager.GetUserRef(drCust["Lead"].ToString()) : UserDataManager.GetDefaultRef();
                Cust.Name = drCust["FullName"] != null ? drCust["FullName"].ToString() : "";
                Cust.ShortName = drCust["ShortName"] != null ? drCust["ShortName"].ToString() : "";
                Cust.NTN = drCust["NTN"] != null ? drCust["NTN"].ToString() : "";
                Cust.STRN = drCust["STRN"] != null ? drCust["STRN"].ToString() : "";
                Cust.Address = drCust["Address"] != null ? drCust["Address"].ToString() : "";
                Cust.InvoiceAddress = drCust["InvoiceAddress"] != null ? drCust["InvoiceAddress"].ToString() : "";
                Cust.Email = drCust["Email"] != null ? drCust["Email"].ToString() : "";
                Cust.Phone = drCust["Phone"] != null ? drCust["Phone"].ToString() : "";
                Cust.ContactPerson = drCust["ContactPerson"] != null ? drCust["ContactPerson"].ToString() : "";
                Cust.HeadOffice = drCust["HeadOffice"] != null ? drCust["HeadOffice"].ToString() : "";
                Cust.Remarks = drCust["Remarks"] != null ? drCust["Remarks"].ToString() : "";
                Cust.Stock = new List<CustomerStock>();
                foreach (DataRow drCustStock in dtCustStock.Rows)
                {
                    Guid custId = drCustStock["CustomerId"] != null ? new Guid(drCustStock["CustomerId"].ToString()) : Guid.Empty;
                    if (custId != Guid.Empty && custId == Cust.Id)
                    {
                        CustomerStock CustStock = new CustomerStock();
                        CustStock.Customer = new Reference() { Id = Cust.Id, Name = Cust.Name };
                        CustStock.Store = drCustStock["StoreId"] != null ? new Reference() { Id = new Guid(drCustStock["StoreId"].ToString()) } : StoreDataManager.GetDefaultRef();
                        CustStock.Vessel = drCustStock["Vessel"] != null ? CommonDataManager.GetVessel(drCustStock["Vessel"].ToString()) : CommonDataManager.GetDefaultRef();
                        CustStock.Origin = drCustStock["Origin"] != null ? CommonDataManager.GetOrigin(drCustStock["Origin"].ToString()) : CommonDataManager.GetDefaultRef();
                        CustStock.Size = drCustStock["Size"] != null ? CommonDataManager.GetSize(drCustStock["Size"].ToString()) : CommonDataManager.GetDefaultRef();
                        CustStock.Quantity = drCustStock["Quantity"] != null ? decimal.Parse(drCustStock["Quantity"].ToString()) : 0;
                        Cust.Stock.Add(CustStock);
                    }

                }
                AllCustomers.Add(Cust);
            }
            return AllCustomers;
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
