﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MZXRM.PSS.Data
{
    public class PurchaseOrder
    {
        public Guid Id;
        public POStatus Status;
        public DateTime CreatedOn;
        public Reference CreatedBy;
        public DateTime ModifiedOn;
        public Reference ModifiedBy;

        public DateTime CompletedOn;

        public Reference Lead;
        public Reference ApprovedBy;
        public DateTime ApprovedDate;

        public string PONumber;
        public DateTime PODate;

        public Item Origin;
        public Item Size;
        public Item Vessel;

        public int TargetDays;
        public Item Supplier;
        public string TermsOfPayment;

        public decimal BufferQuantityMax;
        public decimal BufferQuantityMin;

        public string Remarks;

        public List<PODetail> PODetailsList;
        public decimal TotalQuantity;

        public decimal Available;
        public decimal Received;
        
        public decimal DutyCleared;

        public bool isValid;
        public bool isPSL=true;
    }
    public enum POStatus
    {
        Created,//1
        PendingApproval,//2
        InProcess,//3
        Completed,//4
        Cancelled//0
    }
}