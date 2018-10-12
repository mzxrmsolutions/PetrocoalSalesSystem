using MZXRM.PSS.Business.DBMap;
using MZXRM.PSS.Common;
using MZXRM.PSS.Data;
using MZXRM.PSS.DataManager;
using System;
using System.Collections.Generic;
using System.Data;
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
                if (HttpContext.Current.Session[SessionManager.CustomerSession] != null)
                    _allCustomers = HttpContext.Current.Session[SessionManager.CustomerSession] as List<Customer>;
                if (_allCustomers == null || _allCustomers.Count == 0)
                    _allCustomers = ReadAllCustomers();
                return _allCustomers;
            }
        }
        public static List<Customer> ReadAllCustomers(bool alldata = true)
        {
            DataTable DTcust = CustomerDataManager.GetAllCustomer();
            if (!alldata)
                return CustomerMap.MapCustomerDataTable(DTcust);
            DataTable DTcuststock = CustomerDataManager.GetAllCustomerStock();
            DataTable DTcustdestination = CustomerDataManager.GetAllCustomerDestination();
            List<Customer> allcusts = CustomerMap.MapCustomerDataTable(DTcust, DTcuststock, DTcustdestination);
            List<Customer> calculatedCusts = new List<Customer>();

            foreach (Customer Cust in allcusts)
            {
                Customer cust = CustomerDataManager.CalculateCustomer(Cust);
                calculatedCusts.Add(cust);
            }
            HttpContext.Current.Session.Add(SessionManager.CustomerSession, calculatedCusts);
            _allCustomers = calculatedCusts;
            return calculatedCusts;
        }
        public static void ResetCache()
        {
            _allCustomers = null;
            HttpContext.Current.Session[SessionManager.CustomerSession] = null;

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
        public static Reference GetDefaultRef()
        {
            return new Reference() { Id = Guid.Empty, Name = "" };
        }
        public static Reference GetCustomerRefPSL()
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
        public static Item GetCustomerDestination(Guid custId, string destId)
        {
            foreach (Customer cust in AllCustomers)
            {
                if (cust.Id == custId)
                {
                    foreach (Item dest in cust.Destination)
                        if (dest.Index.ToString() == destId)
                            return dest;

                }
            }
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
                if (values.ContainsKey("cId"))
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

                int totalDestinations = int.Parse(values["TotalDestinations"]);
                for (int i = 1; i <= totalDestinations; i++)
                {
                    cust.Destination.Add(new Item() { Index = i, Value = values[i.ToString()] });
                }
                return cust;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Guid CreateCustomer(Customer cust)
        {
            Guid custId = CustomerDataManager.CreateCustomer(CustomerMap.reMapCustData(cust));
            if(custId!=null)
            {
                foreach(var m in cust.Destination)
                {
                     CustomerDataManager.CreateCustomerDestinations(CustomerMap.reMapCustDest(custId,m.Value));
                }
            }
            ResetCache();
            return custId;
        }
        public static Customer NewCustomer()
        {
            Customer c = new Customer();
            c.Id = Guid.Empty;
            c.Status = CustStatus.Active;
            c.CreatedOn = c.ModifiedOn = DateTime.Now;
            c.CreatedBy = c.ModifiedBy = c.Lead = new Reference() { Id = Common.CurrentUser.Id, Name = Common.CurrentUser.Name };
            c.Stock = new List<CustomerStock>();
            c.TotalStock = 0;
            c.Destination = new List<Item>();
            return c;
        }

        public static void UpdateCustomer(Customer cust)
        {
            CustomerDataManager.UpdateCustomer(CustomerMap.reMapCustData(cust));
        }
    }
}