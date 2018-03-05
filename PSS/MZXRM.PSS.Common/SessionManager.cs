using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MZXRM.PSS.Common
{
   public class SessionManager
    {
       public static string OriginSessionName = "AllOrigin";
        public static string SizeSessionName = "AllSize";
        public static string VesselSessionName = "AllVessel";
        public static string TraderSessionName = "AllTrader";
        public static string TaxRateSessionName = "AllTaxRate";
       public static string POSession = "AllPurchaseOrders";
        public static string SOSession = "AllSaleOrders";
        public static string CustomerSession = "AllCustomers";
        public static string StoreSession = "AllStores";
        public static string StoreIOSession = "AllStoreIOs";
        public static void ClearSession()
        {
            HttpContext.Current.Session.Clear();
            HttpContext.Current.Session.Abandon();
        }

    }
}
