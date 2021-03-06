﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MZXRM.PSS.Data
{
    public class SaleOrder
    {
        public int Id;
        public SOStatus Status;
        public SOType OrderType;
        public DateTime CreatedOn;
        public Reference CreatedBy;
        public DateTime ModifiedOn;
        public Reference ModifiedBy;

        public DateTime CompletedOn;

        public Reference Lead;
        public Reference ApprovedBy;
        public DateTime ApprovedDate;
        public decimal BufferQuantityMax;
        public decimal BufferQuantityMin;

        public string SONumber;
        public DateTime SODate;
        public DateTime SOExpiry;

        public Reference Customer;

        public string PartyPONumber;
        public DateTime PODate;
        public DateTime POExpiry;

        public int CreditPeriod;

        public Item Origin;
        public Item Size;
        public Item Vessel;

        public decimal Quantity;
        public bool LC; // LC false = commercial
        public bool Tax; // false=customer

        public decimal? AgreedRate;
        public Item AgreedTaxRate;

        public decimal TaxAmount;
        public decimal RateIncTax;
        public decimal RateExcTax;

        public decimal FinalPrice;

        public Item Trader;
        public decimal TraderCommission;

        public Item SaleStation;
        public string Remarks;

        public string PartyPOImage;
        public List<DeliveryOrder> DOList { get; set; }

        //QUANTITY RELATED WORK GOES HERE
        public decimal DeliveredQuantity { get; set; }
        public decimal RemainingQuantity { get; set; }
    }
    public enum SOStatus
    {
        Created=1,
        PendingApproval=2,
        InProcess=3,
        Completed=4,
        Cancelled=5
    }
   public enum SOType
    {
        Commercial=1,
        LC=2,
        Loan=3
    }

}


