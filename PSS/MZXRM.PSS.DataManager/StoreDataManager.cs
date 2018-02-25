using System;
using MZXRM.PSS.Data;
using System.Collections.Generic;
using System.Data;
using MZXRM.PSS.Connector.Database;

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
        /*private static DataTable GetAllCustomerStock();
        private static DataTable GetAllStockMovement();
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
        #endregion

        #region Business Need
        public static List<Store> ReadAllStore()
        {
            // Z:TODO
            //GetAllStore();
            //GetAllCustomerStock();
            //GetAllStockMovement();
            return new List<Store>();
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
        #endregion



        #region Get Reference
        public static Reference GetStoreRef(string id)
        {
            // Z:TODO
            return new Reference() { Id = Guid.Empty, Name = "" };
        }

        public static Reference GetDefaultRef()
        {
            return new Reference() { Id = Guid.Empty, Name = "" };
        }
        #endregion








    }
}