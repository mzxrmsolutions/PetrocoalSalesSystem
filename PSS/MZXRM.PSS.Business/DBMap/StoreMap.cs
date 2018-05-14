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
  public  class StoreMap
    {
        public static List<StockMovement> MapStockMovementData(DataTable dt)
        {
            List<StockMovement> StMovements = new List<StockMovement>();
            return StMovements;
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
            if (ST.Id != 0)
                keyValues.Add("@id", ST.Id);


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
            keyValues.Add("@CustomerId", ST.Customer.Id);
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
                mapData.CreatedBy = dr["CreatedBy"] != null ? UserManager.GetUserRef(dr["CreatedBy"].ToString()) : UserManager.GetDefaultRef();
                mapData.ModifiedOn = dr["ModifiedOn"] != DBNull.Value ? DateTime.Parse(dr["ModifiedOn"].ToString()) : DateTime.MinValue;
                mapData.ModifiedBy = dr["ModifiedBy"] != null ? UserManager.GetUserRef(dr["ModifiedBy"].ToString()) : UserManager.GetDefaultRef();

                mapData.CompletedOn = dr["CompletedOn"] != DBNull.Value ? DateTime.Parse(dr["CompletedOn"].ToString()) : DateTime.MinValue;
                mapData.LeadId = dr["LeadId"] != null ? UserManager.GetUserRef(dr["LeadId"].ToString()) : UserManager.GetDefaultRef();
                mapData.STNumber = dr["SMNumber"] != null ? dr["SMNumber"].ToString() : "";
                mapData.STDate = dr["SMDate"] != DBNull.Value ? DateTime.Parse(dr["SMDate"].ToString()) : DateTime.MinValue;

                mapData.Origin = dr["Origin"] != null ? CommonDataManager.GetOrigin(dr["Origin"].ToString()) : CommonDataManager.GetDefaultRef();
                mapData.Size = dr["Size"] != null ? CommonDataManager.GetSize(dr["Size"].ToString()) : CommonDataManager.GetDefaultRef();
                mapData.Vessel = dr["Vessel"] != null ? CommonDataManager.GetVessel(dr["Vessel"].ToString()) : CommonDataManager.GetDefaultRef();

                mapData.Quantity = dr["Quantity"] != null ? decimal.Parse(dr["Quantity"].ToString()) : 0;
                mapData.FromStoreId = dr["FromStoreId"] != null ? StoreManager.GetStoreRef(dr["FromStoreId"].ToString()) : StoreManager.GetDefaultRef();
                mapData.ToStoreId = dr["ToStoreId"] != null ? StoreManager.GetStoreRef(dr["ToStoreId"].ToString()) : StoreManager.GetDefaultRef();

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

            //TODO:
            // keyValues.Add("@Store", DO.Store.Id);
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
                mapData.CreatedBy = dr["CreatedBy"] != null ? UserManager.GetUserRef(dr["CreatedBy"].ToString()) : UserManager.GetDefaultRef();
                mapData.ModifiedOn = dr["ModifiedOn"] != DBNull.Value ? DateTime.Parse(dr["ModifiedOn"].ToString()) : DateTime.MinValue;
                mapData.ModifiedBy = dr["ModifiedBy"] != null ? UserManager.GetUserRef(dr["ModifiedBy"].ToString()) : UserManager.GetDefaultRef();

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
                        CustStock.Customer = drCustStock["CustomerId"] != null ? CustomerManager.GetCustomerRef(drCustStock["CustomerId"].ToString()) : CustomerManager.GetDefaultRef();
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
                        StMovement.Customer = drStMovement["CustomerId"] != null ? CustomerManager.GetCustomerRef(drStMovement["CustomerId"].ToString()) : CustomerManager.GetDefaultRef();
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

    }
}
