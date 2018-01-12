using System;

namespace MZXRM.PSS.Data
{
    public class DutyClear
    {
        public Guid Id;
        public DateTime CreatedOn;
        public Reference CreatedBy;
        public DateTime ModifiedOn;
        public Reference ModifiedBy;

        public string DCLNumber;
        public DateTime DCLDate;
        public Reference PO;
        public Reference PODetail;

        public Reference Store;
        public decimal Quantity;
        public string Remarks;
    }
}