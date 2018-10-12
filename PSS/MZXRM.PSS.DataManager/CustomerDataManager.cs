using MZXRM.PSS.Common;
using MZXRM.PSS.Connector.Database;
using MZXRM.PSS.Data;
using MZXRM.PSS.DataManager;
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
    public class CustomerDataManager
    {
        static string _dataPath = ConfigurationManager.AppSettings["DataPath"];
        public static Customer CalculateCustomer(Customer Customer)
        {
            if (Customer != null)
            {
                Customer.TotalStock = 0;
                foreach (CustomerStock stock in Customer.Stock)
                {
                    if (stock.StockMinus)
                        Customer.TotalStock -= stock.Quantity;
                    else
                        Customer.TotalStock += stock.Quantity;
                }
                if (Customer.TotalStock < 0)
                    Customer.StockMinus = true;
                return Customer;
            }
            return null;
        }
        public static DataTable GetAllCustomer()
        {
            try
            {
                using (var dbc = DataFactory.GetConnection())
                {

                    IDbCommand command = CommandBuilder.CommandGetAll(dbc, "sp_GetAllCustomer");

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
                throw new Exception("Error! Get all Customer from DataBase", ex);
            }
        }
        public static DataTable GetAllCustomerStock()
        {
            try
            {
                using (var dbc = DataFactory.GetConnection())
                {

                    IDbCommand command = CommandBuilder.CommandGetAll(dbc, "sp_GetAllCustomerStock");

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
                throw new Exception("Error! Get all Customer from DataBase", ex);
            }
        }
        public static DataTable GetAllCustomerDestination()
        {
            try
            {
                using (var dbc = DataFactory.GetConnection())
                {

                    IDbCommand command = CommandBuilder.CommandGetAll(dbc, "sp_GetAllCustomerDestination");

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
                throw new Exception("Error! Get all Customer from DataBase", ex);
            }
        }
        public static Guid CreateCustomer(Dictionary<string, object> keyValues)
        {
            using (var dbc = DataFactory.GetConnection())
            {
                IDbCommand command = CommandBuilder.CommandInsert(dbc, "sp_InsertCustomer", keyValues);

                if (command.Connection.State != ConnectionState.Open)
                {
                    command.Connection.Open();
                }

                object obj = command.ExecuteScalar(); //execute query
                Guid retId = new Guid(obj.ToString());
                return retId;
            }
        }

        public static void CreateCustomerDestinations(Dictionary<string, object> keyValues)
        {
            using (var dbc = DataFactory.GetConnection())
            {
                IDbCommand command = CommandBuilder.CommandInsert(dbc, "sp_InsertCustomerDestination", keyValues);

                if (command.Connection.State != ConnectionState.Open)
                {
                    command.Connection.Open();
                }

                object obj = command.ExecuteScalar(); //execute query
                int retId =  int.Parse(obj.ToString());
                //return retId;
            }
        }

        public static void UpdateCustomer(Dictionary<string, object> keyValues)
        {
            using (var dbc = DataFactory.GetConnection())
            {
                IDbCommand command = CommandBuilder.CommandInsert(dbc, "sp_UpdateCustomer", keyValues);

                if (command.Connection.State != ConnectionState.Open)
                {
                    command.Connection.Open();
                }

                command.ExecuteNonQuery(); //execute query
            }
        }

       
    }
}
