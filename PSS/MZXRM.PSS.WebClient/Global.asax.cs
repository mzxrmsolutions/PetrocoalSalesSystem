using MZXRM.PSS.Business;
using MZXRM.PSS.Data;
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
            List<Customer> Customer = CustomerManager.AllCustomers;
            List<Store> Store = StoreManager.AllStore;
            List<PurchaseOrder> PO = PurchaseManager.AllPOs;
            List<SaleOrder> SO = SaleManager.AllSOs;


        }
    }
}
