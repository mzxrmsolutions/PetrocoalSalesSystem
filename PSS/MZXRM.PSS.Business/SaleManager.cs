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

        public static List<SaleOrder> ApprovedSOs
        {
            get
            {
                return AllSOs.Where(x => x.Status == SOStatus.InProcess).ToList();
            }
        }
        public static List<DeliveryOrder> ApprovedDOs
        {
            get
            {
                List<DeliveryOrder> dos = new List<DeliveryOrder>();
                foreach (SaleOrder so in ApprovedSOs)
                {
                    foreach (DeliveryOrder DO in so.DOList)
                    {
                        if (DO.Status == DOStatus.InProcess)
                            dos.Add(DO);
                    }
                }
                return dos;
            }
        }
        //UDPATED BY KASHIF ABBAS ON 13TH MARCH TO ADD DO
        public static DeliveryOrder CreateDO(Dictionary<string, string> values)
        {
            try
            {
                DeliveryOrder DO = new DeliveryOrder();

                SaleOrder SO = SaleDataManager.GetSOById(Convert.ToInt32(values["SONumber"]));

                DO.Store = StoreDataManager.GetStoreRef(values["Store"]);
                DO.SaleOrder = new Item { Index = SO.Id, Value = SO.SONumber };
                DO.Lead = UserManager.GetUserRef(values["Lead"].ToString());
                DO.Status = DOStatus.Created;
                DO.CompletedOn = DO.ApprovedDate = DateTime.MinValue;
                DO.ApprovedBy = null;
                //todo: autogenerate do number
                DO.DODate = DateTime.Parse(values["DODate"].ToString());
                DO.Quantity = decimal.Parse(values["Quantity"].ToString());

                DO.LiftingStartDate = DateTime.Parse(values["LiftingStartDate"].ToString());
                DO.LiftingEndDate = DateTime.Parse(values["LiftingEndDate"].ToString());
                DO.DeliveryDestination = values["DeliveryDestination"].ToString();
                //TODO: trader and transporter are different
                DO.Transportor = values["TransporterId"] != null ? CommonDataManager.GetTrader(values["TransporterId"].ToString()) : CommonDataManager.GetDefaultRef();
                DO.DumperRate = Decimal.Parse(values["DumperRate"].ToString());
                DO.FreightPaymentTerms = Decimal.Parse(values["FreightPaymentTerms"].ToString());
                DO.FreightPerTon = Decimal.Parse(values["FreightPerTon"].ToString());
                DO.FreightTaxPerTon = Decimal.Parse(values["FreightTaxPerTon"].ToString());
                DO.FreightComissionPSL = Decimal.Parse(values["FreightComissionPSL"].ToString());
                DO.FreightComissionAgent = Decimal.Parse(values["FreightComissionAgent"].ToString());
                DO.Remarks = values["Remarks"] != null ? values["Remarks"].ToString() : "";

                Reference CurrentUser = new Reference() { Id = Common.CurrentUser.Id, Name = Common.CurrentUser.Name };
                DO.CreatedBy = DO.ModifiedBy = CurrentUser;

                DO.CreatedOn = DateTime.Now;
                DO.ModifiedOn = DateTime.Now;
                //todo: temp work
                SaleDataManager.SaveDO(DO);
                return DO;
            }
            catch (Exception ex)
            {
                ExceptionHandler.Error("Something went wrong. Details: " + ex.Message, ex);
                ExceptionLogManager.Log(ex, null, "Creating SO");
            }
            return null;
        }
        public static DeliveryOrder UpdateDO(Dictionary<string, string> values)
        {
            try
            {
                DeliveryOrder DO = GetDOByDONumber(values["DONumber"]);

                SaleOrder SO = SaleDataManager.GetSOById(Convert.ToInt32(values["SOID"]));
                DO.Id = Convert.ToInt32(values["DOID"]);
                DO.Store = StoreDataManager.GetStoreRef(values["Store"]);
                DO.SaleOrder = new Item { Index = SO.Id, Value = SO.SONumber };
                DO.Lead = UserManager.GetUserRef(values["Lead"].ToString());
                DO.Status = DOStatus.Created;
                DO.CompletedOn = DO.ApprovedDate = DateTime.MinValue;
                DO.ApprovedBy = null;
                //todo: autogenerate do number
                DO.DODate = DateTime.Parse(values["DODate"].ToString());
                DO.Quantity = decimal.Parse(values["Quantity"].ToString());
                DO.DONumber = values["DONumber"];

                DO.LiftingStartDate = DateTime.Parse(values["LiftingStartDate"].ToString());
                DO.LiftingEndDate = DateTime.Parse(values["LiftingEndDate"].ToString());
                DO.DeliveryDestination = values["DeliveryDestination"].ToString();
                //TODO: trader and transporter are different
                DO.Transportor = values["TransporterId"] != null ? CommonDataManager.GetTrader(values["TransporterId"].ToString()) : CommonDataManager.GetDefaultRef();
                DO.DumperRate = Decimal.Parse(values["DumperRate"].ToString());
                DO.FreightPaymentTerms = Decimal.Parse(values["FreightPaymentTerms"].ToString());
                DO.FreightPerTon = Decimal.Parse(values["FreightPerTon"].ToString());
                DO.FreightTaxPerTon = Decimal.Parse(values["FreightTaxPerTon"].ToString());
                DO.FreightComissionPSL = Decimal.Parse(values["FreightComissionPSL"].ToString());
                DO.FreightComissionAgent = Decimal.Parse(values["FreightComissionAgent"].ToString());
                DO.Remarks = values["Remarks"] != null ? values["Remarks"].ToString() : "";
                DO.CreatedOn = Convert.ToDateTime(values["CreatedOn"]);
                
                Reference CurrentUser = new Reference() { Id = Common.CurrentUser.Id, Name = Common.CurrentUser.Name };
                DO.ModifiedBy = CurrentUser;

                DO.ModifiedOn = DateTime.Now;
                //todo: temp work
                SaleDataManager.UpdateDO(DO);
                return DO;
            }
            catch (Exception ex)
            {
                ExceptionHandler.Error("Something went wrong. Details: " + ex.Message, ex);
                ExceptionLogManager.Log(ex, null, "Creating SO");
            }
            return null;
        }

        public static DeliveryChalan CreateDC(Dictionary<string, string> values)
        {
            try
            {
                DeliveryChalan DC = new DeliveryChalan();

               DeliveryOrder DO = SaleDataManager.GetDOById(Convert.ToInt32(values["DONumber"].ToString()));

                DC.DeliveryOrder = new Item { Index = DO.Id, Value = DO.DONumber };
                DC.Lead = UserManager.GetUserRef(values["Lead"].ToString());
                //TODO: trader and transporter are different
                DC.Transporter = values["TransporterId"] != null ? CommonDataManager.GetTrader(values["TransporterId"].ToString()) : CommonDataManager.GetDefaultRef();
                DC.Status = DCStatus.InTransit;
                DC.DCDate = DateTime.Parse(values["DCDate"].ToString());
                DC.Quantity = decimal.Parse(values["Quantity"].ToString());
                DC.TruckNo = values["TruckNo"].ToString();
                DC.BiltyNo = values["BiltyNo"].ToString();
                DC.SlipNo = values["SlipNo"].ToString();
                DC.Weight = Decimal.Parse(values["Weight"].ToString());
                DC.NetWeight = Decimal.Parse(values["NetWeight"].ToString());
                DC.DriverName = values["DriverName"].ToString(); 

                DC.DriverPhone = values["DriverPhone"].ToString(); 

                DC.Remarks = values["Remarks"] != null ? values["Remarks"].ToString() : "";

                Reference CurrentUser = new Reference() { Id = Common.CurrentUser.Id, Name = Common.CurrentUser.Name };
                DC.CreatedBy = DC.ModifiedBy = CurrentUser;

                DC.CreatedOn = DateTime.Now;
                DC.ModifiedOn = DateTime.Now;
                SaleDataManager.SaveDC(DC);
                return DC;
            }
            catch (Exception ex)
            {
                ExceptionHandler.Error("Something went wrong. Details: " + ex.Message, ex);
                ExceptionLogManager.Log(ex, null, "Creating SO");
            }
            return null;
        }
        //ADDED BY KASHIF ABBAS TO UDPATE SO: 
        public static SaleOrder UpdateSO(Dictionary<string, string> values)
        {
            try
            {
                string soNumber = values["SONumber"];

                SaleOrder SO = SaleManager.GetSOBySONumber(soNumber);
                SO.ModifiedOn = DateTime.Now;
                SO.ModifiedBy = new Reference() { Id = Common.CurrentUser.Id, Name = Common.CurrentUser.Name };
                SO.Lead = UserManager.GetUserRef(values["Lead"].ToString());
                SO.SODate = DateTime.Parse(values["SODate"].ToString());
                SO.SOExpiry = DateTime.Parse(values["SOExpiry"].ToString());
                SO.PartyPONumber = values["PONumber"].ToString();
                SO.PODate = DateTime.Parse(values["PODate"].ToString());
                SO.POExpiry = DateTime.Parse(values["POExpiry"].ToString());
                SO.CreditPeriod = int.Parse(values["CreditPeriod"].ToString());
                SO.Origin = Common.GetOrigin(values["Origin"].ToString());
                SO.Size = Common.GetSize(values["Size"].ToString());
                SO.Vessel = Common.GetVessel(values["Vessel"].ToString());
                SO.Quantity = decimal.Parse(values["Quantity"].ToString());
                SO.AgreedTaxRate = values.ContainsKey("TaxRate")?Common.GetTaxRate(values["TaxRate"].ToString()): null;
                SO.Tax = !((SO.AgreedTaxRate == null) || (SO.AgreedTaxRate.Index == 0));
                SO.AgreedRate = decimal.Parse(values["Rate"].ToString())  ;
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
                SaleDataManager.UpdateSO(SO);
                return SO;
            }
            catch (Exception ex)
            {
                ExceptionHandler.Error("Something went wrong. Details: " + ex.Message, ex);
                ExceptionLogManager.Log(ex, null, "Creating SO");
            }
            return null;
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
                else
                {
                    SO.AgreedRate = null;
                    SO.AgreedTaxRate = null;
                    SO.Trader = null;
                    SO.TraderCommission = 0;

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

        //ADDED BY KASHIF ABBAS FOR UPDATING SO
        public static SaleOrder GetSOBySONumber(String SONumber)
        {
            var SO = AllSOs.SingleOrDefault(x => x.SONumber == SONumber);
            return SO;
           //return AllSOs.Where(x => x.SONumber == SONumber) as SaleOrder;
        }

        public static DeliveryOrder GetDOByDONumber(string doNubmer)
        {
            foreach (SaleOrder so in AllSOs)
            {
                var DO = so.DOList.SingleOrDefault(x => x.DONumber == doNubmer);
                return DO;
            }
            return null;
        }

        //ADDED BY KASHIF ABBAS FOR UPDATING SO
        public static SaleOrder GetSOById(Int32 SOId)
        {
            var SO = AllSOs.SingleOrDefault(x => x.Id == SOId);
            return SO;
            //return AllSOs.Where(x => x.SONumber == SONumber) as SaleOrder;
        }

        public static List<DeliveryChalan> GetDCByDOID(int id)
        {
            foreach (SaleOrder so in AllSOs)
            {
                if (so.DOList != null)
                {
                    foreach (DeliveryOrder DO in so.DOList)
                    {
                        if(DO.DCList != null)
                        return DO.DCList.Where(x => x.DeliveryOrder.Index == id).ToList();
                    }
                }
            }
            return null;
        }

        public static void CompleteDO(DeliveryOrder DO)
        {
            DO.Status = DOStatus.Completed;
            Reference CurrentUser = new Reference() { Id = Common.CurrentUser.Id, Name = Common.CurrentUser.Name };
            DO.ModifiedBy = CurrentUser;
            DO.ModifiedOn =  DateTime.Now;
            DO.ApprovedBy = CurrentUser;
            SaleDataManager.UpdateDO(DO);
        }

        public static void ApproveDO(DeliveryOrder DO)
        {
            DO.Status = DOStatus.InProcess;
            Reference CurrentUser = new Reference() { Id = Common.CurrentUser.Id, Name = Common.CurrentUser.Name };
            DO.ModifiedBy = DO.ApprovedBy = CurrentUser;
            DO.ModifiedOn = DO.ApprovedDate = DateTime.Now;
           
            SaleDataManager.UpdateDO(DO);
            
        }

        public static void StopDOLoading(DeliveryOrder DO)
        {
            DO.Status = DOStatus.LoadingStop;
            Reference CurrentUser = new Reference() { Id = Common.CurrentUser.Id, Name = Common.CurrentUser.Name };
            DO.ModifiedBy =  CurrentUser;
            DO.ModifiedOn = DateTime.Now;

            SaleDataManager.UpdateDO(DO);
        }
        public static void StartDOLoading(DeliveryOrder DO)
        {
            DO.Status = DOStatus.InProcess;
            Reference CurrentUser = new Reference() { Id = Common.CurrentUser.Id, Name = Common.CurrentUser.Name };
            DO.ModifiedBy = CurrentUser;
            DO.ModifiedOn = DateTime.Now;

            SaleDataManager.UpdateDO(DO);
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