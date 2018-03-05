using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MZXRM.PSS.Data
{
  public  class StoreInOut
    {
        public int Id;
        public StoreMovementType InOut;
        public StoreInOutStatus Status;
        public DateTime CreatedOn;
        public Reference CreatedBy;
        public DateTime ModifiedOn;
        public Reference ModifiedBy;
        public DateTime CompletedOn;
        public Reference LeadId;
        public string SMNumber;
        public DateTime SMDate;
        public Item Origin;
        public Item Size;
        public int Vessel;
        public decimal Quantity;
        public Reference FromStoreId;
        public Reference ToStoreId;
        public string VehicleNo;
        public string BiltyNo;
        public DateTime BiltyDate;
        public string RRInvoice;
        public string CCMNumber;
        public Item Transporter;
        public DateTime StoreInDate;
        public decimal StoreInQuantity;
        public Reference StoreMovementId;
    }
    public enum StoreInOutStatus
    {
        Active=1,
        InActive=0
    }
    public enum StoreMovementType
    {
        Active = 1,
        InActive = 0
    }
}
