using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MZXRM.PSS.Data
{
   public class StockMovement
    {
        public Guid Id;
        public Reference Store;
        public Reference Customer;
        public StMovType Type;
        public string HistoryRef;
        public DateTime Date;
        public decimal Quantity;

        public bool IsIn;

        public Item Vessel;
        public Item Origin;
        public Item Size;
        public string Remarks;
       
    }
    public enum StMovType
    {
        GRNClear=1,
        DCSuccess=2,
        StoreMovement=3
    }
}
