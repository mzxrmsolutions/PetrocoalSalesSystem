using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MZXRM.PSS.Data
{
    public class Team
    {
        public Guid Id { get; set; }
        public String Name { get; set; }
        public Guid SaleStationId { get; set; }
        public TeamStatus TeamStatus { get; set; }

        public DateTime CreatedOn { get; set; }
        public Reference CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public Reference ModifiedBy { get; set; }
    }
    //temporary, must be declared in common file by the name of ActiveStatus
    public enum TeamStatus
    {
        InActive = 0,
        Active = 1
    }
}
