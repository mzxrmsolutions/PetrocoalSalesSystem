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
        static bool readFromDB = true;
        public static List<Customer> ReadAllCustomers()
        {
            List<Customer> AllCustomers = new List<Customer>();
            if (HttpContext.Current.Session[SessionManager.CustomerSession] == null)
                readFromDB = true;
            if (readFromDB)
            {
                DataTable DTcust = GetAllCustomer();
                DataTable DTcuststock = GetAllCustomerStock();
                List<Customer> allcusts = DataMap.MapCustomerDataTable(DTcust, DTcuststock);
                foreach (Customer Cust in allcusts)
                {
                    Customer cust = CalculateCustomer(Cust);
                    AllCustomers.Add(cust);
                }
                HttpContext.Current.Session.Add(SessionManager.CustomerSession, AllCustomers);
                readFromDB = false;
                return AllCustomers;
            }
            AllCustomers = HttpContext.Current.Session[SessionManager.CustomerSession] as List<Customer>;
            return AllCustomers;

        }
        public static void ResetCache()
        {
            readFromDB = true;
        }
        private static Customer CalculateCustomer(Customer Customer)
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
        private static DataTable GetAllCustomer()
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



        private static DataTable GetAllCustomerStock()
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

        public static Guid CreateCustomer(Customer cust)
        {
            using (var dbc = DataFactory.GetConnection())
            {
                Dictionary<string, object> keyValues = DataMap.reMapCustData(cust); //map po to db columns
                IDbCommand command = CommandBuilder.CommandInsert(dbc, "sp_InsertCustomer", keyValues);

                if (command.Connection.State != ConnectionState.Open)
                {
                    command.Connection.Open();
                }

                ResetCache();
                object obj = command.ExecuteScalar(); //execute query
                Guid retId = new Guid(obj.ToString());
                return retId;
            }
        }

        public static bool SaveCustomer(Customer cust)
        {
            string poPath = _dataPath + "/Customer";
            string fileName = poPath + "/" + cust.Name + ".xml";
            XMLUtil.WriteToXmlFile<Customer>(fileName, cust);
            return true;
        }

        public static void UpdateCustomer(Customer cust)
        {
            using (var dbc = DataFactory.GetConnection())
            {
                Dictionary<string, object> keyValues = DataMap.reMapCustData(cust); //map po to db columns
                IDbCommand command = CommandBuilder.CommandInsert(dbc, "sp_UpdateCustomer", keyValues);

                if (command.Connection.State != ConnectionState.Open)
                {
                    command.Connection.Open();
                }

                command.ExecuteNonQuery(); //execute query
                ResetCache();
            }
        }

        public static Reference GetCustRef(string id)
        {
            Guid custId = new Guid(id);
            if (custId != Guid.Empty)
            {
                List<Customer> allCustomers = ReadAllCustomers();
                foreach (Customer cust in allCustomers)
                    if (custId == cust.Id)
                        return new Reference() { Id = cust.Id, Name = cust.Name };
            }
            return new Reference() { Id = Guid.Empty, Name = "" };
        }
        public static Reference GetDefaultRef()
        {
            return new Reference() { Id = Guid.Empty, Name = "" };
        }
    }
}
