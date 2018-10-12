using MZXRM.PSS.Data;
using MZXRM.PSS.Business;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MZXRM.PSS.DataManager;

namespace MZXRM.PSS.Business.DBMap
{
   public class CustomerMap
    {
        public static List<Customer> MapCustomerDataTable(DataTable dtCust)
        {
            List<Customer> AllCustomers = new List<Customer>();
            foreach (DataRow drCust in dtCust.Rows)
            {
                Customer Cust = new Customer();

                Cust.Id = drCust["Id"] != null ? new Guid(drCust["Id"].ToString()) : Guid.Empty;
                Cust.Status = drCust["Status"] != null ? CustStatus.Active : CustStatus.InActive;
                Cust.CreatedOn = drCust["CreatedOn"] != DBNull.Value ? DateTime.Parse(drCust["CreatedOn"].ToString()) : DateTime.MinValue;
                Cust.CreatedBy = drCust["CreatedBy"] != null ? UserManager.GetUserRef(drCust["CreatedBy"].ToString()) : UserManager.GetDefaultRef();
                Cust.ModifiedOn = drCust["ModifiedOn"] != DBNull.Value ? DateTime.Parse(drCust["ModifiedOn"].ToString()) : DateTime.MinValue;
                Cust.ModifiedBy = drCust["ModifiedBy"] != null ? UserManager.GetUserRef(drCust["ModifiedBy"].ToString()) : UserManager.GetDefaultRef();
                Cust.Lead = drCust["Lead"] != null ? UserManager.GetUserRef(drCust["Lead"].ToString()) : UserManager.GetDefaultRef();
                Cust.Name = drCust["FullName"] != null ? drCust["FullName"].ToString() : "";
                Cust.ShortName = drCust["ShortName"] != null ? drCust["ShortName"].ToString() : "";
                Cust.NTN = drCust["NTN"] != null ? drCust["NTN"].ToString() : "";
                Cust.STRN = drCust["STRN"] != null ? drCust["STRN"].ToString() : "";
                Cust.Address = drCust["Address"] != null ? drCust["Address"].ToString() : "";
                Cust.InvoiceAddress = drCust["InvoiceAddress"] != null ? drCust["InvoiceAddress"].ToString() : "";
                Cust.Email = drCust["Email"] != null ? drCust["Email"].ToString() : "";
                Cust.Phone = drCust["Phone"] != null ? drCust["Phone"].ToString() : "";
                Cust.ContactPerson = drCust["ContactPerson"] != null ? drCust["ContactPerson"].ToString() : "";
                Cust.HeadOffice = drCust["HeadOffice"] != null ? drCust["HeadOffice"].ToString() : "";
                Cust.Remarks = drCust["Remarks"] != null ? drCust["Remarks"].ToString() : "";
                Cust.Stock = new List<CustomerStock>();
                Cust.Destination = new List<Item>();
                AllCustomers.Add(Cust);
            }
            return AllCustomers;
        }
        public static Dictionary<string, object> reMapCustData(Customer c)
        {
            Dictionary<string, object> keyValues = new Dictionary<string, object>();
            if (c.Id != Guid.Empty)
                keyValues.Add("@id", c.Id);
            keyValues.Add("@Status", c.Status);
            if (c.CreatedOn == DateTime.MinValue)
                keyValues.Add("@CreatedOn", DBNull.Value);
            else
                keyValues.Add("@CreatedOn", c.CreatedOn);
            keyValues.Add("@CreatedBy", c.CreatedBy == null ? Guid.Empty : c.CreatedBy.Id);
            if (c.ModifiedOn == DateTime.MinValue)
                keyValues.Add("@ModifiedOn", DBNull.Value);
            else
                keyValues.Add("@ModifiedOn", c.ModifiedOn);
            keyValues.Add("@ModifiedBy", c.ModifiedBy == null ? Guid.Empty : c.ModifiedBy.Id);
            keyValues.Add("@Lead", c.Lead == null ? Guid.Empty : c.Lead.Id);
            keyValues.Add("@FullName", c.Name);
            keyValues.Add("@ShortName", c.ShortName);
            keyValues.Add("@NTN", c.NTN);
            keyValues.Add("@STRN", c.STRN);
            keyValues.Add("@Address", c.Address);
            keyValues.Add("@InvoiceAddress", c.InvoiceAddress);
            keyValues.Add("@Email", c.Email);
            keyValues.Add("@Phone", c.Phone);
            keyValues.Add("@ContactPerson", c.ContactPerson);
            keyValues.Add("@HeadOffice", c.HeadOffice);
            keyValues.Add("@Remarks", c.Remarks);

            return keyValues;
        }

        public static Dictionary<string, object> reMapCustDest(Guid customerId,string Address)
        {
            Dictionary<string, object> keyValues = new Dictionary<string, object>();
            keyValues.Add("@CustomerId", customerId);
            keyValues.Add("@Status", 1);
            keyValues.Add("@Name", Address);

            return keyValues;
        }
        public static List<Customer> MapCustomerDataTable(DataTable dtCust, DataTable dtCustStock, DataTable dtCustDest)
        {
            List<Customer> AllCustomers = new List<Customer>();
            foreach (DataRow drCust in dtCust.Rows)
            {
                Customer Cust = new Customer();

                Cust.Id = drCust["Id"] != null ? new Guid(drCust["Id"].ToString()) : Guid.Empty;
                Cust.Status = drCust["Status"] != null ? CustStatus.Active : CustStatus.InActive;
                Cust.CreatedOn = drCust["CreatedOn"] != DBNull.Value ? DateTime.Parse(drCust["CreatedOn"].ToString()) : DateTime.MinValue;
                Cust.CreatedBy = drCust["CreatedBy"] != null ? UserManager.GetUserRef(drCust["CreatedBy"].ToString()) : UserManager.GetDefaultRef();
                Cust.ModifiedOn = drCust["ModifiedOn"] != DBNull.Value ? DateTime.Parse(drCust["ModifiedOn"].ToString()) : DateTime.MinValue;
                Cust.ModifiedBy = drCust["ModifiedBy"] != null ? UserManager.GetUserRef(drCust["ModifiedBy"].ToString()) : UserManager.GetDefaultRef();
                Cust.Lead = drCust["Lead"] != null ? UserManager.GetUserRef(drCust["Lead"].ToString()) : UserManager.GetDefaultRef();
                Cust.Name = drCust["FullName"] != null ? drCust["FullName"].ToString() : "";
                Cust.ShortName = drCust["ShortName"] != null ? drCust["ShortName"].ToString() : "";
                Cust.NTN = drCust["NTN"] != null ? drCust["NTN"].ToString() : "";
                Cust.STRN = drCust["STRN"] != null ? drCust["STRN"].ToString() : "";
                Cust.Address = drCust["Address"] != null ? drCust["Address"].ToString() : "";
                Cust.InvoiceAddress = drCust["InvoiceAddress"] != null ? drCust["InvoiceAddress"].ToString() : "";
                Cust.Email = drCust["Email"] != null ? drCust["Email"].ToString() : "";
                Cust.Phone = drCust["Phone"] != null ? drCust["Phone"].ToString() : "";
                Cust.ContactPerson = drCust["ContactPerson"] != null ? drCust["ContactPerson"].ToString() : "";
                Cust.HeadOffice = drCust["HeadOffice"] != null ? drCust["HeadOffice"].ToString() : "";
                Cust.Remarks = drCust["Remarks"] != null ? drCust["Remarks"].ToString() : "";
                Cust.Stock = new List<CustomerStock>();
                foreach (DataRow drCustStock in dtCustStock.Rows)
                {
                    Guid custId = drCustStock["CustomerId"] != null ? new Guid(drCustStock["CustomerId"].ToString()) : Guid.Empty;
                    if (custId != Guid.Empty && custId == Cust.Id)
                    {
                        CustomerStock CustStock = new CustomerStock();
                        CustStock.Customer = new Reference() { Id = Cust.Id, Name = Cust.Name };
                        CustStock.Store = drCustStock["StoreId"] != null ? new Reference() { Id = new Guid(drCustStock["StoreId"].ToString()) } : StoreManager.GetDefaultRef();
                        CustStock.Vessel = drCustStock["Vessel"] != null ? CommonDataManager.GetVessel(drCustStock["Vessel"].ToString()) : CommonDataManager.GetDefaultRef();
                        CustStock.Origin = drCustStock["Origin"] != null ? CommonDataManager.GetOrigin(drCustStock["Origin"].ToString()) : CommonDataManager.GetDefaultRef();
                        CustStock.Size = drCustStock["Size"] != null ? CommonDataManager.GetSize(drCustStock["Size"].ToString()) : CommonDataManager.GetDefaultRef();
                        CustStock.Quantity = drCustStock["Quantity"] != null ? decimal.Parse(drCustStock["Quantity"].ToString()) : 0;
                        Cust.Stock.Add(CustStock);
                    }

                }
                Cust.Destination = new List<Item>();
                foreach (DataRow drCustDest in dtCustDest.Rows)
                {
                    Guid custId = drCustDest["CustomerId"] != null ? new Guid(drCustDest["CustomerId"].ToString()) : Guid.Empty;
                    if (custId != Guid.Empty && custId == Cust.Id)
                    {
                        Item CustDest = new Item() { Index = int.Parse(drCustDest["id"].ToString()), Value = drCustDest["Name"].ToString() };
                        Cust.Destination.Add(CustDest);
                    }

                }
                AllCustomers.Add(Cust);
            }
            return AllCustomers;
        }

    }
}
