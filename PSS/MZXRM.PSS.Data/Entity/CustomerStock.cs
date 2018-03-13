using System;

namespace MZXRM.PSS.Data
{
    public class CustomerStock
    {

        public Reference Customer;
        public Reference Store;

        public Item Vessel;
        public Item Origin;
        public Item Size;
        public decimal Quantity;
        public bool StockMinus;
    }
}