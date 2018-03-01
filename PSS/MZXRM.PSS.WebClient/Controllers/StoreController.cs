using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MZXRM.PSS.WebClient.Controllers
{
    public class StoreController : Controller
    {
        // GET: Store
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult StoreInOut()
        {
            return View();
        }
        public ActionResult Seiving()
        {
            return View();
        }
    }
}