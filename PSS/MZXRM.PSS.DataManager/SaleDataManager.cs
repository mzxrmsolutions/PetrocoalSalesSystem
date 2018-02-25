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
        public static List<SaleOrder> ReadAllSO()
        {
            List<SaleOrder> AllSOs = new List<SaleOrder>();
            if (HttpContext.Current.Session[SessionManager.SOSession] == null)
                readFromDB = true;
            if (readFromDB)
            {
                DataTable DTso = GetAllSOs();

                List<SaleOrder> allSOs = DataMap.MapSOData(DTso);
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

        private static SaleOrder CalculateSO(SaleOrder SO)
        {
            //TODO
            return SO;
        }

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





        #region TODO
        // K:TODO
        /* private static DataTable GetAllSO()
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
         private static DataTable GetAllDO();
         private static DataTable GetAllDC();
         private static DataTable GetAllDCR();
         private static int CreateSO(SaleOrder SO);
         private static int CreateDO(DeliveryOrder DO);
         private static int CreateDC(DeliveryChallan DC);
         private static int CreateDCR(DCReturn DCR);
         private static void UpdateSO(SaleOrder SO);
         private static void UpdateDO(DeliveryOrder DO);
         private static void UpdateDC(DeliveryChallan DC);
         private static void UpdateDCR(DCReturn DCR);*/
        #endregion
    }
}
