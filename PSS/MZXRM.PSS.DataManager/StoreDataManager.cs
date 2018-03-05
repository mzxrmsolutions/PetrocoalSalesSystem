﻿using System;
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
        private static DataTable GetAllStore()
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
        private static DataTable GetAllCustomerStock() {
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
        private static DataTable GetAllStoreInOut()
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

        #region Business Need
        public static List<Store> ReadAllStore()
        {
            // Z:TODO
            List<Store> AllStores = new List<Store>();
            if (HttpContext.Current.Session[SessionManager.StoreSession] == null)
                readFromDB = true;
            if (readFromDB)
            {
                DataTable DTstore = GetAllStore();
                DataTable DTcustStock = GetAllCustomerStock();
                //GetAllStockMovement();
                List<Store> allStores = DataMap.MapStoreData(DTstore,DTcustStock);
                foreach (Store store in allStores)
                {
                    Store CalculatedStore = CalculateStoreQuantity(store);
                    AllStores.Add(CalculatedStore);
                }
                HttpContext.Current.Session.Add(SessionManager.StoreSession, AllStores);
                readFromDB = false;
                return AllStores;
            }
            AllStores = HttpContext.Current.Session[SessionManager.StoreSession] as List<Store>;
            return AllStores;
        }

        public static Store CalculateStoreQuantity(Store Store)
        {
            // Z:TODO
            return Store;
        }
        public static void ResetCache()
        {
            readFromDB = true;
        }
        public static List<StoreInOut> ReadAllStoreIO()
        {
            List<StoreInOut> AllStores = new List<StoreInOut>();
            if (HttpContext.Current.Session[SessionManager.StoreIOSession] == null)
                readFromDB = true;
            if (readFromDB)
            {
                DataTable DTstoreIO = GetAllStoreInOut();
                List<StoreInOut> allStoreIOs = DataMap.MapStoreIOData(DTstoreIO);
                //foreach (StoreInOut storeIO in allStoreIOs)
                //{
                //    StoreInOut CalculatedStore = CalculateStoreQuantity(storeIO);
                //    AllStores.Add(CalculatedStore);
                //}
                HttpContext.Current.Session.Add(SessionManager.StoreSession, AllStores);
                readFromDB = false;
                return AllStores;
            }
            AllStores = HttpContext.Current.Session[SessionManager.StoreSession] as List<StoreInOut>;
            return AllStores;
        }

        
        #endregion



        #region Get Reference
        public static Reference GetStoreRef(string id)
        {
            Guid storeId = !string.IsNullOrEmpty(id) ? new Guid(id) : Guid.Empty;
            Store store = GetStore(storeId);
            if (store != null)
                return new Reference() { Id = store.Id, Name = store.Name };
            return GetDefaultRef();
        }

        public static Store GetStore(Guid storeId)
        {
            if (storeId != Guid.Empty)
            {
                List<Store> Stores = ReadAllStore();
                foreach (Store store in Stores)
                {
                    if (store.Id == storeId)
                        return store;
                }
            }
            return null;
        }

        public static Reference GetDefaultRef()
        {
            return new Reference() { Id = Guid.Empty, Name = "" };
        }

        public static Guid CreateStockMovement(DutyClear DCL)
        {
            using (var dbc = DataFactory.GetConnection())
            {
                Dictionary<string, object> keyValues = DataMap.reMapStockMovementData(DCL); //map dcl to db columns
                IDbCommand command = CommandBuilder.CommandInsert(dbc, "sp_InsertStockMovement", keyValues);

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








    }
}