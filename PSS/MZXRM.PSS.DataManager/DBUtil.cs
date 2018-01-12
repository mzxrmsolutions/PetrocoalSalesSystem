using MZXRM.PSS.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net.Http;
using System.Web;

namespace MZXRM.PSS.DataManager
{
    public static class DBUtil
    {
        static string _dataPath = ConfigurationManager.AppSettings["DataPath"];
        public static List<PurchaseOrder> ReadAllPO()
        {
            List<PurchaseOrder> openPOs = new List<PurchaseOrder>();
            //if (HttpContent.Current.Session["DBPurchaseOrders"] == null)
            {
                string poPath = _dataPath + "/PO";
                string poPath2 = "./PO";

                if (!Directory.Exists(poPath))
                    Directory.CreateDirectory(poPath);
                string[] files = Directory.GetFiles(poPath);
                foreach (string file in files)
                {
                    PurchaseOrder po = XMLUtil.ReadFromXmlFile<PurchaseOrder>(file);
                    openPOs.Add(po);
                }
                //HttpContext.Current.Session.Add("DBPurchaseOrders", openPOs);
            }
            //else
            //{
            //    openPOs = HttpContext.Current.Session["DBPurchaseOrders"] as List<PurchaseOrder>;
            //}
            return openPOs;
        }

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

        public static List<Customer> GetAllCustomers()
        {
            string filePath = _dataPath + "/Customer";
            List<Customer> allCustomers = new List<Customer>();

            if (Directory.Exists(filePath))
            {
                string[] files = Directory.GetFiles(filePath);
                foreach (string file in files)
                {
                    Customer customer = XMLUtil.ReadFromXmlFile<Customer>(file);
                    allCustomers.Add(customer);
                }
            }
            else
                Directory.CreateDirectory(filePath);
            return allCustomers;
        }

        public static List<Item> GetOriginList()
        {
            string fileName = _dataPath + "/Lists/Origin.xml";
            List<Item> list = XMLUtil.ReadFromXmlFile<List<Item>>(fileName);
            return list;
        }
        public static List<Item> GetTaxRateList()
        {
            string fileName = _dataPath + "/Lists/TaxRate.xml";
            List<Item> list = XMLUtil.ReadFromXmlFile<List<Item>>(fileName);
            return list;
        }

        public static List<Item> GetVesselList()
        {
            string fileName = _dataPath + "/Lists/Vessel.xml";
            List<Item> list = XMLUtil.ReadFromXmlFile<List<Item>>(fileName);
            return list;
        }
        public static List<Item> GetTraderList()
        {
            string fileName = _dataPath + "/Lists/Trader.xml";
            List<Item> list = XMLUtil.ReadFromXmlFile<List<Item>>(fileName);
            return list;
        }

        public static List<Reference> GetStoreList()
        {
            string fileName = _dataPath + "/Lists/Store.xml";
            List<Reference> list = XMLUtil.ReadFromXmlFile<List<Reference>>(fileName);
            return list;
        }

        public static List<Item> GetSizeList()
        {
            string fileName = _dataPath + "/Lists/Size.xml";
            List<Item> list = XMLUtil.ReadFromXmlFile<List<Item>>(fileName);
            return list;
        }
        public static List<Item> GetSupplierList()
        {
            string fileName = _dataPath + "/Lists/Supplier.xml";
            List<Item> list = XMLUtil.ReadFromXmlFile<List<Item>>(fileName);
            return list;
        }
        public static void SaveVesselList(List<Item> data)
        {
            string fileName = _dataPath + "/Lists/Vessel.xml";
            XMLUtil.WriteToXmlFile<List<Item>>(fileName, data);
        }
        public static void SaveStoreList(List<Reference> data)
        {
            string fileName = _dataPath + "/Lists/Store.xml";
            XMLUtil.WriteToXmlFile<List<Reference>>(fileName, data);
        }
        public static bool SavePO(PurchaseOrder PO)
        {
            string poPath = _dataPath + "/PO";
            string fileName = poPath + "/" + PO.PONumber + ".xml";
            XMLUtil.WriteToXmlFile<PurchaseOrder>(fileName, PO);
            HttpContext.Current.Session.Remove("DBPurchaseOrders");
            ReadAllPO();
            return true;
        }
        public static bool SaveCustomer(Customer cust)
        {
            string poPath = _dataPath + "/Customer";
            string fileName = poPath + "/" + cust.Name + ".xml";
            XMLUtil.WriteToXmlFile<Customer>(fileName, cust);
            return true;
        }
        public static bool SaveUser(User user)
        {
            string poPath = _dataPath + "/User";
            string fileName = poPath + "/" + user.Name + ".xml";
            XMLUtil.WriteToXmlFile<User>(fileName, user);
            return true;
        }
        public static List<User> GetAllUsers()
        {
            string filePath = _dataPath + "/User";
            List<User> users = new List<User>();

            if (Directory.Exists(filePath))
            {
                string[] files = Directory.GetFiles(filePath);
                foreach (string file in files)
                {
                    User user = XMLUtil.ReadFromXmlFile<User>(file);
                    users.Add(user);
                }
            }
            else
                Directory.CreateDirectory(filePath);
            return users;
        }
    }
}