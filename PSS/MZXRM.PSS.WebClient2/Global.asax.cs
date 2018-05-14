using MZXRM.PSS.Business;
using MZXRM.PSS.Data;
using MZXRM.PSS.DataManager;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace MZXRM.PSS.WebClient
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        protected void Session_Start()
        {
            List<Item> Origin = Business.Common.Origin;
            List<Item> Size = Business.Common.Size;
            List<Item> Vessel = Business.Common.Vessel;
            List<Item> Supplier = Business.Common.Supplier;
            List<Item> TaxRate = Business.Common.TaxRate;
            List<Item> Trader = Business.Common.Trader;
            List<Item> Transporter = Business.Common.Transporter;
            List<Reference> AllSaleStations = Business.Common.AllSaleStations;

            List<Customer> Customer = CustomerDataManager.ReadCustomers();
            List<Store> Store = StoreManager.AllStore;
            List<PurchaseOrder> PO = PurchaseManager.AllPOs;
            List<SaleOrder> SO = SaleManager.AllSOs;
            Customer = CustomerManager.AllCustomers;



        }
    }
}
