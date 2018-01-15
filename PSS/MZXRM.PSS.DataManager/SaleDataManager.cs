using MZXRM.PSS.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MZXRM.PSS.DataManager
{

    public class SaleDataManager
    {
        static string _dataPath = ConfigurationManager.AppSettings["DataPath"];
        public static List<SaleOrder> ReadAllSO()
        {
            List<SaleOrder> allData = new List<SaleOrder>();
            //if (HttpContext.Current.Session["DBSaleOrders"] == null)
            {
                string soPath = _dataPath + "/SO";

                if (!Directory.Exists(soPath))
                    Directory.CreateDirectory(soPath);
                string[] files = Directory.GetFiles(soPath);
                foreach (string file in files)
                {
                    SaleOrder so = XMLUtil.ReadFromXmlFile<SaleOrder>(file);
                    allData.Add(so);
                }
                //HttpContext.Current.Session.Add("DBSaleOrders", allData);
            }
            //else
            //{
            //    allData = HttpContext.Current.Session["DBSaleOrders"] as List<SaleOrder>;
            //}
            return allData;
        }
    }
}
