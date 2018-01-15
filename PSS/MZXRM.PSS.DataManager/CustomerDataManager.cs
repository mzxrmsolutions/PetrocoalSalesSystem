using MZXRM.PSS.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MZXRM.PSS.DataManager
{
   public class CustomerDataManager
    {
        static string _dataPath = ConfigurationManager.AppSettings["DataPath"];
        public static List<Customer> GetAllCustomers()
        {
            string filePath = _dataPath + "/Customer";
            List<Customer> allCustomers = new List<Customer>();

            if (Directory.Exists(filePath))
            {
                string[] files = Directory.GetFiles(filePath);
                foreach (string file in files)
                {
                    Customer customer = XMLUtil.ReadFromXmlFile<Customer>(file);
                    allCustomers.Add(customer);
                }
            }
            else
                Directory.CreateDirectory(filePath);
            return allCustomers;
        }
        public static bool SaveCustomer(Customer cust)
        {
            string poPath = _dataPath + "/Customer";
            string fileName = poPath + "/" + cust.Name + ".xml";
            XMLUtil.WriteToXmlFile<Customer>(fileName, cust);
            return true;
        }

    }
}
