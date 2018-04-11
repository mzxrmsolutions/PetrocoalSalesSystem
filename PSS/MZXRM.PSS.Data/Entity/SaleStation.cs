using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MZXRM.PSS.Data
{
    public class SaleStation
    {
        public Guid Id { get; set; }
        public SaleStationStatus Status { get; set; }
        public DateTime CreatedOn { get; set; }
        public Reference CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public Reference ModifiedBy { get; set; }
        public String Name { get; set; }
        public Reference Organization { get; set; }
    }
    public enum SaleStationStatus
    {
        Active = 1,
        InActive = 0
    }
}
