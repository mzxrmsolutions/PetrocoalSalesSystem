﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PatrocoalSalesSystem.Models
{
    public static class Common
    {
        private static List<Item> _lOrigin;
        private static List<Item> _lSize;
        private static List<Item> _lVessel;
        private static List<Item> _lSupplier;
        private static List<Item> _lTaxRate;
        private static List<Item> _lTrader;
        private static List<Reference> _lStore;

        internal static void ReloadData()
        {
            PurchaseUtil.AllPOs = DBUtil.ReadAllPO();
            UserUtil.Users = DBUtil.GetAllUsers();
            //CurrentUser = null;
        }

        public static List<Role> AllRole;
        public static List<Item> Origin
        {
            get
            {
                if (_lOrigin == null || _lOrigin.Count == 0)
                    _lOrigin = DBUtil.GetOriginList();
                return _lOrigin;
            }
            set
            {
                _lOrigin = value;
            }
        }
        public static List<Item> TaxRate
        {
            get
            {
                if (_lTaxRate == null || _lTaxRate.Count == 0)
                    _lTaxRate = DBUtil.GetTaxRateList();
                return _lTaxRate;
            }
            set
            {
                _lTaxRate = value;
            }
        }
        public static List<Item> Trader
        {
            get
            {
                if (_lTrader == null || _lTrader.Count == 0)
                    _lTrader = DBUtil.GetTraderList();
                return _lTrader;
            }
            set
            {
                _lTrader = value;
            }
        }
        public static List<Item> Size
        {
            get
            {
                if (_lSize == null || _lSize.Count == 0)
                    _lSize = DBUtil.GetSizeList();
                return _lSize;
            }
            set
            {
                _lSize = value;
            }
        }
        public static List<Item> Vessel
        {
            get
            {
                if (_lVessel == null || _lVessel.Count == 0)
                    _lVessel = DBUtil.GetVesselList();
                return _lVessel;
            }
            set
            {
                _lVessel = value;
                DBUtil.SaveVesselList(_lVessel);
            }
        }

        internal static int GenerateIndex(List<Item> items)
        {
            throw new NotImplementedException();
        }

        public static List<Item> Supplier
        {
            get
            {
                if (_lSupplier == null || _lSupplier.Count == 0)
                    _lSupplier = DBUtil.GetSupplierList();
                return _lSupplier;
            }
            set
            {
                _lSupplier = value;
            }
        }
        public static List<Reference> Store
        {
            get
            {
                if (_lStore == null || _lStore.Count == 0)
                    _lStore = DBUtil.GetStoreList();
                return _lStore;
            }
            set
            {
                _lStore = value;
                DBUtil.SaveStoreList(_lStore);
            }
        }

        internal static Item GetSize(string id)
        {
            foreach (Item size in Size)
                if (size.Index.ToString() == id)
                    return size;
            return new Item() { Index = 0, Value = "" };
        }

        internal static Item GetVessel(string id)
        {
            foreach (Item vessel in Vessel)
                if (vessel.Index.ToString() == id)
                    return vessel;
            return new Item() { Index = 0, Value = "" };
        }

        internal static Item GetSupplier(string id)
        {
            foreach (Item supplier in Supplier)
                if (supplier.Index.ToString() == id)
                    return supplier;
            return new Item() { Index = 0, Value = "" };
        }
        internal static Item GetOrigin(string id)
        {
            foreach (Item origin in Origin)
                if (origin.Index.ToString() == id)
                    return origin;
            return new Item() { Index = 0, Value = "" };
        }
        internal static Reference GetStore(string id)
        {
            foreach (Reference origin in Store)
                if (origin.Id == new Guid(id))
                    return origin;
            return new Reference() { Id = Guid.Empty, Name = "" };
        }
        //public static List<Item> Origin;
        //public static List<Item> Origin;
        //public static List<Item> Origin;
        //public static List<Item> Origin;
        //public static List<Item> Origin;
        public static string MyUrl = "/home";
        public static User CurrentUser;

        internal static void LoadTempDataForTesting()
        {
            //GetDemoUsers();
            //PurchaseUtil.GetDemoPO();

            //lOrigin = DBUtil.GetOriginList();
            //lSize = DBUtil.GetSizeList();
            //lVessel = DBUtil.GetVesselList();
        }



        public static void LoadUserDetails()
        {

        }

        internal static bool isAuthorize()
        {
            /* if (CurrentUser == null)
             {
                 return false;
             }
             return true;*/
            return !(CurrentUser == null);
        }


        private static void GetDemoRoles()
        {
            AllRole = new List<Role>() {
                new Role() {
                    Id=Guid.NewGuid(),
                    Name="Admin"
                }
            };
        }
        private static void GetDemoUsers()
        {
            User user = new User()
            {
                Id = Guid.NewGuid(),
                Status = UserStatus.Active,
                CreatedOn = new DateTime(2017, 12, 1),
                ModifiedOn = new DateTime(2017, 12, 1),
                Name = "Faisal Nawaz",
                Login = "admin",
                Password = "admin"
                //Role = AllRole.FirstOrDefault().Ref()
            };
            XMLUtil.WriteToXmlFile("E:/MZ XRM Solutions/Patrocol/Patrocoal/PatrocoalSalesSystem/App_Data/User/" + user.Name + ".xml", user);

        }



        public static bool SavePO(PurchaseOrder po)
        {
            return true;
        }
    }
}