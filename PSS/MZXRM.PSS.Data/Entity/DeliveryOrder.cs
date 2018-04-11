using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MZXRM.PSS.Data
{
    public class DeliveryOrder
    {
        public int Id { get; set; }
        public Reference Location { get; set; }
        public DOStatus Status { get; set; }
        public Item SaleOrder { get; set; }
        public DateTime CreatedOn { get; set; }
        public Reference CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public Reference ModifiedBy { get; set; }
        public DateTime CompletedOn { get; set; }
        public Reference Lead { get; set; }
        public Reference ApprovedBy { get; set; }
        public DateTime ApprovedDate { get; set; }
        public String DONumber { get; set; }
        public DateTime DODate { get; set; }
        public decimal Quantity { get; set; }
        public DateTime LiftingStartDate { get; set; }
        public DateTime LiftingEndDate { get; set; }
        public Item DeliveryDestination { get; set; }
        public Item Transportor { get; set; }
        public decimal DumperRate { get; set; }
        public decimal FreightPaymentTerms { get; set; }
        public decimal FreightPerTon { get; set; }
        public decimal FreightTaxPerTon { get; set; }
        public decimal FreightComissionPSL { get; set; }
        public decimal FreightComissionAgent { get; set; }
        public String Remarks { get; set; }
        public List<DeliveryChalan> DCList { get; set; }

        //QUANTITY RELATED WORK GOES HERE
        public decimal DeliveredQuantity { get; set; }
        public decimal RemainingQuantity { get; set; }
    }
    public enum DOStatus
    {
        Cancelled = 0,//0
        Created = 1,//1
        PendingApproval,//2
        InProcess = 3,//3
        Completed = 4,//4
        LoadingStop = 5
    }
}
