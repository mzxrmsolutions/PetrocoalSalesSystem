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

        

        #region " CalculateSO Function "
        public static SaleOrder CalculateSO(SaleOrder SO)
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
        public static DataTable GetAllSOs()
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
        public static DataTable GetAllDOs()
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

        
        


        #region " GetAllDCs Function "
        public static DataTable GetAllDCs()
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
        public static int SaveSO(Dictionary<string, object> keyValues)
        {
            using (var dbc = DataFactory.GetConnection())
            {
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
        public static void UpdateSO(Dictionary<string, object> keyValues)
        {
            using (var dbc = DataFactory.GetConnection())
            {
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
       

        #region " SaveDO Function "
        public static int SaveDO(Dictionary<string, object> keyValues)
        {
            using (var dbc = DataFactory.GetConnection())
            {
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
         public static int SaveDC(Dictionary<string, object> keyValues)
        {
            using (var dbc = DataFactory.GetConnection())
            {
                IDbCommand command = CommandBuilder.CommandInsert(dbc, "sp_InsertDC", keyValues);

                if (command.Connection.State != ConnectionState.Open)
                {
                    command.Connection.Open();
                }

                object obj = command.ExecuteScalar(); //execute query
                ResetCache();


                int retId = int.Parse(obj.ToString());
                //StoreDataManager.CreateStockMovement_DC(SO, DO, DC);
                return retId;
            }
        }
        public static void UpdateDC(Dictionary<string, object> keyValues)
        {
            using (var dbc = DataFactory.GetConnection())
            {
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
        public static int UpdateDO(Dictionary<string, object> keyValues)
        {
            using (var dbc = DataFactory.GetConnection())
            {
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
