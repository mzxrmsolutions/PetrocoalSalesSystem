﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PatrocoalSalesSystem.Models
{
    public class CustomerUtil
    {
        private static List<Customer> _allCustomers;
        public static List<Customer> Customers
        {
            get
            {
                if (_allCustomers == null || _allCustomers.Count == 0)
                    _allCustomers = DBUtil.GetAllCustomers();
                return _allCustomers;
            }
            set
            {
                _allCustomers = value;
            }
        }

        internal static Reference GetCustomerRef(string id)
        {
            foreach (Customer c in Customers)
            {
                if (c.Id == new Guid(id))
                    return new Reference() { Id = c.Id, Name = c.Name };
            }
            ExceptionHandler.Error("Customer Not Found");
            return null;
        }
    }
}