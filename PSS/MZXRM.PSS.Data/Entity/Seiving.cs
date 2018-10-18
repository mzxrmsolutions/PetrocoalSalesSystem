using System;
using System.Collections.Generic;

namespace MZXRM.PSS.Data
{
    public class Seiving
    {
        
        public int ID { get; set; }

         public string SeivingNo { get; set; }

        public DateTime Date { get; set; }

        public string StoreId { get; set; }

        public int VesselId { get; set; }

       public string CoalDescription { get; set; }

        public string FromSize { get; set; }

         public decimal FromQuantity { get; set; }

        public List<SeivingSizeQty> seivingSizeQty { get; set; }

       
        public string Remarks { get; set; }
    }
}