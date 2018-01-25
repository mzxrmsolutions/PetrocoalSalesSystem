using MZXRM.PSS.Connector.Database;
using MZXRM.PSS.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MZXRM.PSS.DataManager
{
    public static class PurchaseDataManager
    {
        static string _dataPath = ConfigurationManager.AppSettings["DataPath"];
        static bool readFromDB = true;
        static string sessionName = "AllPurchaseOrders";
        public static List<PurchaseOrder> ReadAllPO()
        {
            List<PurchaseOrder> AllPOs = new List<PurchaseOrder>();
            if (HttpContext.Current.Session[sessionName] == null)
                readFromDB = true;
            if (readFromDB)
            {
                DataTable DTpo = DB_ReadAllPO();
                DataTable DTpod = DB_ReadAllPODetail();
                DataTable DTgrn = DB_ReadAllGRN();
                DataTable DTdcl = DB_ReadAllDCL();
             List<PurchaseOrder>   allPOs = DataMap.MapPOData(DTpo, DTpod, DTgrn, DTdcl);
                foreach (PurchaseOrder PO in allPOs)
                {
                    PurchaseOrder po = CalculatePO(PO);
                    AllPOs.Add(po);
                }
                HttpContext.Current.Session.Add(sessionName, AllPOs);
                readFromDB = false;
                return AllPOs;
            }
            AllPOs = HttpContext.Current.Session[sessionName] as List<PurchaseOrder>;
            return AllPOs;
        }

        private static PurchaseOrder CalculatePO(PurchaseOrder PO)
        {
            if (PO == null)
                return null;
            //if (PO.Id == Guid.Empty) PO.Id = Guid.NewGuid();
            PO.TotalQuantity = 0;
            PO.Received = 0;
            PO.DutyCleared = 0;
            foreach (PODetail pod in PO.PODetailsList)
            {
                pod.Received = 0;
                pod.DutyCleared = 0;
                pod.Wastage = 0;

                foreach (GRN grn in pod.GRNsList)
                {
                    pod.Received += grn.Quantity;
                }
                foreach (DutyClear dc in pod.DutyClearsList)
                {
                    pod.DutyCleared += dc.Quantity;
                }
                pod.Wastage = pod.DutyCleared * pod.AllowedWaistage / 100;
                pod.Remaining = pod.Quantity - pod.Received;
                pod.DutyRemaining = pod.Received - pod.DutyCleared;

                PO.TotalQuantity += pod.Quantity;
                PO.Received += pod.Received;
                PO.DutyCleared += pod.DutyCleared;
            }
            return PO;
        }

        public static bool SavePO(PurchaseOrder PO)
        {
            if (PO.Id == Guid.Empty)
            {
                Guid newPOId = DB_CreatePO(PO);
                PO.Id = newPOId;
            }
            else
                DB_UpdatePO(PO);
            foreach (PODetail pod in PO.PODetailsList)
            {
                if (pod.Id == Guid.Empty)
                {
                    pod.PO = new Reference() { Id = PO.Id, Name = PO.PONumber };
                    Guid newPODId = DB_CreatePODetail(pod);
                    pod.Id = newPODId;
                }
                else
                {
                    DB_UpdatePODetail(pod);
                }
                foreach (GRN grn in pod.GRNsList)
                {
                    if (grn.Id == Guid.Empty)
                    {
                        grn.PO = new Reference() { Id = PO.Id, Name = PO.PONumber };
                        grn.PODetail = new Reference() { Id = pod.Id, Name = PO.PONumber };
                        Guid newGrnId = DB_CreateGRN(grn);
                        grn.Id = newGrnId;
                    }
                    else
                    {
                        DB_UpdateGRN(grn);
                    }
                }
                foreach (DutyClear dcl in pod.DutyClearsList)
                {
                    if (dcl.Id == Guid.Empty)
                    {
                        dcl.PO = new Reference() { Id = PO.Id, Name = PO.PONumber };
                        dcl.PODetail = new Reference() { Id = pod.Id, Name = PO.PONumber };
                        Guid newDclId = DB_CreateDCL(dcl);
                        dcl.Id = newDclId;
                    }
                    else
                    {
                        DB_UpdateDCL(dcl);
                    }
                }
            }
            readFromDB = true;



            /*string poPath = _dataPath + "/PO";
            string fileName = poPath + "/" + PO.PONumber + ".xml";
            XMLUtil.WriteToXmlFile<PurchaseOrder>(fileName, PO);
            HttpContext.Current.Session.Remove("DBPurchaseOrders");
            ReadAllPO();*/
            return true;
        }

        public static void DB_UpdatePO(PurchaseOrder PO)
        {
            using (var dbc = DataFactory.GetConnection())
            {
                Dictionary<string, object> keyValues = DataMap.reMapPOData(PO); //map po to db columns
                IDbCommand command = CommandBuilder.CommandInsert(dbc, "sp_UpdatePO", keyValues);

                if (command.Connection.State != ConnectionState.Open)
                {
                    command.Connection.Open();
                }

                command.ExecuteNonQuery(); //execute query
            }
        }
        public static void DB_UpdatePODetail(PODetail POD)
        {
            using (var dbc = DataFactory.GetConnection())
            {
                Dictionary<string, object> keyValues = DataMap.reMapPODetailData(POD); //map po to db columns
                IDbCommand command = CommandBuilder.CommandInsert(dbc, "sp_UpdatePODetail", keyValues);

                if (command.Connection.State != ConnectionState.Open)
                {
                    command.Connection.Open();
                }

                command.ExecuteNonQuery(); //execute query
            }
        }
        public static void DB_UpdateGRN(GRN Grn)
        {
            using (var dbc = DataFactory.GetConnection())
            {
                Dictionary<string, object> keyValues = DataMap.reMapGRNData(Grn); //map po to db columns
                IDbCommand command = CommandBuilder.CommandInsert(dbc, "sp_UpdateGRN", keyValues);

                if (command.Connection.State != ConnectionState.Open)
                {
                    command.Connection.Open();
                }

                command.ExecuteNonQuery(); //execute query
            }
        }
        public static void DB_UpdateDCL(DutyClear Dcl)
        {
            using (var dbc = DataFactory.GetConnection())
            {
                Dictionary<string, object> keyValues = DataMap.reMapDCLData(Dcl); //map po to db columns
                IDbCommand command = CommandBuilder.CommandInsert(dbc, "sp_UpdateDCL", keyValues);

                if (command.Connection.State != ConnectionState.Open)
                {
                    command.Connection.Open();
                }

                command.ExecuteNonQuery(); //execute query
            }
        }

        public static Guid DB_CreatePO(PurchaseOrder PO)
        {
            using (var dbc = DataFactory.GetConnection())
            {
                Dictionary<string, object> keyValues = DataMap.reMapPOData(PO); //map po to db columns
                IDbCommand command = CommandBuilder.CommandInsert(dbc, "sp_InsertPO", keyValues);

                if (command.Connection.State != ConnectionState.Open)
                {
                    command.Connection.Open();
                }

                object obj = command.ExecuteScalar(); //execute query
                Guid retId = new Guid(obj.ToString());
                return retId;
            }
        }
        public static Guid DB_CreatePODetail(PODetail PODetail)
        {
            using (var dbc = DataFactory.GetConnection())
            {
                Dictionary<string, object> keyValues = DataMap.reMapPODetailData(PODetail); //map podetail to db columns
                IDbCommand command = CommandBuilder.CommandInsert(dbc, "sp_InsertPODetail", keyValues);

                if (command.Connection.State != ConnectionState.Open)
                {
                    command.Connection.Open();
                }

                object obj = command.ExecuteScalar(); //execute query
                Guid retId = new Guid(obj.ToString());
                return retId;
            }
        }
        public static Guid DB_CreateGRN(GRN GRN)
        {
            using (var dbc = DataFactory.GetConnection())
            {
                Dictionary<string, object> keyValues = DataMap.reMapGRNData(GRN); //map grn to db columns
                IDbCommand command = CommandBuilder.CommandInsert(dbc, "sp_InsertGRN", keyValues);

                if (command.Connection.State != ConnectionState.Open)
                {
                    command.Connection.Open();
                }

                object obj = command.ExecuteScalar(); //execute query
                Guid retId = new Guid(obj.ToString());
                return retId;
            }
        }
        public static Guid DB_CreateDCL(DutyClear DCL)
        {
            using (var dbc = DataFactory.GetConnection())
            {
                Dictionary<string, object> keyValues = DataMap.reMapDCLData(DCL); //map dcl to db columns
                IDbCommand command = CommandBuilder.CommandInsert(dbc, "sp_InsertDCL", keyValues);

                if (command.Connection.State != ConnectionState.Open)
                {
                    command.Connection.Open();
                }

                object obj = command.ExecuteScalar(); //execute query, no result
                Guid retId = new Guid(obj.ToString());
                return retId;
            }
        }
        private static DataTable DB_ReadAllPO()
        {
            try
            {
                using (var dbc = DataFactory.GetConnection())
                {

                    IDbCommand command = CommandBuilder.CommandGetAll(dbc, "sp_GetAllPO");

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
                throw new Exception("Error! Get all PO from DataBase", ex);
            }
        }
        private static DataTable DB_ReadAllPODetail()
        {
            try
            {
                using (var dbc = DataFactory.GetConnection())
                {

                    IDbCommand command = CommandBuilder.CommandGetAll(dbc, "sp_GetAllPODetail");

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
                throw new Exception("Error! Get all PO from DataBase", ex);
            }
        }
        private static DataTable DB_ReadAllGRN()
        {
            try
            {
                using (var dbc = DataFactory.GetConnection())
                {

                    IDbCommand command = CommandBuilder.CommandGetAll(dbc, "sp_GetAllGRN");

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
                throw new Exception("Error! Get all PO from DataBase", ex);
            }
        }
        private static DataTable DB_ReadAllDCL()
        {
            try
            {
                using (var dbc = DataFactory.GetConnection())
                {

                    IDbCommand command = CommandBuilder.CommandGetAll(dbc, "sp_GetAllDCL");

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
                throw new Exception("Error! Get all PO from DataBase", ex);
            }
        }



    }
}
