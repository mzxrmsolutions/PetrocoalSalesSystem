using System;

namespace MZXRM.PSS.Data
{
    public class DeliveryChalan
    {
        public int Id { get; set; }
        public Item DeliveryOrder { get; set; }
        public Reference Lead { get; set; }
        public Item Transporter { get; set; }
        public DCStatus Status { get; set; }
        public String DCNumber { get; set; }
        public DateTime DCDate { get; set; }
        public decimal Quantity { get; set; }
        public String TruckNo { get; set; }
        public String BiltyNo { get; set; }
        public String SlipNo { get; set; }
        public Decimal Weight { get; set; }
        public Decimal NetWeight { get; set; }
        public String DriverName{ get; set; }
        public String DriverPhone { get; set; }
        public DateTime CreatedOn { get; set; }
        public Reference CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public Reference ModifiedBy { get; set; }
        public String Remarks { get; set; }
    }

    public enum DCStatus
    {
        InTransit = 0,//0
        Completed = 1,//1
        Rejected = 2,//2
    }
}
