using MZXRM.PSS.Common;
using MZXRM.PSS.Data;
using MZXRM.PSS.DataManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MZXRM.PSS.Business
{
    public class CustomerManager
    {
        private static List<Customer> _allCustomers;
        public static List<Customer> AllCustomers
        {
            get
            {
                //if (_allCustomers == null || _allCustomers.Count == 0)
                    _allCustomers = CustomerDataManager.ReadAllCustomers();
                return _allCustomers;
            }
            set
            {
                _allCustomers = value;
            }
        }

        public static Reference GetCustomerRef(string id)
        {
            foreach (Customer c in AllCustomers)
            {
                if (c.Id == new Guid(id))
                    return new Reference() { Id = c.Id, Name = c.Name };
            }
            ExceptionHandler.Error("Customer Not Found");
            return null;
        }
        public static Reference GetCustomerPSL()
        {
            foreach (Customer c in AllCustomers)
            {
                if (c.Name == "PSL")
                    return new Reference() { Id = c.Id, Name = c.Name };
            }
            ExceptionHandler.Error("Customer Not Found");
            return null;
        }
        public static Customer GetCustomer(Guid CustomerId)
        {
            foreach (Customer c in AllCustomers)
            {
                if (c.Id == CustomerId)
                    return c;
            }
            ExceptionHandler.Error("Customer Not Found");
            return null;
        }

        public static Customer ValidateCreateForm(Dictionary<string, string> values)
        {

            try
            {
                foreach (KeyValuePair<string, string> keyValue in values)
                {
                    string key = keyValue.Key;
                    string value = keyValue.Value;
                    if (key == "FullName" && value == "") throw new Exception("FullName is required");
                    if (key == "ShortName" && value == "") throw new Exception("ShortName is required");
                    if (key == "NTN" && value == "") throw new Exception("NTN is required");
                    if (key == "STRN" && value == "") throw new Exception("STRN is required");
                    if (key == "Address" && value == "") throw new Exception("Address is required");
                    if (key == "InvoiceAddress" && value == "") throw new Exception("InvoiceAddress is required");
                    if (key == "Email" && value == "") throw new Exception("Email is required");
                    if (key == "Phone" && value == "") throw new Exception("Phone is required");
                    if (key == "ContactPerson" && value == "") throw new Exception("ContactPerson is required");
                    if (key == "HeadOffice" && value == "") throw new Exception("HeadOffice is required");
                }
                Customer cust = new Customer();
                if (values["cId"] != null)
                    cust = GetCustomer(Guid.Parse(values["cId"]));
                else
                    cust = NewCustomer();
                cust.Name = values["FullName"];
                cust.ShortName = values["ShortName"];
                cust.NTN = values["NTN"];
                cust.STRN = values["STRN"];
                cust.Address = values["Address"];
                cust.InvoiceAddress = values["InvoiceAddress"];
                cust.Email = values["Email"];
                cust.Phone = values["Phone"];
                cust.ContactPerson = values["ContactPerson"];
                cust.HeadOffice = values["HeadOffice"];
                cust.Remarks = values["Remarks"];
                return cust;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Guid CreateCustomer(Customer cust)
        {
            Guid custId = CustomerDataManager.CreateCustomer(cust);
            return custId;
        }
        public static Customer NewCustomer()
        {
            Customer c = new Customer();
            c.Id = Guid.Empty;
            c.Status = CustStatus.Active;
            c.CreatedOn = c.ModifiedOn = DateTime.Now;
            c.CreatedBy = c.ModifiedBy = c.Lead= new Reference() { Id = Common.CurrentUser.Id, Name = Common.CurrentUser.Name };
            c.Stock = new List<CustomerStock>();
            c.TotalStock = 0;
            return c;
        }

        public static void UpdateCustomer( Customer cust)
        {
            CustomerDataManager.UpdateCustomer(cust);
        }
    }
}