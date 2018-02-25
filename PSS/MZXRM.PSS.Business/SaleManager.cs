using MZXRM.PSS.Common;
using MZXRM.PSS.Data;
using MZXRM.PSS.DataManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MZXRM.PSS.Business
{
    public class SaleManager
    {
        private static List<SaleOrder> _AllSOs;
        public static List<SaleOrder> AllSOs
        {
            get
            {
                _AllSOs = SaleDataManager.ReadAllSO();
                return _AllSOs;
            }
            set
            {
                _AllSOs = value;
            }
        }

        public static SaleOrder CreateSO(Dictionary<string, string> values)
        {
            try
            {
                SaleOrder SO = new SaleOrder();
                SO.Status = SOStatus.Created;
                SO.OrderType = (SOType) Enum.Parse(typeof(SOType), values["ordertype"].ToString());
                SO.CreatedOn = DateTime.Now;
                SO.CreatedBy = new Reference() { Id = Common.CurrentUser.Id, Name = Common.CurrentUser.Name };
                SO.ModifiedOn = DateTime.Now;
                SO.ModifiedBy = new Reference() { Id = Common.CurrentUser.Id, Name = Common.CurrentUser.Name };
                SO.Lead = UserManager.GetUserRef(values["Lead"].ToString());
                SO.SONumber = GenerateNextSONumber();
                SO.SODate = DateTime.Parse(values["SODate"].ToString());
                SO.SOExpiry = DateTime.Parse(values["SOExpiry"].ToString());
                SO.Customer = CustomerManager.GetCustomerRef(values["Customer"].ToString());
                SO.PartyPONumber = values["PONumber"].ToString();
                SO.PODate = DateTime.Parse(values["PODate"].ToString());
                SO.POExpiry = DateTime.Parse(values["POExpiry"].ToString());
                SO.CreditPeriod=int.Parse( values["CreditPeriod"].ToString());
                SO.Origin = Common.GetOrigin(values["Origin"].ToString());
                SO.Size = Common.GetSize(values["Size"].ToString());
                SO.Vessel = Common.GetVessel(values["Vessel"].ToString());
                SO.Quantity = decimal.Parse(values["Quantity"].ToString());
                SO.LC = SO.OrderType == SOType.LC;
                SO.AgreedTaxRate = Common.GetTaxRate( values["TaxRate"].ToString());
                SO.Tax = !((SO.AgreedTaxRate == null) || (SO.AgreedTaxRate.Index==0));
                SO.AgreedRate = decimal.Parse(values["Rate"].ToString());
                if (SO.Tax)
                {
                    //SO.TaxAmount = decimal.Parse(values[""].ToString());
                    //SO.RateIncTax = decimal.Parse(values[""].ToString());
                    //SO.RateExcTax = decimal.Parse(values[""].ToString());
                }
                if (!SO.LC)
                {
                    SO.Trader = Common.GetTrader(values["Trader"].ToString());
                    SO.TraderCommission = decimal.Parse(values["TraderCommission"].ToString());
                }
                SO.Remarks = values["Remarks"].ToString();
                //SO.PartyPOImage = values["POScanImage"].ToString();
                //todo: temp work
                SO.PartyPOImage = String.Empty;
                SaleDataManager.SaveSO(SO);
                return SO;
            }
            catch (Exception ex)
            {
                ExceptionHandler.Error("Something went wrong. Details: " + ex.Message, ex);
                ExceptionLogManager.Log(ex, null,"Creating SO");
            }
            return null;
        }

        private static string GenerateNextSONumber()
        {
            int lNumber = 1001;
            foreach (SaleOrder so in AllSOs)
            {
                string soNumber = so.SONumber.Replace("SO-", "");
                int soNo = int.Parse(soNumber);
                if (soNo > lNumber)
                    lNumber = soNo;
            }
            lNumber++;
            return "SO-" + lNumber.ToString();
        }

        public static string ValidateCreateSOForm(Dictionary<string, string> values)
        {
            try
            {
                foreach (KeyValuePair<string, string> keyValue in values)
                {
                    //string key = keyValue.Key;
                    //string value = keyValue.Value;
                    //if (key == "Origin" && value == "0") throw new Exception("Origin is required");
                    //if (key == "Size" && value == "0") throw new Exception("Size is required");
                    //if (key == "Vessel" && value == "0") throw new Exception("Vessel is required");
                    //if (key == "PODate" && string.IsNullOrEmpty(value)) throw new Exception("PODate is required");
                    //if (key == "TargetDays" && string.IsNullOrEmpty(value)) throw new Exception("TargetDays is required");
                    //if (key == "Supplier" && string.IsNullOrEmpty(value)) throw new Exception("Supplier is required");
                    //if (key == "Lead" && string.IsNullOrEmpty(value)) throw new Exception("Lead is required");
                    //// if (key == "PaymentTerms" && string.IsNullOrEmpty(value)) throw new Exception("PaymentTerms is required");
                    //if (key == "BufferMin" && string.IsNullOrEmpty(value)) throw new Exception("Buffer Min is required");
                    //if (key == "BufferMax" && string.IsNullOrEmpty(value)) throw new Exception("Buffer Max is required");
                }
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}