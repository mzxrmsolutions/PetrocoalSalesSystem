using MZXRM.PSS.Common;
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


        #region Business need
        public static List<PurchaseOrder> ReadAllPO()
        {
            List<PurchaseOrder> AllPOs = new List<PurchaseOrder>();
            if (HttpContext.Current.Session[SessionManager.POSession] == null)
                readFromDB = true;
            if (readFromDB)
            {
                DataTable DTpo = GetAllPOs();
                DataTable DTpod = GetAllPODs();
                DataTable DTgrn = GetAllGRNs();
                DataTable DTdcl = GetAllDCLs();
                List<PurchaseOrder> allPOs = DataMap.MapPOData(DTpo, DTpod, DTgrn, DTdcl);
                foreach (PurchaseOrder PO in allPOs)
                {
                    PurchaseOrder po = CalculatePO(PO);
                    AllPOs.Add(po);
                }
                HttpContext.Current.Session.Add(SessionManager.POSession, AllPOs);
                readFromDB = false;
                return AllPOs;
            }
            AllPOs = HttpContext.Current.Session[SessionManager.POSession] as List<PurchaseOrder>;
            return AllPOs;
        }
        public static void ResetCache()
        {
            readFromDB = true;
        }
        public static PurchaseOrder CalculatePO(PurchaseOrder PO)
        {
            if (PO == null)
                return null;
            //if (PO.Id == Guid.Empty) PO.Id = Guid.NewGuid();
            PO.TotalQuantity = 0;
            PO.Received = 0;
            PO.DutyCleared = 0;
            PO.isPSL = false;
            foreach (PODetail pod in PO.PODetailsList)
            {
                if (pod.Customer.Id == CustomerDataManager.GetDefaultRef().Id)
                    PO.isPSL = true;
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
            PO.isValid = false;
            decimal maxAllowed = PO.TotalQuantity * PO.BufferQuantityMax / 100;
            decimal minAllowed = PO.TotalQuantity * PO.BufferQuantityMin / 100;
            if (PO.Received <= (PO.TotalQuantity + maxAllowed) && PO.DutyCleared <= PO.Received)
                PO.isValid = true;

            return PO;
        }
        public static bool SavePO(PurchaseOrder PO)
        {
            if (PO.Id == Guid.Empty)
            {
                Guid newPOId = CreatePO(PO);
                PO.Id = newPOId;
            }
            else
                UpdatePO(PO);
            foreach (PODetail pod in PO.PODetailsList)
            {
                if (pod.Id == Guid.Empty)
                {
                    pod.PO = new Reference() { Id = PO.Id, Name = PO.PONumber };
                    Guid newPODId = CreatePODetail(pod);
                    pod.Id = newPODId;
                }
                else
                {
                    UpdatePODetail(pod);
                }
                foreach (GRN grn in pod.GRNsList)
                {
                    if (grn.Id == Guid.Empty)
                    {
                        grn.PO = new Reference() { Id = PO.Id, Name = PO.PONumber };
                        grn.PODetail = new Reference() { Id = pod.Id, Name = PO.PONumber };
                        Guid newGrnId = CreateGRN(grn);
                        grn.Id = newGrnId;
                    }
                    else
                    {
                        UpdateGRN(grn);
                    }
                }
                foreach (DutyClear dcl in pod.DutyClearsList)
                {
                    if (dcl.Id == Guid.Empty)
                    {
                        dcl.PO = new Reference() { Id = PO.Id, Name = PO.PONumber };
                        dcl.PODetail = new Reference() { Id = pod.Id, Name = PO.PONumber };
                        Guid newDclId = CreateDCL(dcl);
                        dcl.Id = newDclId;
                    }
                    else
                    {
                        UpdateDCL(dcl);
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
        #endregion

        #region DB Update functions
        public static void UpdatePO(PurchaseOrder PO)
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
        public static void UpdatePODetail(PODetail POD)
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
        public static void UpdateGRN(GRN Grn)
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
        public static void UpdateDCL(DutyClear Dcl)
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
        #endregion

        #region DB Create functions
        public static Guid CreatePO(PurchaseOrder PO)
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
        public static Guid CreatePODetail(PODetail PODetail)
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
                readFromDB = true;
                return retId;
            }
        }
        public static Guid CreateGRN(GRN GRN)
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
        public static Guid CreateDCL(DutyClear DCL)
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
        #endregion

        #region DB Get all functions
        private static DataTable GetAllPOs()
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

        private static DataTable GetAllPODs()
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
                throw new Exception("Error! Get all POD from DataBase", ex);
            }
        }
        private static DataTable GetAllGRNs()
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
                throw new Exception("Error! Get all GRN from DataBase", ex);
            }
        }
        private static DataTable GetAllDCLs()
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
                throw new Exception("Error! Get all DCL from DataBase", ex);
            }
        }
        #endregion

        #region DB Get by ID functions
        #endregion




    }
}
