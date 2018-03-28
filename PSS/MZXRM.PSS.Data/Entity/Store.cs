using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MZXRM.PSS.Data
{
  public  class Store
    {
        public Guid Id;
        public StoreStatus Status;
        public DateTime CreatedOn;
        public Reference CreatedBy;
        public DateTime ModifiedOn;
        public Reference ModifiedBy;

        public Reference StoreManager;

        public string Name;
        public string Location;
        //public string SaleStationId;
        //public string SubType;

        public decimal Capacity;
        
        public List<CustomerStock> Stock;
        public decimal TotalStock;
        public List<StockMovement> ListStockMovement;

    }
    public enum StoreStatus
    {
        Active=1,
        InActive=0
    }
}
