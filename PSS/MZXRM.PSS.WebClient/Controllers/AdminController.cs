using PatrocoalSalesSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PatrocoalSalesSystem.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Vessel()
        {
            Common.MyUrl = Request.RawUrl;
            if (!Common.isAuthorize())
                Response.Redirect("/Login");
            List<Item> data = Common.Vessel;
            return View(data);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Vessel(FormCollection form)
        {
            string[] indexes = form["index"].Split(new char[] { ',' });
            string[] values = form["value"].Split(new char[] { ',' });
            List<Item> data = new List<Item>();
            for (int i = 0; i < indexes.Length; i++)
            {
                if (values[i] != "")
                {
                    int index = indexes[i] != "" ? int.Parse(indexes[i]) : 0;
                    data.Add(new Item() { Index = index, Value = values[i] });
                }
            }
            Common.Vessel = data;
            return View(data);
        }
        public ActionResult Store()
        {
            Common.MyUrl = Request.RawUrl;
            if (!Common.isAuthorize())
                Response.Redirect("/Login");
            List<Reference> data = Common.Store;
            return View(data);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Store(FormCollection form)
        {
            string[] indexes = form["index"].Split(new char[] { ',' });
            string[] values = form["value"].Split(new char[] { ',' });
            List<Reference> data = new List<Reference>();
            for (int i = 0; i < indexes.Length; i++)
            {
                if (values[i] != "")
                {
                    Guid index = indexes[i] != "" ? new Guid(indexes[i]) : Guid.NewGuid();
                    data.Add(new Reference() { Id = index, Name = values[i] });
                }
            }
            Common.Store = data;
            return View(data);
        }
    }
}