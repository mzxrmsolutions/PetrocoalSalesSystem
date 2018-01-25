using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MZXRM.PSS.Data
{
    public class PODetail
    {
        public Guid Id;

        public Reference PO;
        public Reference Customer;
        
        public decimal Quantity;
        public decimal Rate;
        public decimal CostPerTon;
        public decimal AllowedWaistage;
        public DateTime TargetDate;
        public string Remarks;
        public decimal Received;
        public decimal Remaining;
        public decimal DutyCleared;
        public decimal DutyRemaining;
        public decimal Wastage;

        public List<GRN> GRNsList;
        public List<DutyClear> DutyClearsList;
    }
    
}