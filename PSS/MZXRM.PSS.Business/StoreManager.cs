using MZXRM.PSS.Data;
using MZXRM.PSS.DataManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MZXRM.PSS.Business
{
    public class StoreManager
    {
        private static List<Store> _allStores;
        public static List<Store> AllStore
        {
            get
            {
                _allStores = StoreDataManager.ReadAllStore();
                return _allStores;
            }
            set
            {
                _allStores = value;
            }
        }
        private static List<StoreTransfer> _allStoreInOuts;
        public static List<StoreTransfer> AllStoreInOut
        {
            get
            {
                _allStoreInOuts = StoreDataManager.ReadAllStoreIO();
                return _allStoreInOuts;
            }
            set
            {
                _allStoreInOuts = value;
            }
        }

        public static Store GetStore(Guid storeId)
        {
            foreach (Store store in AllStore)
            {
                if (store.Id == storeId)
                    return store;
            }
            return null;
        }

        public static string ValidateCreateStoreTransferForm(Dictionary<string, string> values)
        {
            try
            {
                foreach (KeyValuePair<string, string> keyValue in values)
                {
                    string key = keyValue.Key;
                    string value = keyValue.Value;
                    //if (key == "PO" && value == "0") throw new Exception("PO is required");
                    //if (key == "Customer" && value == "0") throw new Exception("Customer is required");
                    //if (key == "GRNDate" && string.IsNullOrEmpty(value)) throw new Exception("GRN Date is required");
                    //if (key == "Store" && value == "0") throw new Exception("Store is required");
                    //if (key == "Quantity" && value == "0") throw new Exception("Quantity is required");
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
            ST.STDate= values.ContainsKey("Date") ? DateTime.Parse(values["Date"]) : DateTime.Now;
            ST.Customer = values.ContainsKey("Customer") ? CustomerDataManager.GetCustRef(values["Customer"]) : CustomerDataManager.GetDefaultRef();
            ST.Origin = values.ContainsKey("Origin") ? CommonDataManager.GetOrigin(values["Origin"]) : CommonDataManager.GetDefaultRef();
            ST.Size = values.ContainsKey("Size") ? CommonDataManager.GetOrigin(values["Size"]) : CommonDataManager.GetDefaultRef();
            ST.Vessel = values.ContainsKey("Vessel") ? CommonDataManager.GetOrigin(values["Vessel"]) : CommonDataManager.GetDefaultRef();
            ST.Quantity = values.ContainsKey("Quantity") ? decimal.Parse(values["Quantity"]) : 0;
            ST.FromStoreId = values.ContainsKey("FromStore") ? StoreDataManager.GetStoreRef(values["FromStore"]) : StoreDataManager.GetDefaultRef();
            ST.ToStoreId = values.ContainsKey("ToStore") ? StoreDataManager.GetStoreRef(values["ToStore"]) : StoreDataManager.GetDefaultRef();
            ST.VehicleNo = values.ContainsKey("VehicleNo") ? values["VehicleNo"] : "";
            ST.BiltyNo = values.ContainsKey("BiltyNo") ? values["BiltyNo"] : "";
            ST.BiltyDate = values.ContainsKey("BiltyDate") ? DateTime.Parse(values["BiltyDate"]) : DateTime.MinValue;
            ST.RRInvoice = values.ContainsKey("Invoice") ? values["Invoice"] : "";
            ST.CCMNumber = values.ContainsKey("CCMNo") ? values["CCMNo"] : "";
            ST.Transporter = values.ContainsKey("Transporter") ? CommonDataManager.GetTrader( values["Transporter"]) : CommonDataManager.GetDefaultRef();
            ST.StoreInDate = DateTime.MinValue;
            ST.StoreInQuantity= 0;
            ST.Remarks = values.ContainsKey("Remarks") ? values["Remarks"] : "";

            StoreDataManager.CreateStoreTransfer(ST);
            StoreDataManager.ResetCache();
            return ST;
        }
    }
}
