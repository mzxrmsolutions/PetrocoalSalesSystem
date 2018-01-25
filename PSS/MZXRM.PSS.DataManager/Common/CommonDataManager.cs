using MZXRM.PSS.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net.Http;
using System.Web;

namespace MZXRM.PSS.DataManager
{
    public class CommonDataManager
    {
        static string _dataPath = ConfigurationManager.AppSettings["DataPath"];
        

        public static List<Item> GetOriginList()
        {
            string fileName = _dataPath + "/Lists/Origin.xml";
            List<Item> list = XMLUtil.ReadFromXmlFile<List<Item>>(fileName);
            return list;
        }
        public static Item GetOrigin(string id)
        {
            List<Item> list = GetOriginList();
            foreach (Item item in list)
            {
                if (item.Index.ToString() == id)
                    return item;
            }
            return GetDefaultRef();
        }
        public static Item SaveOrigin(Item data)
        {
            // TODO
            return GetDefaultRef();
        }
        public static Item GetSize(string id)
        {
            List<Item> list = GetSizeList();
            foreach (Item item in list)
            {
                if (item.Index.ToString() == id)
                    return item;
            }
            return GetDefaultRef();
        }
        public static Item GetVessel(string id)
        {
            List<Item> list = GetVesselList();
            foreach (Item item in list)
            {
                if (item.Index.ToString() == id)
                    return item;
            }
            return GetDefaultRef();
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

        public static Item GetSupplier(string id)
        {
            List<Item> list = GetSupplierList();
            foreach (Item item in list)
            {
                if (item.Index.ToString() == id)
                    return item;
            }
            return GetDefaultRef();
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

        internal static Item GetDefaultRef()
        {
            return new Item() { Index = 0, Value = "" };
        }

        public static void SaveStoreList(List<Reference> data)
        {
            string fileName = _dataPath + "/Lists/Store.xml";
            XMLUtil.WriteToXmlFile<List<Reference>>(fileName, data);
        }
       
       
        
    }
}