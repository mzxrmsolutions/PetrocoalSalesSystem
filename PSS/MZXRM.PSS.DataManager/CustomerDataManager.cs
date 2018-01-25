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
        static string sessionName = "AllCustomers";
        public static List<Customer> GetAllCustomers()
        {
            if (HttpContext.Current.Session[sessionName] == null)
                readFromDB = true;
            if (readFromDB)
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
                        List<Customer> allCustomers = DataMap.MapCustomerDataTable(dt);
                        HttpContext.Current.Session.Add(sessionName, allCustomers);
                        readFromDB = false;
                        return allCustomers;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error! Get all Customer from DataBase", ex);
                }
            }
            List<Customer> allCust = HttpContext.Current.Session[sessionName] as List<Customer>;
            return allCust;
        }
        public static bool SaveCustomer(Customer cust)
        {
            string poPath = _dataPath + "/Customer";
            string fileName = poPath + "/" + cust.Name + ".xml";
            XMLUtil.WriteToXmlFile<Customer>(fileName, cust);
            return true;
        }

        public static Reference GetCustRef(string id)
        {
            Guid custId = new Guid(id);
            if (custId != Guid.Empty)
            {
                List<Customer> allCustomers = GetAllCustomers();
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
