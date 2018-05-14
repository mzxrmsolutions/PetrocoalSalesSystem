using System;
using MZXRM.PSS.Data;
using System.Collections.Generic;
using System.Data;
using MZXRM.PSS.Connector.Database;
using System.Web;
using MZXRM.PSS.Common;

namespace MZXRM.PSS.DataManager
{
    public class StoreDataManager
    {
        static bool readFromDB = true;

        #region DB functions
        // K:TODO
        public static DataTable GetAllStore()
        {
            try
            {
                using (var dbc = DataFactory.GetConnection())
                {

                    IDbCommand command = CommandBuilder.CommandGetAll(dbc, "sp_GetAllStore");

                    if (command.Connection.State != ConnectionState.Open)
                    {
                        command.Connection.Open();
                    }

                    IDataReader datareader = command.ExecuteReader(); // execute select query
                    DataTable dt = new DataTable();
                    dt.Load(datareader);
                    return dt;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error! Get all from DataBase", ex);
            }
        }
        public static DataTable GetAllStockMovements()
        {
            try
            {
                using (var dbc = DataFactory.GetConnection())
                {

                    IDbCommand command = CommandBuilder.CommandGetAll(dbc, "sp_GetAllStockMovement");

                    if (command.Connection.State != ConnectionState.Open)
                    {
                        command.Connection.Open();
                    }

                    IDataReader datareader = command.ExecuteReader(); // execute select query
                    DataTable dt = new DataTable();
                    dt.Load(datareader);

                    return dt;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error! Get all from DataBase", ex);
            }
        }
        public static DataTable GetAllCustomerStock() {
            try
            {
                using (var dbc = DataFactory.GetConnection())
                {

                    IDbCommand command = CommandBuilder.CommandGetAll(dbc, "sp_GetAllCustomerStock");

                    if (command.Connection.State != ConnectionState.Open)
                    {
                        command.Connection.Open();
                    }

                    IDataReader datareader = command.ExecuteReader(); // execute select query
                    DataTable dt = new DataTable();
                    dt.Load(datareader);
                    return dt;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error! Get all from DataBase", ex);
            }
        }

        

        public static void CreateStoreTransfer(Dictionary<string, object> keyValues)
        {
            using (var dbc = DataFactory.GetConnection())
            {
                IDbCommand command = CommandBuilder.CommandInsert(dbc, "sp_InsertStoreInOut", keyValues);

                if (command.Connection.State != ConnectionState.Open)
                {
                    command.Connection.Open();
                }

                object obj = command.ExecuteScalar(); //execute query
                //todo                                      //
                //StoreDataManager.CreateStockMovementStoreOut(ST);
                //Guid retId = new Guid(obj.ToString());
                //return retId;
            }
        }
        public static void UpdateStoreTransfer(Dictionary<string, object> keyValues)
        {
            using (var dbc = DataFactory.GetConnection())
            {
                IDbCommand command = CommandBuilder.CommandInsert(dbc, "sp_UpdateStoreInOut", keyValues);

                if (command.Connection.State != ConnectionState.Open)
                {
                    command.Connection.Open();
                }

                object obj = command.ExecuteScalar(); //execute query
                //StoreDataManager.CreateStockMovementStoreOut(ST);
                //Guid retId = new Guid(obj.ToString());
                //return retId;
            }
        }

        /*private static DataTable GetAllStockMovement();
private static Guid CreateStore(Store Store)
{
   try
   {
       using (var dbc = DataFactory.GetConnection())
       {
           Dictionary<string, object> keyValues = DataMap.reMapStoreData(Store); //map object to db columns
           IDbCommand command = CommandBuilder.CommandInsert(dbc, "sp_InsertStore", keyValues);

           if (command.Connection.State != ConnectionState.Open)
           {
               command.Connection.Open();
           }

           object obj = command.ExecuteScalar(); //execute create query
           Guid retId = new Guid(obj.ToString());
           return retId;
       }
   }
   catch (Exception ex)
   {
       throw new Exception("Error! Create in dataBase", ex);
   }
}
private static Guid CreateStockMovement(StockMovement StockMovement);
private static Guid CreateCustomerStock(CustomerStock CustomerStock);
private static Store GetStoreById(Guid id);
private static Store GetCustomerStockById(Guid id);
private static Store GetStockMovementById(Guid id);
private static void UpdateStore(Store Store);
private static void UpdateStockMovement(StockMovement StockMovement);
private static void UpdateCustomerStock(CustomerStock CustomerStock);*/
        public static DataTable GetAllStoreInOut()
        {
            try
            {
                using (var dbc = DataFactory.GetConnection())
                {

                    IDbCommand command = CommandBuilder.CommandGetAll(dbc, "sp_GetAllStoreInOut");

                    if (command.Connection.State != ConnectionState.Open)
                    {
                        command.Connection.Open();
                    }

                    IDataReader datareader = command.ExecuteReader(); // execute select query
                    DataTable dt = new DataTable();
                    dt.Load(datareader);
                    return dt;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error! Get all from DataBase", ex);
            }
        }
        #endregion




        public static Guid CreateStockMovement(Dictionary<string, object> keyValues)
        {
            using (var dbc = DataFactory.GetConnection())
            {
                IDbCommand command = CommandBuilder.CommandInsert(dbc, "sp_InsertStockMovement", keyValues);

                if (command.Connection.State != ConnectionState.Open)
                {
                    command.Connection.Open();
                }

                object obj = command.ExecuteScalar(); //execute query, no result
                //Guid retId = new Guid(obj.ToString());
                return Guid.Empty;
            }
        }
        public static Guid CreateStockMovementStoreOut(Dictionary<string, object> keyValues)
        {
            using (var dbc = DataFactory.GetConnection())
            {
                IDbCommand command = CommandBuilder.CommandInsert(dbc, "sp_InsertStockMovement", keyValues);

                if (command.Connection.State != ConnectionState.Open)
                {
                    command.Connection.Open();
                }

                object obj = command.ExecuteScalar(); //execute query, no result
                //Guid retId = new Guid(obj.ToString());
                return Guid.Empty;
            }
        }

        public static Guid CreateStockMovement_DC(Dictionary<string, object> keyValues)
        {
            using (var dbc = DataFactory.GetConnection())
            {
                IDbCommand command = CommandBuilder.CommandInsert(dbc, "sp_InsertStockMovement", keyValues);

                if (command.Connection.State != ConnectionState.Open)
                {
                    command.Connection.Open();
                }

                object obj = command.ExecuteScalar(); //execute query, no result
                //Guid retId = new Guid(obj.ToString());
                return Guid.Empty;
            }
        }








    }
}