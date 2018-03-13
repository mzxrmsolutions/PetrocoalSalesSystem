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

                List<SaleOrder> allSOs = DataMap.MapSOData(DTso, DTdo);
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
            //TODO
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

        #region " GetAllDOs Function "

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

            }
        }
        #endregion
        public static void ResetCache()
        {
            readFromDB = true;
        }
    }
}
