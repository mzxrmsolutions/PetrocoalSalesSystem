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
  public  class SOMap
    {
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
                SO.CreatedBy = dr["CreatedBy"] != null ? UserManager.GetUserRef(dr["CreatedBy"].ToString()) : UserManager.GetDefaultRef();
                SO.ModifiedOn = dr["ModifiedOn"] != DBNull.Value ? DateTime.Parse(dr["ModifiedOn"].ToString()) : DateTime.MinValue;
                SO.ModifiedBy = dr["ModifiedBy"] != null ? UserManager.GetUserRef(dr["ModifiedBy"].ToString()) : UserManager.GetDefaultRef();

                SO.CompletedOn = dr["CompletedOn"] != DBNull.Value ? DateTime.Parse(dr["CompletedOn"].ToString()) : DateTime.MinValue;
                SO.Lead = dr["LeadId"] != null ? UserManager.GetUserRef(dr["LeadId"].ToString()) : UserManager.GetDefaultRef();
                SO.ApprovedDate = dr["ApprovedDate"] != DBNull.Value ? DateTime.Parse(dr["ApprovedDate"].ToString()) : DateTime.MinValue;
                SO.ApprovedBy = dr["ApprovedBy"] != null ? UserManager.GetUserRef(dr["LeadId"].ToString()) : UserManager.GetDefaultRef();


                SO.SONumber = dr["SONumber"].ToString();
                SO.SODate = DateTime.Parse(dr["SODate"].ToString());
                SO.SOExpiry = DateTime.Parse(dr["SOExpiryDate"].ToString());
                SO.Customer = dr["CustomerId"] != null ? CustomerManager.GetCustomerRef(dr["CustomerId"].ToString()) : CustomerManager.GetDefaultRef();
                SO.PartyPONumber = dr["PartyPONumber"].ToString();
                SO.PODate = DateTime.Parse(dr["PartyPODate"].ToString());
                SO.POExpiry = DateTime.Parse(dr["PartyPOExpiryDate"].ToString());
                SO.CreditPeriod = int.Parse(dr["CreditPeriod"].ToString());
                SO.Origin = dr["OriginId"] != null ? CommonDataManager.GetOrigin(dr["OriginId"].ToString()) : CommonDataManager.GetDefaultRef();
                SO.Size = dr["SizeId"] != null ? CommonDataManager.GetSize(dr["SizeId"].ToString()) : CommonDataManager.GetDefaultRef();
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
                        DO.StoreId = StoreManager.GetStoreRef(drDo["StoreId"].ToString());    //.ReadAllStore().Where(x => x.StoreManager.Id == (Guid)drDo["StoreId"]).FirstOrDefault());
                        DO.Location = drDo["SaleStationId"] != null ? CommonDataManager.GetSaleStation(drDo["SaleStationId"].ToString()) : CommonDataManager.GetDefaultReference();
                        DO.Lead = drDo["LeadId"] != null ? UserManager.GetUserRef(drDo["LeadId"].ToString()) : UserManager.GetDefaultRef();
                        DO.Status = (DOStatus)Enum.Parse(typeof(DOStatus), drDo["status"].ToString());

                        DO.CompletedOn = drDo["CompletedOn"] != DBNull.Value ? DateTime.Parse(drDo["CompletedOn"].ToString()) : DateTime.MinValue;
                        DO.ApprovedDate = drDo["ApprovedDate"] != DBNull.Value ? DateTime.Parse(drDo["ApprovedDate"].ToString()) : DateTime.MinValue;
                        DO.ApprovedBy = drDo["ApprovedBy"] != null ? UserManager.GetUserRef(drDo["LeadId"].ToString()) : UserManager.GetDefaultRef();


                        //TODO: Get sale order reference here
                        DO.SaleOrder = new Item() { Index = SO.Id, Value = SO.SONumber };

                        DO.DONumber = drDo["DONumber"].ToString();
                        DO.DODate = DateTime.Parse(drDo["DODate"].ToString());
                        DO.Quantity = Decimal.Parse(drDo["Quantity"].ToString());
                        DO.LiftingStartDate = DateTime.Parse(drDo["LiftingStartDate"].ToString());
                        DO.LiftingEndDate = DateTime.Parse(drDo["LiftingEndDate"].ToString());
                        DO.DeliveryDestination = CustomerManager.GetCustomerDestination(SO.Customer.Id, drDo["DeliveryDestination"].ToString());
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
                        DO.CreatedBy = drDo["CreatedBy"] != null ? UserManager.GetUserRef(drDo["CreatedBy"].ToString()) : UserManager.GetDefaultRef();
                        DO.ModifiedOn = drDo["ModifiedOn"] != DBNull.Value ? DateTime.Parse(drDo["ModifiedOn"].ToString()) : DateTime.MinValue;

                        DO.ModifiedBy = drDo["ModifiedBy"] != null ? UserManager.GetUserRef(drDo["ModifiedBy"].ToString()) : UserManager.GetDefaultRef();
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
                                DC.Lead = drDC["LeadId"] != null ? UserManager.GetUserRef(drDC["LeadId"].ToString()) : UserManager.GetDefaultRef();
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
                                DC.CreatedBy = drDC["CreatedBy"] != null ? UserManager.GetUserRef(drDC["CreatedBy"].ToString()) : UserManager.GetDefaultRef();
                                DC.ModifiedOn = drDC["ModifiedOn"] != DBNull.Value ? DateTime.Parse(drDC["ModifiedOn"].ToString()) : DateTime.MinValue;
                                DC.ModifiedBy = drDC["ModifiedBy"] != null ? UserManager.GetUserRef(drDC["ModifiedBy"].ToString()) : UserManager.GetDefaultRef();

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
            if (SO.LC)
            {
                keyValues.Add("@TaxRateId", 0);
                keyValues.Add("@TraderId", 0);
                keyValues.Add("@AgreedRate", 0);
                keyValues.Add("@TraderCommision", 0);
            }
            if (!SO.LC)
            {
                keyValues.Add("@TaxRateId", SO.AgreedTaxRate.Index);
                keyValues.Add("@TraderId", SO.Trader.Index);
                keyValues.Add("@TraderCommision", SO.TraderCommission);
                keyValues.Add("@AgreedRate", SO.AgreedRate);
            }
            
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
            keyValues.Add("@StoreId", DO.StoreId.Id);

            keyValues.Add("@SaleStationId", DO.Location.Id);
            keyValues.Add("@LeadId", DO.Lead.Id);
            keyValues.Add("@SOId", DO.SaleOrder.Index);
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
            keyValues.Add("@DeliveryDestination", DO.DeliveryDestination.Index);
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
            keyValues.Add("@Quantity", DC.NetWeight);
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


    }
}
