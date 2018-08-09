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
        #region Business need
        #endregion

        #region DB Update functions
        public static void UpdatePO(Dictionary<string, object> keyValues)
        {
            using (var dbc = DataFactory.GetConnection())
            {
                IDbCommand command = CommandBuilder.CommandInsert(dbc, "sp_UpdatePO", keyValues);

                if (command.Connection.State != ConnectionState.Open)
                {
                    command.Connection.Open();
                }

                command.ExecuteNonQuery(); //execute query
            }
        }
        public static void UpdatePODetail(Dictionary<string, object> keyValues)
        {
            using (var dbc = DataFactory.GetConnection())
            {
                IDbCommand command = CommandBuilder.CommandInsert(dbc, "sp_UpdatePODetail", keyValues);

                if (command.Connection.State != ConnectionState.Open)
                {
                    command.Connection.Open();
                }

                command.ExecuteNonQuery(); //execute query
            }
        }
        public static void UpdateGRN(Dictionary<string, object> keyValues)
        {
            using (var dbc = DataFactory.GetConnection())
            {
                IDbCommand command = CommandBuilder.CommandInsert(dbc, "sp_UpdateGRN", keyValues);

                if (command.Connection.State != ConnectionState.Open)
                {
                    command.Connection.Open();
                }

                command.ExecuteNonQuery(); //execute query
            }
        }
        public static void UpdateDCL(Dictionary<string, object> keyValues)
        {
            using (var dbc = DataFactory.GetConnection())
            {
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
        public static Guid CreatePO(Dictionary<string, object> keyValues)
        {
            using (var dbc = DataFactory.GetConnection())
            {
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
        public static Guid CreatePODetail(Dictionary<string, object> keyValues)
        {
            using (var dbc = DataFactory.GetConnection())
            {
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
        public static Guid CreateGRN(Dictionary<string, object> keyValues)
        {
            using (var dbc = DataFactory.GetConnection())
            {
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
        public static Guid CreateDCL(Dictionary<string, object> keyValues)
        {
            using (var dbc = DataFactory.GetConnection())
            {
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
        public static DataTable GetAllPOs()
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
        

        public static DataTable GetAllPODs()
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
        public static DataTable GetAllGRNs()
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
        public static DataTable GetAllDCLs()
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
                if (pod.Customer.Id == Guid.Empty)
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



    }
}
