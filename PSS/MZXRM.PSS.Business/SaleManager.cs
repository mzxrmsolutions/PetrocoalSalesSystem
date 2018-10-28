using MZXRM.PSS.Business.DBMap;
using MZXRM.PSS.Common;
using MZXRM.PSS.Data;
using MZXRM.PSS.DataManager;
using System;
using System.Collections.Generic;
using System.Data;
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
                if (HttpContext.Current.Session[SessionManager.SOSession] != null)
                    _AllSOs = HttpContext.Current.Session[SessionManager.SOSession] as List<SaleOrder>;
                if (_AllSOs == null || _AllSOs.Count == 0)
                    _AllSOs = ReadAllSO();
                return _AllSOs;
            }
        }

        #region " ReadAllSO Function "
        public static List<SaleOrder> ReadAllSO()
        {
                DataTable DTso =SaleDataManager. GetAllSOs();
                DataTable DTdo = SaleDataManager.GetAllDOs();
                DataTable DTdc = SaleDataManager.GetAllDCs();

                List<SaleOrder> allSOs = SOMap.MapSOData(DTso, DTdo, DTdc);
            List<SaleOrder> calculatedSO = new List<SaleOrder>();
                foreach (SaleOrder SO in allSOs)
                {
                    SaleOrder so = SaleDataManager.CalculateSO(SO);
                    calculatedSO.Add(so);
                }
                HttpContext.Current.Session.Add(SessionManager.SOSession, calculatedSO);
            _AllSOs = calculatedSO;
                return _AllSOs;
        }

        public static void ResetCache()
        {
            _AllSOs = null;
            HttpContext.Current.Session[SessionManager.SOSession] = null;
        }
        #endregion
        public static DeliveryOrder GetDOByDONumber(string donumber)
        {
            List<SaleOrder> soList = ReadAllSO();
            try
            {
                foreach (SaleOrder SO in soList)
                {
                    foreach (DeliveryOrder DO in SO.DOList)
                    {
                        if (DO.DONumber == donumber)
                        {
                            return DO;
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                throw new Exception("Error! Get all SO from DataBase", ex);
            }
            return null;
        }
        public static DeliveryOrder GetDOById(int id)
        {
            List<SaleOrder> soList = ReadAllSO();
            try
            {
                foreach (SaleOrder SO in soList)
                {
                    foreach (DeliveryOrder DO in SO.DOList)
                    {
                        if (DO.Id == id)
                        {
                            return DO;
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                throw new Exception("Error! Get all SO from DataBase", ex);
            }
            return null;
        }
        
        public static SaleOrder GetSOByNumber(String SONumber)
        {
            foreach (SaleOrder so in ReadAllSO())
            {
                if (so.SONumber == SONumber)
                    return so;
            }
            return null;
        }

        public static DeliveryChalan GetDCById(int id)
        {
            List<SaleOrder> soList = ReadAllSO();
            try
            {
                foreach (SaleOrder SO in soList)
                {
                    foreach (DeliveryOrder DO in SO.DOList)
                    {
                        foreach (DeliveryChalan DC in DO.DCList)
                            if (DC.Id == id)
                            {
                                return DC;
                            }
                    }
                }
            }

            catch (Exception ex)
            {
                throw new Exception("Error! Get all SO from DataBase", ex);
            }
            return null;
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

                SaleOrder SO = GetSOById(Convert.ToInt32(values["SONumber"]));
                //DO.StoreId = StoreManager.GetStoreRef(values["StoreId"].ToString());
                DO.Location = CommonDataManager.GetSaleStation(values["Location"]);
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
                DO.DeliveryDestination = CustomerManager.GetCustomerDestination(SO.Customer.Id, values["DeliveryDestination"].ToString());
                //TODO: trader and transporter are different
                DO.Transportor = values["TransporterId"] != null ? Common.GetTransporter(values["TransporterId"].ToString()) : CommonDataManager.GetDefaultRef();
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
                SaleDataManager.SaveDO(SOMap.reMapDOData( DO));
                ResetCache();
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

                SaleOrder SO = GetSOById(Convert.ToInt32(values["SOID"]));
                DO.Id = Convert.ToInt32(values["DOID"]);

                //DO.StoreId = StoreManager.GetStoreRef(values["StoreId"].ToString());
                DO.Location = CommonDataManager.GetSaleStation(values["Location"]);
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
                DO.DeliveryDestination = CustomerManager.GetCustomerDestination(SO.Customer.Id, values["DeliveryDestination"].ToString());
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
                SaleDataManager.UpdateDO(SOMap.reMapDOData( DO));
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

               DeliveryOrder DO = SaleManager.GetDOById(Convert.ToInt32(values["DONumber"].ToString()));

                for (int i = 1; i <= 10; i++)
                {
                    if (values["TransporterId_" + i] == "0")
                        continue;

                    DC.DeliveryOrder = new Item { Index = DO.Id, Value = DO.DONumber };
                    DC.Lead = new Reference() { Id = DO.Lead.Id, Name = DO.Lead.Name };//UserManager.GetUserRef(values["Lead"].ToString());
                    //TODO: trader and transporter are different
                    DC.Transporter = values["TransporterId_" + i] != null ? CommonDataManager.GetTrader(values["TransporterId_" + i].ToString()) : CommonDataManager.GetDefaultRef();

                    
                    DC.Store = StoreManager.GetStoreRef(values["Store_"+i].ToString());

                    DC.Status = DCStatus.InTransit;
                    DC.DCDate = DateTime.Parse(values["DCDate_" + i].ToString());
                    DC.Quantity = decimal.Parse(values["Loaded_" + i].ToString());
                    DC.TruckNo = values["TruckNo_" + i].ToString();
                    DC.BiltyNo = values["BiltyNo_" + i].ToString();
                    DC.SlipNo = values["SlipNo_" + i].ToString();
                    DC.Weight = Decimal.Parse(values["Weight_" + i].ToString());
                    DC.NetWeight = Decimal.Parse(values["NetWeight_" + i].ToString());
                    DC.DriverName = values["DriverName_" + i].ToString();

                    DC.DriverPhone = values["DriverPhone_" + i].ToString();

                    DC.Remarks = values["Remarks_" + i] != null ? values["Remarks_" + i].ToString() : "";

                    Reference CurrentUser = new Reference() { Id = Common.CurrentUser.Id, Name = Common.CurrentUser.Name };
                    DC.CreatedBy = DC.ModifiedBy = CurrentUser;

                    DC.CreatedOn = DateTime.Now;
                    DC.ModifiedOn = DateTime.Now;
                    SaleDataManager.SaveDC(SOMap.reMapDCData(DC));

                }
                return DC;
            }
            catch (Exception ex)
            {
                ExceptionHandler.Error("Something went wrong. Details: " + ex.Message, ex);
                ExceptionLogManager.Log(ex, null, "Creating SO");
            }
            return null;
        }

        public static DeliveryChalan UpdateDC(Dictionary<string, string> values)
        {
            try
            {
                DeliveryChalan DC = GetDCByDCNumber(values["DCNumber"]);

                DeliveryOrder DO = SaleManager.GetDOById(Convert.ToInt32(values["DOId"].ToString()));

                DC.DeliveryOrder = new Item { Index = DO.Id, Value = DO.DONumber };
                DC.Lead = UserManager.GetUserRef(values["Lead"].ToString());
                //TODO: trader and transporter are different
                DC.Transporter = values["TransporterId"] != null ? CommonDataManager.GetTrader(values["TransporterId"].ToString()) : CommonDataManager.GetDefaultRef();
                DC.Store = StoreManager.GetStoreRef(values["Store"].ToString());
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
                DC.ModifiedBy = CurrentUser;

                DC.ModifiedOn = DateTime.Now;
                SaleDataManager.UpdateDC(SOMap.reMapDCData( DC));
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
                SO.BufferQuantityMin = decimal.Parse(values["BufferMin"].ToString());
                SO.BufferQuantityMax = decimal.Parse(values["BufferMax"].ToString());
                SO.AgreedTaxRate = values.ContainsKey("TaxRate") ? Common.GetTaxRate(values["TaxRate"].ToString()) : null;
                SO.Tax = !((SO.AgreedTaxRate == null) || (SO.AgreedTaxRate.Index == 0));
                //SO.AgreedRate = decimal.Parse(values["AgreedRate"].ToString());
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
                SO.CompletedOn = SO.ApprovedDate = DateTime.MinValue;
                SO.ApprovedBy = null;
                SaleDataManager.UpdateSO(SOMap.reMapSOData( SO));
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
                SO.BufferQuantityMin = decimal.Parse(values["BufferMin"].ToString());
                SO.BufferQuantityMax = decimal.Parse(values["BufferMax"].ToString());
                SO.LC = SO.OrderType == SOType.LC;
                if (values["ordertype"] == "1" || values["ordertype"] == "3")
                {
                    SO.AgreedTaxRate = Common.GetTaxRate(values["TaxRate"].ToString());
                    SO.Tax = !((SO.AgreedTaxRate == null) || (SO.AgreedTaxRate.Index == 0));
                    SO.AgreedRate = decimal.Parse(values["Rate"].ToString());
                    
                }
                    
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
                SO.PartyPOImage = values["POScanImage"].ToString();
                //todo: temp work
                //SO.PartyPOImage = String.Empty;
                SaleDataManager.SaveSO(SOMap.reMapSOData( SO));
                ResetCache();
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

        #region " Added by kashif abbas on 29th March 2018 "
        public static DeliveryChalan GetDCByDCNumber(String DCNumber)
        {
            foreach (SaleOrder so in AllSOs)
            {
                if (so.DOList != null)
                {
                    foreach (DeliveryOrder DO in so.DOList)
                    {
                        if (DO.DCList != null)
                        {
                            DeliveryChalan dc = DO.DCList.SingleOrDefault(x => x.DCNumber == DCNumber);
                            if (dc != null)
                                return dc;
                        }
                    }
                }
            }
            return null;
        } 
        #endregion

        public static void CompleteDO(DeliveryOrder DO)
        {
            DO.Status = DOStatus.Completed;
            Reference CurrentUser = new Reference() { Id = Common.CurrentUser.Id, Name = Common.CurrentUser.Name };
            DO.ModifiedBy = CurrentUser;
            DO.ModifiedOn =  DateTime.Now;
            DO.ApprovedBy = CurrentUser;
            SaleDataManager.UpdateDO(SOMap.reMapDOData( DO));
        }

        public static void ApproveDO(DeliveryOrder DO)
        {
            DO.Status = DOStatus.InProcess;
            Reference CurrentUser = new Reference() { Id = Common.CurrentUser.Id, Name = Common.CurrentUser.Name };
            DO.ModifiedBy = DO.ApprovedBy = CurrentUser;
            DO.ModifiedOn = DO.ApprovedDate = DateTime.Now;
           
            SaleDataManager.UpdateDO(SOMap.reMapDOData(DO));
            
        }

        public static void StopDOLoading(DeliveryOrder DO)
        {
            DO.Status = DOStatus.LoadingStop;
            Reference CurrentUser = new Reference() { Id = Common.CurrentUser.Id, Name = Common.CurrentUser.Name };
            DO.ModifiedBy =  CurrentUser;
            DO.ModifiedOn = DateTime.Now;

            SaleDataManager.UpdateDO(SOMap.reMapDOData(DO));
        }
        public static void StartDOLoading(DeliveryOrder DO)
        {
            DO.Status = DOStatus.InProcess;
            Reference CurrentUser = new Reference() { Id = Common.CurrentUser.Id, Name = Common.CurrentUser.Name };
            DO.ModifiedBy = CurrentUser;
            DO.ModifiedOn = DateTime.Now;

            SaleDataManager.UpdateDO(SOMap.reMapDOData(DO));
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
                string ErrorMessage = string.Empty;
                if(values["ordertype"] == "1" || values["ordertype"] == "3")
                {
                    //Rate is changed to AgreedRate
                    //if (string.IsNullOrEmpty(values["Rate"])) ErrorMessage += "Rate"; 
                    if (string.IsNullOrEmpty(values["AgreedRate"])) ErrorMessage += "AgreedRate";
                    if (string.IsNullOrEmpty(values["TaxRate"])) ErrorMessage += "TaxRate";
                    if (string.IsNullOrEmpty(values["Trader"])) ErrorMessage += "Trader";
                    if (string.IsNullOrEmpty(values["TraderCommission"])) ErrorMessage += "TraderCommission";
                }
                    
                if (string.IsNullOrEmpty(values["CreditPeriod"])) ErrorMessage += "CreditPeriod";
                if (string.IsNullOrEmpty(values["Remarks"])) ErrorMessage += "Remarks";
                if (string.IsNullOrEmpty(values["Quantity"])) ErrorMessage += "Quantity";
                //if (string.IsNullOrEmpty(values["POScanImage"])) ErrorMessage += "POScanImage";
                if (string.IsNullOrEmpty(values["BufferMin"])) ErrorMessage += "Buffer Min %, ";
                if (string.IsNullOrEmpty(values["BufferMax"])) ErrorMessage += "Buffer Max %, ";
                if (string.IsNullOrEmpty(values["POExpiry"])) ErrorMessage += "POExpiry";
                if (string.IsNullOrEmpty(values["PODate"])) ErrorMessage += "PODate";
                if (string.IsNullOrEmpty(values["PONumber"])) ErrorMessage += "PONumber";
                if (string.IsNullOrEmpty(values["SOExpiry"])) ErrorMessage += "SOExpiry";
                if (string.IsNullOrEmpty(values["SODate"])) ErrorMessage += "SODate";
                if (values["Customer"] == "0") ErrorMessage += "Customer, ";
                if (values["Lead"] == "0") ErrorMessage += "Lead, ";
                if (values["Origin"] == "0") ErrorMessage += "Origin, ";
                if (values["Size"] == "0") ErrorMessage += "Size, ";
                if (values["Vessel"] == "0") ErrorMessage += "Vessel, ";
               
                if (string.IsNullOrEmpty(ErrorMessage))
                {
                    SaleOrder SO = NewSO();
                    SO.Lead = UserManager.GetUserRef(values["Lead"]);
                    SO.PODate = values.ContainsKey("PODate") ? DateTime.Parse(values["PODate"]) : DateTime.Now;
                    SO.Origin = Common.GetOrigin(values["Origin"]);
                    SO.Size = Common.GetSize(values["Size"]);
                    SO.Vessel = Common.GetVessel(values["Vessel"]);
                    SO.Customer= CustomerManager.GetCustomerRef(values["Customer"]);

                    SO.BufferQuantityMin = values.ContainsKey("BufferMin") ? decimal.Parse(values["BufferMin"]) : 10;
                    SO.BufferQuantityMax = values.ContainsKey("BufferMax") ? decimal.Parse(values["BufferMax"]) : 10;


                    SO.Status = SOStatus.Created;
                    SO.PODate = values.ContainsKey("PODate") ? DateTime.Parse(values["PODate"]) : DateTime.MaxValue;
                    SO.SODate = values.ContainsKey("SODate") ? DateTime.Parse(values["SODate"]) : DateTime.MaxValue;
                    SO.SOExpiry = values.ContainsKey("SOExpiry") ? DateTime.Parse(values["SOExpiry"]) : DateTime.MaxValue;
                    SO.POExpiry = values.ContainsKey("POExpiry") ? DateTime.Parse(values["POExpiry"]) : DateTime.MaxValue;
                    SO.SONumber = GenerateNextSONumber();
                    if (values["ordertype"] == "1" || values["ordertype"] == "3")
                    {
                        SO.AgreedTaxRate = Common.GetTaxRate(values["TaxRate"]);
                        SO.Trader = Common.GetTrader(values["Trader"]);
                        SO.TraderCommission = values.ContainsKey("TraderCommission") ? decimal.Parse(values["TraderCommission"]) : 0;
                    }
                  
                    SO.CreditPeriod = values.ContainsKey("CreditPeriod") ? int.Parse(values["CreditPeriod"]) : 0;
                    SO.Remarks = values["Remarks"];
                    SO.Quantity = values.ContainsKey("Quantity") ? decimal.Parse(values["Quantity"]) : 0;


                    SO = SaleDataManager.CalculateSO(SO);
                    return "";
                }
                throw new Exception("Enter required fields: " + ErrorMessage);
            }
            catch (Exception ex)
            {
                ExceptionHandler.Error(ex.Message);
            }
            return null;
        }

        public static string ValidateCreateDCForm(Dictionary<string, string> values)
        {
            try
            {
                string ErrorMessage = string.Empty;
               
                if (values["DONumber"] == "0") ErrorMessage += "Origin, ";
                bool error = false;
                for (int i = 1; i <= 10; i++)
                {

                    if (values["TransporterId_" + i] == "0")
                        continue;
                  
                    if (values["Store_" + i] == "0") ErrorMessage += "Store, ";
                    if (values["UnloadedSize_" + i] == "0") ErrorMessage += "UnloadedSize_, ";
                    if (values["LoadedSize_" + i] == "0") ErrorMessage += "LoadedSize_, ";
                    if (string.IsNullOrEmpty(values["TruckNo_" + i])) ErrorMessage += "TruckNo_, ";
                    if (string.IsNullOrEmpty(values["BiltyNo_" + i])) ErrorMessage += "BiltyNo_, ";
                    if (string.IsNullOrEmpty(values["SlipNo_" + i])) ErrorMessage += "SlipNo_, ";
                    if (string.IsNullOrEmpty(values["Weight_" + i])) ErrorMessage += "Weight_, ";
                    if (string.IsNullOrEmpty(values["NetWeight_" + i])) ErrorMessage += "NetWeight_, ";
                    if (string.IsNullOrEmpty(values["NetWeight_" + i])) ErrorMessage += "NetWeight_, ";
                    if (string.IsNullOrEmpty(values["DriverPhone_" + i])) ErrorMessage += "DriverPhone_, ";
                    if (string.IsNullOrEmpty(values["Loaded_" + i])) ErrorMessage += "Loaded_, ";
                    if (string.IsNullOrEmpty(values["Unloaded_" + i])) ErrorMessage += "Unloaded_, ";
                    if (string.IsNullOrEmpty(values["Remarks_" + i])) ErrorMessage += "Remarks_, ";
                    DateTime tdate = new DateTime();
                    if (!DateTime.TryParse(values["DCDate_" + i], out tdate)) ErrorMessage += "DCDate_, ";
                    if (error)
                    {
                        ErrorMessage += "DC";
                        break;
                    }
                }

                if (string.IsNullOrEmpty(ErrorMessage))
                {
                    for (int i = 1; i <= 10; i++)
                    {
                        if (values["TransporterId_" + i] == "0")
                            continue;

                        if (values["Store_" + i] == "0")
                            continue;

                        DeliveryChalan dc = NewDeliveryChalan();
                        if (values.ContainsKey("Store"))
                            dc.Store = StoreManager.GetStoreRef(values["Store"].ToString());
                        dc.TruckNo = values["TruckNo_" + i.ToString()];
                        dc.DCDate = values.ContainsKey("DCDate_" + i.ToString()) ? DateTime.Parse(values["DCDate_" + i.ToString()]) : DateTime.MaxValue;
                        dc.Quantity = values.ContainsKey("Loaded_" + i.ToString()) ? decimal.Parse(values["Loaded_" + i.ToString()]) : 0;
                        dc.BiltyNo = values["BiltyNo_" + i.ToString()];
                        dc.SlipNo = values["SlipNo_" + i.ToString()];
                        dc.Weight = values.ContainsKey("Weight_" + i.ToString()) ? decimal.Parse(values["Weight_" + i.ToString()]) : 0;
                        dc.NetWeight = values.ContainsKey("NetWeight_" + i.ToString()) ? decimal.Parse(values["NetWeight_" + i.ToString()]) : 0;
                        dc.DriverName = values["DriverName_" + i.ToString()];
                        dc.DriverPhone = values["DriverPhone_" + i.ToString()];
                        dc.Remarks = values["Remarks_" + i.ToString()];
                    }
                    return "";
                }
                throw new Exception("Enter required fields: " + ErrorMessage);
            }
            catch (Exception ex)
            {
                ExceptionHandler.Error(ex.Message);
            }
            return null;
        }

        public static string ValidateUpdateDCForm(Dictionary<string, string> values)
        {
            try
            {
                string ErrorMessage = string.Empty;

                if (values["DONumber"] == "0") ErrorMessage += "Origin, ";
                bool error = false;


                if (values["TransporterId"] == "0") ErrorMessage += "TransporterId, ";

                //if (values["UnloadedSize"] == "0") ErrorMessage += "UnloadedSize, ";
                //if (values["LoadedSize"] == "0") ErrorMessage += "LoadedSize, ";
                if (string.IsNullOrEmpty(values["TruckNo"])) ErrorMessage += "TruckNo, ";
                    if (string.IsNullOrEmpty(values["BiltyNo"])) ErrorMessage += "BiltyNo, ";
                    if (string.IsNullOrEmpty(values["SlipNo"])) ErrorMessage += "SlipNo, ";
                    if (string.IsNullOrEmpty(values["Weight"])) ErrorMessage += "Weight, ";
                    if (string.IsNullOrEmpty(values["NetWeight"])) ErrorMessage += "NetWeight, ";
                if (string.IsNullOrEmpty(values["Quantity"])) ErrorMessage += "Quantity, ";
                // if (string.IsNullOrEmpty(values["NetWeight"])) ErrorMessage += "NetWeight, ";
                if (string.IsNullOrEmpty(values["DriverPhone"])) ErrorMessage += "DriverPhone, ";
                if (string.IsNullOrEmpty(values["DriverName"])) ErrorMessage += "DriverName, ";
                //if (string.IsNullOrEmpty(values["Loaded"])) ErrorMessage += "Loaded, ";
                //if (string.IsNullOrEmpty(values["Unloaded"])) ErrorMessage += "Unloaded, ";
                if (string.IsNullOrEmpty(values["Remarks"])) ErrorMessage += "Remarks, ";
                    DateTime tdate = new DateTime();
                    if (!DateTime.TryParse(values["DCDate"], out tdate)) ErrorMessage += "DCDate, ";
                    if (error)
                    {
                        ErrorMessage += "DC";
                    }


                if (string.IsNullOrEmpty(ErrorMessage))
                {

                    //if (values["TransporterId"] == "0")

                    DeliveryChalan dc = NewDeliveryChalan();
                    dc.TruckNo = values["TruckNo"];
                    dc.DCDate = values.ContainsKey("DCDate") ? DateTime.Parse(values["DCDate"]) : DateTime.MaxValue;
                   
                    dc.Quantity = values.ContainsKey("Quantity") ? decimal.Parse(values["Quantity"]) : 0;
                    dc.BiltyNo = values["BiltyNo"];
                    dc.SlipNo = values["SlipNo"];
                    dc.Weight = values.ContainsKey("Weight") ? decimal.Parse(values["Weight"]) : 0;
                    dc.NetWeight = values.ContainsKey("NetWeight") ? decimal.Parse(values["NetWeight"]) : 0;
                        dc.DriverName = values["DriverName"];
                        dc.DriverPhone = values["DriverPhone"];
                        dc.Remarks = values["Remarks"];
                    
                    return "";
                }
                throw new Exception("Enter required fields: " + ErrorMessage);
            }
            catch (Exception ex)
            {
                ExceptionHandler.Error(ex.Message);
            }
            return null;
        }

        public static string ValidateCreateDOForm(Dictionary<string, string> values)
        {
            try
            {
                string ErrorMessage = string.Empty;

                if (values["SONumber"] == "0") ErrorMessage += "SO, ";
                bool error = false;
               

                    if (values["TransporterId"] == "0") ErrorMessage += "TransporterId, ";
                    if (values["Lead"] == "0") ErrorMessage += "Lead, ";
                    if (values["Location"] == "0") ErrorMessage += "Location, ";
                    if (values["DeliveryDestination"] == "0") ErrorMessage += "DeliveryDestination, ";


                    if (string.IsNullOrEmpty(values["DODate"])) ErrorMessage += "DODate, ";
                    if (string.IsNullOrEmpty(values["Quantity"])) ErrorMessage  += "Quantity, ";
                    if (string.IsNullOrEmpty(values["LiftingStartDate"])) ErrorMessage += "LiftingStartDate, ";
                if (string.IsNullOrEmpty(values["LiftingEndDate"])) ErrorMessage += "LiftingEndDate, ";
                //if (string.IsNullOrEmpty(values["StoreId"])) ErrorMessage += "StoreId, ";
                if (string.IsNullOrEmpty(values["DumperRate"])) ErrorMessage += "DumperRate, ";
                if (string.IsNullOrEmpty(values["FreightPaymentTerms"])) ErrorMessage += "FreightPaymentTerms, ";
                if (string.IsNullOrEmpty(values["FreightPerTon"])) ErrorMessage += "FreightPerTon, ";
                if (string.IsNullOrEmpty(values["FreightTaxPerTon"])) ErrorMessage += "FreightTaxPerTon, ";
                if (string.IsNullOrEmpty(values["FreightComissionPSL"])) ErrorMessage += "FreightComissionPSL, ";
                if (string.IsNullOrEmpty(values["FreightComissionAgent"])) ErrorMessage += "FreightComissionAgent, ";
                if (string.IsNullOrEmpty(values["Remarks"])) ErrorMessage += "Remarks, ";



                if (string.IsNullOrEmpty(ErrorMessage))
                {
                       
                        string SONumber = values["SONumber"];
                    SaleOrder SO = NewSO();
                    try
                    {
                         SO = GetSOById(int.Parse(SONumber));
                    }
                    catch
                    {
                         SO = GetSOBySONumber(SONumber);
                    }
                    

                    DeliveryOrder dos = NewDO(SO);
                    dos.Transportor = values["TransporterId"] != null ? Common.GetTransporter(values["TransporterId"].ToString()) : CommonDataManager.GetDefaultRef();
                    dos.Lead = UserManager.GetUserRef(values["Lead"]);
                    dos.Location = CommonDataManager.GetSaleStation(values["Location"]);
                    dos.Quantity = values.ContainsKey("Quantity") ? decimal.Parse(values["Quantity"]) : 0;
                    dos.LiftingStartDate = values.ContainsKey("LiftingStartDate") ? DateTime.Parse(values["LiftingStartDate"]) : DateTime.MaxValue;
                    dos.LiftingEndDate = values.ContainsKey("LiftingEndDate") ? DateTime.Parse(values["LiftingEndDate"]) : DateTime.MaxValue;
                    //dos.StoreId = StoreManager.GetStoreRef(values["StoreId"]);
                    dos.DeliveryDestination = CustomerManager.GetCustomerDestination(SO.Customer.Id, values["DeliveryDestination"]); 
                    dos.DumperRate = values.ContainsKey("DumperRate") ? decimal.Parse(values["DumperRate"]) : 0;
                    dos.FreightPaymentTerms = values.ContainsKey("FreightPaymentTerms") ? decimal.Parse(values["FreightPaymentTerms"]) : 0;
                    dos.FreightPerTon = values.ContainsKey("FreightPerTon") ? decimal.Parse(values["FreightPerTon"]) : 0;
                    dos.FreightTaxPerTon= values.ContainsKey("FreightTaxPerTon") ? decimal.Parse(values["FreightTaxPerTon"]) : 0;
                    dos.FreightComissionPSL= values.ContainsKey("FreightComissionPSL") ? decimal.Parse(values["FreightComissionPSL"]) : 0;
                    dos.FreightComissionAgent= values.ContainsKey("FreightComissionAgent") ? decimal.Parse(values["FreightComissionAgent"]) : 0;
                    dos.Remarks= values["Remarks"];
                  
                    return "";
                }
                throw new Exception("Enter required fields: " + ErrorMessage);
            }
            catch (Exception ex)
            {
                ExceptionHandler.Error(ex.Message);
            }
            return null;
        }

        public static string ValidateCreateDOFormUpdate(Dictionary<string, string> values)
        {
            try
            {
                string ErrorMessage = string.Empty;

                //if (values["SONumber"] == "0") ErrorMessage += "SO, ";
                //bool error = false;


                if (values["TransporterId"] == "0") ErrorMessage += "TransporterId, ";
                if (values["Lead"] == "0") ErrorMessage += "Lead, ";
                if (values["Location"] == "0") ErrorMessage += "Location, ";
                if (values["DeliveryDestination"] == "0") ErrorMessage += "DeliveryDestination, ";


                if (string.IsNullOrEmpty(values["DODate"])) ErrorMessage += "DODate, ";
                if (string.IsNullOrEmpty(values["Quantity"])) ErrorMessage += "Quantity, ";
                if (string.IsNullOrEmpty(values["LiftingStartDate"])) ErrorMessage += "LiftingStartDate, ";
                if (string.IsNullOrEmpty(values["LiftingEndDate"])) ErrorMessage += "LiftingEndDate, ";
                //if (string.IsNullOrEmpty(values["StoreId"])) ErrorMessage += "StoreId, ";
                if (string.IsNullOrEmpty(values["DumperRate"])) ErrorMessage += "DumperRate, ";
                if (string.IsNullOrEmpty(values["FreightPaymentTerms"])) ErrorMessage += "FreightPaymentTerms, ";
                if (string.IsNullOrEmpty(values["FreightPerTon"])) ErrorMessage += "FreightPerTon, ";
                if (string.IsNullOrEmpty(values["FreightTaxPerTon"])) ErrorMessage += "FreightTaxPerTon, ";
                if (string.IsNullOrEmpty(values["FreightComissionPSL"])) ErrorMessage += "FreightComissionPSL, ";
                if (string.IsNullOrEmpty(values["FreightComissionAgent"])) ErrorMessage += "FreightComissionAgent, ";
                if (string.IsNullOrEmpty(values["Remarks"])) ErrorMessage += "Remarks, ";



                if (string.IsNullOrEmpty(ErrorMessage))
                {

                    string SONumber = values["SONumber"];
                    SaleOrder SO = GetSOById(int.Parse(SONumber));

                    DeliveryOrder dos = NewDO(SO);
                    dos.Transportor = values["TransporterId"] != null ? Common.GetTransporter(values["TransporterId"].ToString()) : CommonDataManager.GetDefaultRef();
                    dos.Lead = UserManager.GetUserRef(values["Lead"]);
                    dos.Location = CommonDataManager.GetSaleStation(values["Location"]);
                    dos.Quantity = values.ContainsKey("Quantity") ? decimal.Parse(values["Quantity"]) : 0;
                    dos.LiftingStartDate = values.ContainsKey("LiftingStartDate") ? DateTime.Parse(values["LiftingStartDate"]) : DateTime.MaxValue;
                    dos.LiftingEndDate = values.ContainsKey("LiftingEndDate") ? DateTime.Parse(values["LiftingEndDate"]) : DateTime.MaxValue;
                    //dos.StoreId = StoreManager.GetStoreRef(values["StoreId"]);
                    dos.DeliveryDestination = CustomerManager.GetCustomerDestination(SO.Customer.Id, values["DeliveryDestination"]);
                    dos.DumperRate = values.ContainsKey("DumperRate") ? decimal.Parse(values["DumperRate"]) : 0;
                    dos.FreightPaymentTerms = values.ContainsKey("FreightPaymentTerms") ? decimal.Parse(values["FreightPaymentTerms"]) : 0;
                    dos.FreightPerTon = values.ContainsKey("FreightPerTon") ? decimal.Parse(values["FreightPerTon"]) : 0;
                    dos.FreightTaxPerTon = values.ContainsKey("FreightTaxPerTon") ? decimal.Parse(values["FreightTaxPerTon"]) : 0;
                    dos.FreightComissionPSL = values.ContainsKey("FreightComissionPSL") ? decimal.Parse(values["FreightComissionPSL"]) : 0;
                    dos.FreightComissionAgent = values.ContainsKey("FreightComissionAgent") ? decimal.Parse(values["FreightComissionAgent"]) : 0;
                    dos.Remarks = values["Remarks"];

                    return "";
                }
                throw new Exception("Enter required fields: " + ErrorMessage);
            }
            catch (Exception ex)
            {
                ExceptionHandler.Error(ex.Message);
            }
            return null;
        }

        public static void ApprovedSO(SaleOrder SO)
        {
            SO.Status = SOStatus.InProcess;
            Reference CurrentUser = new Reference() { Id = Common.CurrentUser.Id, Name = Common.CurrentUser.Name };
            SO.ModifiedBy = SO.ApprovedBy = CurrentUser;
            SO.ModifiedOn = SO.ApprovedDate = DateTime.Now;

            SaleDataManager.UpdateSO(SOMap.reMapSOData( SO));
        }

        public static void CompleteSO(SaleOrder SO)
        {
            SO.Status = SOStatus.Completed;
            Reference CurrentUser = new Reference() { Id = Common.CurrentUser.Id, Name = Common.CurrentUser.Name };
            SO.ModifiedBy = CurrentUser;
            SO.CompletedOn = DateTime.Now;
            SO.ModifiedOn = DateTime.Now;

            SaleDataManager.UpdateSO(SOMap.reMapSOData(SO));
        }
        public static void CancelSO(SaleOrder SO)
        {
            SO.Status = SOStatus.Cancelled;
            Reference CurrentUser = new Reference() { Id = Common.CurrentUser.Id, Name = Common.CurrentUser.Name };
            SO.ModifiedBy = CurrentUser;
            SO.ModifiedOn = DateTime.Now;

            SaleDataManager.UpdateSO(SOMap.reMapSOData(SO));
        }


        private static DeliveryChalan NewDeliveryChalan()
        {
            DeliveryChalan dc = new DeliveryChalan();
            dc.Id = 0;
            dc.DeliveryOrder = new Item() { Index = 0, Value = "" };
            dc.Lead = new Reference() { Id = Guid.Empty, Name = "" };
            dc.Transporter = new Item() { Index = 0, Value = "" };
            dc.Status = 0;
            dc.DCNumber = "";
            dc.DCDate = DateTime.Now;
            dc.Quantity = 0;
            dc.TruckNo = "";
            dc.BiltyNo = "";
            dc.SlipNo = "";
            dc.Weight = 0;
            dc.NetWeight = 0;
            dc.DriverName = "";
            dc.DriverPhone = "";
            dc.CreatedOn = DateTime.Now;
            dc.CreatedBy = new Reference() { Id = Guid.Empty, Name = "" };
            dc.ModifiedOn = DateTime.Now;
            dc.ModifiedBy = new Reference() { Id = Guid.Empty, Name = "" };
            dc.Remarks = "";
            return dc;
        }

        private static SaleOrder NewSO()
        {
            SaleOrder SO = new SaleOrder();
            Reference currUser = new Reference() { Id = Common.CurrentUser.Id, Name = Common.CurrentUser.Name };
            SO.Id = 0;
            SO.Status = SOStatus.Created;
            SO.OrderType = SOType.Commercial;
            SO.CreatedOn = SO.ModifiedOn = SO.PODate = SO.SODate = SO.SOExpiry = SO.PODate = SO.POExpiry = DateTime.Now;
            SO.CreatedBy = SO.ModifiedBy = SO.Lead = SO.Customer = currUser;
            SO.CompletedOn = SO.ApprovedDate = DateTime.MinValue;
            SO.ApprovedBy = null;
            SO.SONumber = GenerateNextSONumber();
            SO.Origin = SO.Size = SO.Vessel = SO.AgreedTaxRate = SO.Trader = SO.SaleStation = new Item() { Index = 0, Value = "" };
            SO.CreditPeriod = 0;
            SO.PartyPONumber = SO.Remarks = SO.PartyPOImage = "";
            SO.Quantity = SO.TaxAmount = SO.RateExcTax = SO.RateIncTax = SO.FinalPrice = SO.TraderCommission = SO.DeliveredQuantity = SO.RemainingQuantity = 0;
            SO.LC = SO.Tax = true;
            SO.DOList = new List<DeliveryOrder>();
            return SO;
            // public decimal? AgreedRate;
        }

        private static DeliveryOrder NewDO(SaleOrder SO)
        {
            DeliveryOrder DO = new DeliveryOrder();
            Reference currUser = new Reference() { Id = Common.CurrentUser.Id, Name = Common.CurrentUser.Name };
            DO.SaleOrder = new Item() { Index = SO.Id, Value = SO.SONumber};
            DO.Id = 0;
            DO.Status = DOStatus.Created;
            DO.Location = DO.Lead = new Reference() { Id = Guid.Empty, Name = "" };
            DO.CreatedBy=DO.Lead  = DO.ApprovedBy = new Reference() { Id = Guid.Empty, Name = "" };
            DO.DONumber = DO.Remarks = "";
            DO.CreatedOn = DO.ModifiedOn = DO.CompletedOn = DO.ApprovedDate = DO.LiftingStartDate = DO.DODate = DO.LiftingEndDate = DateTime.Now;
            DO.CreatedOn  = DateTime.MinValue;
            DO.SaleOrder = DO.DeliveryDestination = DO.Transportor = new Item() { Index = 0, Value = "" };
            DO.Quantity= DO.DumperRate= DO.FreightPaymentTerms =DO.FreightPerTon= DO.FreightTaxPerTon= DO.FreightComissionPSL= DO.FreightComissionAgent= DO.DeliveredQuantity= DO.RemainingQuantity = 0;
            
            DO.DCList = new List<DeliveryChalan>();
            return DO;
    }
    }
}