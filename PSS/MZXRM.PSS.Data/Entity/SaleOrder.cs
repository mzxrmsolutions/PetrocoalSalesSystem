using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MZXRM.PSS.Data
{
    public class SaleOrder
    {
        public Guid Id;
        public SOStatus Status;
        public DateTime CreatedOn;
        public Reference CreatedBy;
        public DateTime ModifiedOn;
        public Reference ModifiedBy;

        public DateTime CompletedOn;

        public Reference Lead;
        public Reference ApprovedBy;
        public DateTime ApprovedDate;

        public string SONumber;
        public DateTime SODate;
        public DateTime SOExpiry;

        public Reference Customer;

        public string PartyPONumber;
        public DateTime PODate;
        public DateTime POExpiry;

        public string CreditPeriod;

        public Item Origin;
        public Item Size;
        public Item Vessel;

        public decimal Quantity;
        public bool LC; // LC false = commercial
        public bool Tax; // false=customer

        public decimal AgreedRate;
        public decimal AgreedTaxRate;

        public decimal TaxAmount;
        public decimal RateIncTax;
        public decimal RateExcTax;

        public decimal FinalPrice;

        public Item Trader;
        public decimal TraderCommission;

        public Item SaleStation;
        public string Remarks;
    }
    public enum SOStatus
    {
        Created,
        PendingApproval,
        InProcess,
        Completed,
        Cancelled
    }
}