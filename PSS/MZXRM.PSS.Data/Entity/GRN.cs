using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MZXRM.PSS.Data
{
    public class GRN
    {
        public Guid Id;
        public GRNStatus Status;
        public DateTime CreatedOn;
        public Reference CreatedBy;
        public DateTime ModifiedOn;
        public Reference ModifiedBy;
        public DateTime CompletedOn;

        public string GRNNumber;
        public DateTime GRNDate;
        public Reference PO;
        public Reference PODetail;

        public Reference Store;
        public string InvoiceNo;
        public decimal AdjPrice;
        public decimal Quantity;
        public string Remarks;
    }
    public enum GRNStatus
    {
        PendingRecieve,//2
        Recieved,//1
        Loan,//3
        Cancelled//0
    }
}