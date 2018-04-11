using MZXRM.PSS.Common;
using MZXRM.PSS.Connector.Database;
using MZXRM.PSS.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MZXRM.PSS.DataManager
{

    public class SaleDataManager
    {
        static string _dataPath = ConfigurationManager.AppSettings["DataPath"];
        static bool readFromDB = true;

        #region " ReadAllSO Function "
        public static List<SaleOrder> ReadAllSO()
        {
            List<SaleOrder> AllSOs = new List<SaleOrder>();
            if (HttpContext.Current.Session[SessionManager.SOSession] == null)
                readFromDB = true;
            if (readFromDB)
            {
                DataTable DTso = GetAllSOs();
                DataTable DTdo = GetAllDOs();
                DataTable DTdc = GetAllDCs();

                List<SaleOrder> allSOs = DataMap.MapSOData(DTso, DTdo, DTdc);
                foreach (SaleOrder SO in allSOs)
                {
                    SaleOrder so = CalculateSO(SO);
                    AllSOs.Add(so);
                }
                HttpContext.Current.Session.Add(SessionManager.SOSession, AllSOs);
                readFromDB = false;
                return AllSOs;
            }
            AllSOs = HttpContext.Current.Session[SessionManager.SOSession] as List<SaleOrder>;
            return AllSOs;
        }
        #endregion

        #region " CalculateSO Function "
        private static SaleOrder CalculateSO(SaleOrder SO)
        {
            if (SO == null)
                return null;
            //TODO: Kashif Abbas. Need to populate the delivered and remaining quantity for Sale Order
            SO.DeliveredQuantity = SO.DOList.Sum(x => x.Quantity);
            SO.RemainingQuantity = SO.Quantity - SO.DeliveredQuantity;
            foreach(DeliveryOrder DO in SO.DOList)
            {
                DO.DeliveredQuantity = DO.DCList.Sum(y => y.Quantity);
                DO.RemainingQuantity = DO.Quantity - DO.DeliveredQuantity;
            }
            return SO;
        }
        #endregion

        #region " GetAllSOs Function "
        private static DataTable GetAllSOs()
        {
            try
            {
                using (var dbc = DataFactory.GetConnection())
                {

                    IDbCommand command = CommandBuilder.CommandGetAll(dbc, "sp_GetAllSO");

                    if (command.Connection.State != ConnectionState.Open)
                    {
                        command.Connection.Open();
                    }

                    IDataReader datareader = command.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(datareader);
                    return dt;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error! Get all SO from DataBase", ex);
            }
        }
        #endregion

        #region " GetAllDOs Function "
        private static DataTable GetAllDOs()
        {
            try
            {
                using (var dbc = DataFactory.GetConnection())
                {

                    IDbCommand command = CommandBuilder.CommandGetAll(dbc, "sp_GetAllDO");

                    if (command.Connection.State != ConnectionState.Open)
                    {
                        command.Connection.Open();
                    }

                    IDataReader datareader = command.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(datareader);
                    return dt;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error! Get all SO from DataBase", ex);
            }
        }
        #endregion

        public static DeliveryOrder GetDOById(int id)
        {
            List<SaleOrder> soList = ReadAllSO();
            try
            {
                foreach(SaleOrder SO in soList)
                {
                    foreach(DeliveryOrder DO in SO.DOList)
                    {
                        if(DO.Id == id)
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


        #region " GetAllDCs Function "
        private static DataTable GetAllDCs()
        {
            try
            {
                using (var dbc = DataFactory.GetConnection())
                {

                    IDbCommand command = CommandBuilder.CommandGetAll(dbc, "sp_GetAllDC");

                    if (command.Connection.State != ConnectionState.Open)
                    {
                        command.Connection.Open();
                    }

                    IDataReader datareader = command.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(datareader);
                    return dt;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error! Get all SO from DataBase", ex);
            }
        }
        #endregion

        #region " SaveSO Function "
        public static int SaveSO(SaleOrder SO)
        {
            using (var dbc = DataFactory.GetConnection())
            {
                Dictionary<string, object> keyValues = DataMap.reMapSOData(SO); //map podetail to db columns
                IDbCommand command = CommandBuilder.CommandInsert(dbc, "sp_InsertSO", keyValues);

                if (command.Connection.State != ConnectionState.Open)
                {
                    command.Connection.Open();
                }

                object obj = command.ExecuteScalar(); //execute query
                ResetCache();
                int retId = int.Parse(obj.ToString());
                return retId;
            }
        }
        #endregion

        //ADDED BY KASHIF ABBAS TO UDPATE SO
        #region " UpdateSO Function "
        public static void UpdateSO(SaleOrder SO)
        {
            using (var dbc = DataFactory.GetConnection())
            {
                Dictionary<string, object> keyValues = DataMap.reMapSOData(SO); //map podetail to db columns
                IDbCommand command = CommandBuilder.CommandInsert(dbc, "sp_UpdateSO", keyValues);

                if (command.Connection.State != ConnectionState.Open)
                {
                    command.Connection.Open();
                }
                command.ExecuteNonQuery(); //execute query
                ResetCache();

            }
        }
        #endregion
        public static void ResetCache()
        {
            readFromDB = true;
        }
        public static SaleOrder GetSOById(Int32 SOId)
        {
            foreach (SaleOrder so in ReadAllSO())
            {
                if (so.Id == SOId)
                    return so;
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
                        foreach(DeliveryChalan DC in DO.DCList)
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

        #region " SaveDO Function "
        public static int SaveDO(DeliveryOrder DO)
        {
            using (var dbc = DataFactory.GetConnection())
            {
                Dictionary<string, object> keyValues = DataMap.reMapDOData(DO); //map podetail to db columns
                IDbCommand command = CommandBuilder.CommandInsert(dbc, "sp_InsertDO", keyValues);

                if (command.Connection.State != ConnectionState.Open)
                {
                    command.Connection.Open();
                }

                object obj = command.ExecuteScalar(); //execute query
                ResetCache();
                int retId = int.Parse(obj.ToString());
                return retId;
            }
        }




        #endregion

        #region " SaveDC Function "
         public static int SaveDC(DeliveryChalan DC)
        {
            using (var dbc = DataFactory.GetConnection())
            {
                Dictionary<string, object> keyValues = DataMap.reMapDCData(DC); //map podetail to db columns
                IDbCommand command = CommandBuilder.CommandInsert(dbc, "sp_InsertDC", keyValues);

                if (command.Connection.State != ConnectionState.Open)
                {
                    command.Connection.Open();
                }

                object obj = command.ExecuteScalar(); //execute query
                ResetCache();


                int retId = int.Parse(obj.ToString());
                DC.Id = retId;

                DeliveryOrder DO = GetDOById(DC.DeliveryOrder.Index);
                SaleOrder SO = GetSOByNumber(DO.SaleOrder.Value);
                DC = GetDCById(DC.Id);
                StoreDataManager.CreateStockMovement_DC(SO, DO, DC);
                return retId;
            }
        }
        public static void UpdateDC(DeliveryChalan DC)
        {
            using (var dbc = DataFactory.GetConnection())
            {
                Dictionary<string, object> keyValues = DataMap.reMapDCData(DC); //map podetail to db columns
                IDbCommand command = CommandBuilder.CommandInsert(dbc, "sp_UpdateDC", keyValues);

                if (command.Connection.State != ConnectionState.Open)
                {
                    command.Connection.Open();
                }

                object obj = command.ExecuteScalar(); //execute query
                ResetCache();
            }
        }



        #endregion

        #region " SaveDO Function "
        public static int UpdateDO(DeliveryOrder DO)
        {
            using (var dbc = DataFactory.GetConnection())
            {
                Dictionary<string, object> keyValues = DataMap.reMapDOData(DO); //map podetail to db columns
                IDbCommand command = CommandBuilder.CommandInsert(dbc, "sp_UpdateDO", keyValues);

                if (command.Connection.State != ConnectionState.Open)
                {
                    command.Connection.Open();
                }

                object obj = command.ExecuteScalar(); //execute query
                ResetCache();
                int retId = int.Parse(obj.ToString());
                return retId;
            }
        }
        #endregion

    }


}
