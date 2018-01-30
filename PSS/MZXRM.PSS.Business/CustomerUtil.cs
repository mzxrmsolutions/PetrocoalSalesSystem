using MZXRM.PSS.Common;
using MZXRM.PSS.Data;
using MZXRM.PSS.DataManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MZXRM.PSS.Business
{
    public class CustomerUtil
    {
        private static List<Customer> _allCustomers;
        public static List<Customer> Customers
        {
            get
            {
                if (_allCustomers == null || _allCustomers.Count == 0)
                    _allCustomers = CustomerDataManager.GetAllCustomers();
                return _allCustomers;
            }
            set
            {
                _allCustomers = value;
            }
        }

        public static Reference GetCustomerRef(string id)
        {
            foreach (Customer c in Customers)
            {
                if (c.Id == new Guid(id))
                    return new Reference() { Id = c.Id, Name = c.Name };
            }
            ExceptionHandler.Error("Customer Not Found");
            return null;
        }
        public static Reference GetCustomerPSL()
        {
            foreach (Customer c in Customers)
            {
                if (c.Name == "PSL")
                    return new Reference() { Id = c.Id, Name = c.Name };
            }
            ExceptionHandler.Error("Customer Not Found");
            return null;
        }
    }
}