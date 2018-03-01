using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MZXRM.PSS.Data
{
    public class Customer
    {
        public Guid Id;
        public CustStatus Status;
        public DateTime CreatedOn;
        public Reference CreatedBy;
        public DateTime ModifiedOn;
        public Reference ModifiedBy;

        public Reference Lead;

        public string Name;
        public string ShortName;
        public string NTN;
        public string STRN;
        public string Address;
        public string InvoiceAddress;
        public string Email;
        public string Phone;
        public string ContactPerson;
        public string HeadOffice;
        public string Remarks;

        public List<CustomerStock> Stock;
        public decimal TotalStock;
        public bool StockMinus;
    }
    public enum CustStatus
    {
       Active=1,
       InActive=0
    }
}