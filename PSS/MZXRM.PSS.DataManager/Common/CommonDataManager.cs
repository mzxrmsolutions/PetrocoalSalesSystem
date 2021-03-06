﻿using MZXRM.PSS.Common;
using MZXRM.PSS.Connector.Database;
using MZXRM.PSS.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Net.Http;
using System.Web;

namespace MZXRM.PSS.DataManager
{
    public class CommonDataManager
    {
        static string _dataPath = ConfigurationManager.AppSettings["DataPath"];

        #region Origin
        public static List<Item> GetOriginList()
        {
            string session = SessionManager.OriginSessionName;
            if (HttpContext.Current.Session[session] == null || ((List<Item>)HttpContext.Current.Session[session]).Count == 0)
            {
                try
                {
                    using (var dbc = DataFactory.GetConnection())
                    {

                        IDbCommand command = CommandBuilder.CommandGetAll(dbc, "sp_GetAllOrigin");

                        if (command.Connection.State != ConnectionState.Open)
                        {
                            command.Connection.Open();
                        }

                        IDataReader datareader = command.ExecuteReader();

                        List<Item> returnList = new List<Item>();
                        while (datareader.Read())
                        {
                            Item item = new Item();
                            item.Index = (int)datareader["id"];
                            item.Value = datareader["name"].ToString();
                            returnList.Add(item);
                            item = null;
                        }
                        HttpContext.Current.Session.Add(SessionManager.OriginSessionName, returnList);
                        return returnList;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error! Get all Origin from DataBase", ex);
                }
            }
            List<Item> Allresults = HttpContext.Current.Session[SessionManager.OriginSessionName] as List<Item>;
            return Allresults;

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
        #endregion

        #region Size
        public static List<Item> GetSizeList()
        {
            string session = SessionManager.SizeSessionName;
            if (HttpContext.Current.Session[session] == null || ((List<Item>)HttpContext.Current.Session[session]).Count == 0)
            {
                try
                {
                    using (var dbc = DataFactory.GetConnection())
                    {

                        IDbCommand command = CommandBuilder.CommandGetAll(dbc, "sp_GetAllSize");

                        if (command.Connection.State != ConnectionState.Open)
                        {
                            command.Connection.Open();
                        }

                        IDataReader datareader = command.ExecuteReader();

                        List<Item> returnList = new List<Item>();
                        while (datareader.Read())
                        {
                            Item item = new Item();
                            item.Index = (int)datareader["id"];
                            item.Value = datareader["name"].ToString();
                            returnList.Add(item);
                            item = null;
                        }
                        HttpContext.Current.Session.Add(SessionManager.SizeSessionName, returnList);
                        return returnList;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error! Get all Size from DataBase", ex);
                }
            }
            List<Item> Allresults = HttpContext.Current.Session[SessionManager.SizeSessionName] as List<Item>;
            return Allresults;

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
        #endregion

        #region Vessel
        public static List<Item> GetVesselList()
        {
            string session = SessionManager.VesselSessionName;
            if (HttpContext.Current.Session[session] == null || ((List<Item>)HttpContext.Current.Session[session]).Count == 0)
            {
                try
                {
                    using (var dbc = DataFactory.GetConnection())
                    {

                        IDbCommand command = CommandBuilder.CommandGetAll(dbc, "sp_GetAllVessel");

                        if (command.Connection.State != ConnectionState.Open)
                        {
                            command.Connection.Open();
                        }

                        IDataReader datareader = command.ExecuteReader();

                        List<Item> returnList = new List<Item>();
                        while (datareader.Read())
                        {
                            Item item = new Item();
                            item.Index = (int)datareader["id"];
                            item.Value = datareader["name"].ToString();
                            returnList.Add(item);
                            item = null;
                        }
                        HttpContext.Current.Session.Add(SessionManager.VesselSessionName, returnList);
                        return returnList;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error! Get all Vessel from DataBase", ex);
                }
            }
            List<Item> Allresults = HttpContext.Current.Session[SessionManager.VesselSessionName] as List<Item>;
            return Allresults;

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



        #endregion

        #region " Sale Station Region "
        public static List<Reference> GetSaleStationList()
        {
            string session = SessionManager.SaleStationSession;
            if (HttpContext.Current.Session[session] == null || ((List<Reference>)HttpContext.Current.Session[session]).Count == 0)

            {
                try
                {
                    using (var dbc = DataFactory.GetConnection())
                    {

                        IDbCommand command = CommandBuilder.CommandGetAll(dbc, "sp_GetAllSaleStation");

                        if (command.Connection.State != ConnectionState.Open)
                        {
                            command.Connection.Open();
                        }

                        IDataReader datareader = command.ExecuteReader();

                        List<Reference> returnList = new List<Reference>();
                        while (datareader.Read())
                        {
                            Reference item = new Reference();
                            item.Id = (Guid)datareader["Id"];
                            item.Name = datareader["Name"].ToString();
                            returnList.Add(item);
                            //item = null;
                        }
                        HttpContext.Current.Session.Add(SessionManager.SaleStationSession, returnList);
                        return returnList;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error! Get all Sale station from DataBase", ex);
                }
            }
            List<Reference> Allresults = HttpContext.Current.Session[SessionManager.SaleStationSession] as List<Reference>;
            return Allresults;

        }
        public static Reference GetSaleStation(string id)
        {
            List<Reference> list = GetSaleStationList();
            foreach (Reference item in list)
            {
                if (item.Id.ToString() == id)
                    return item;
            }
            return GetDefaultReference();
        }

        #endregion

        #region TaxRate
        public static List<Item> GetTaxRateList()
        {
            string session = SessionManager.TaxRateSessionName;
            if (HttpContext.Current.Session[session] == null || ((List<Item>)HttpContext.Current.Session[session]).Count == 0)
            {
                try
                {
                    using (var dbc = DataFactory.GetConnection())
                    {

                        IDbCommand command = CommandBuilder.CommandGetAll(dbc, "sp_GetAllTaxRate");

                        if (command.Connection.State != ConnectionState.Open)
                        {
                            command.Connection.Open();
                        }

                        IDataReader datareader = command.ExecuteReader();

                        List<Item> returnList = new List<Item>();
                        while (datareader.Read())
                        {
                            Item item = new Item();
                            item.Index = (int)datareader["id"];
                            item.Value = datareader["name"].ToString();
                            returnList.Add(item);
                            item = null;
                        }
                        HttpContext.Current.Session.Add(SessionManager.TaxRateSessionName, returnList);
                        return returnList;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error! Get all tax rate from DataBase", ex);
                }
            }
            List<Item> Allresults = HttpContext.Current.Session[SessionManager.TaxRateSessionName] as List<Item>;
            return Allresults;

        }
        public static Item GetTaxRate(string id)
        {
            List<Item> list = GetTaxRateList();
            foreach (Item item in list)
            {
                if (item.Index.ToString() == id)
                    return item;
            }
            return GetDefaultRef();
        }
        #endregion

        #region Trader
        public static List<Item> GetTraderList()
        {
            string session = SessionManager.TraderSessionName;
            if (HttpContext.Current.Session[session] == null || ((List<Item>)HttpContext.Current.Session[session]).Count == 0)
            {
                try
                {
                    using (var dbc = DataFactory.GetConnection())
                    {

                        IDbCommand command = CommandBuilder.CommandGetAll(dbc, "sp_GetAllTrader");

                        if (command.Connection.State != ConnectionState.Open)
                        {
                            command.Connection.Open();
                        }

                        IDataReader datareader = command.ExecuteReader();

                        List<Item> returnList = new List<Item>();
                        while (datareader.Read())
                        {
                            Item item = new Item();
                            item.Index = (int)datareader["id"];
                            item.Value = datareader["name"].ToString();
                            returnList.Add(item);
                            item = null;
                        }
                        HttpContext.Current.Session.Add(SessionManager.TraderSessionName, returnList);
                        return returnList;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error! Get all trader from DataBase", ex);
                }
            }
            List<Item> Allresults = HttpContext.Current.Session[SessionManager.TraderSessionName] as List<Item>;
            return Allresults;

        }
        public static List<Item> GetTransporterList()
        {
            string session = SessionManager.TransporterSessionName;
            if (HttpContext.Current.Session[session] == null || ((List<Item>)HttpContext.Current.Session[session]).Count == 0)
            {
                try
                {
                    using (var dbc = DataFactory.GetConnection())
                    {

                        IDbCommand command = CommandBuilder.CommandGetAll(dbc, "sp_GetAllTransporter");

                        if (command.Connection.State != ConnectionState.Open)
                        {
                            command.Connection.Open();
                        }

                        IDataReader datareader = command.ExecuteReader();

                        List<Item> returnList = new List<Item>();
                        while (datareader.Read())
                        {
                            Item item = new Item();
                            item.Index = (int)datareader["id"];
                            item.Value = datareader["name"].ToString();
                            returnList.Add(item);
                            item = null;
                        }
                        HttpContext.Current.Session.Add(SessionManager.TransporterSessionName, returnList);
                        return returnList;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error! Get all trader from DataBase", ex);
                }
            }
            List<Item> Allresults = HttpContext.Current.Session[SessionManager.TransporterSessionName] as List<Item>;
            return Allresults;

        }
        public static Item GetTrader(string id)
        {
            List<Item> list = GetTraderList();
            foreach (Item item in list)
            {
                if (item.Index.ToString() == id)
                    return item;
            }
            return GetDefaultRef();
        }
        #endregion

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
            string session = SessionManager.SupplierSessionName;
            if (HttpContext.Current.Session[session] == null || ((List<Item>)HttpContext.Current.Session[session]).Count == 0)
            {
                try
                {
                    using (var dbc = DataFactory.GetConnection())
                    {

                        IDbCommand command = CommandBuilder.CommandGetAll(dbc, "sp_GetAllSupplier");

                        if (command.Connection.State != ConnectionState.Open)
                        {
                            command.Connection.Open();
                        }

                        IDataReader datareader = command.ExecuteReader();

                        List<Item> returnList = new List<Item>();
                        while (datareader.Read())
                        {
                            Item item = new Item();
                            item.Index = (int)datareader["id"];
                            item.Value = datareader["name"].ToString();
                            returnList.Add(item);
                            item = null;
                        }
                        HttpContext.Current.Session.Add(SessionManager.SupplierSessionName, returnList);
                        return returnList;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error! Get all trader from DataBase", ex);
                }
            }
            List<Item> Allresults = HttpContext.Current.Session[SessionManager.SupplierSessionName] as List<Item>;
            return Allresults;
        }
        public static void SaveVesselList(List<Item> data)
        {
            string fileName = _dataPath + "/Lists/Vessel.xml";
            XMLUtil.WriteToXmlFile<List<Item>>(fileName, data);
        }

        public static Item GetDefaultRef()
        {
            return new Item() { Index = 0, Value = "" };
        }
        public static Reference GetDefaultReference()
        {
            return new Reference() { Id = Guid.Empty, Name = "" };
        }

        public static void SaveStoreList(List<Reference> data)
        {
            string fileName = _dataPath + "/Lists/Store.xml";
            XMLUtil.WriteToXmlFile<List<Reference>>(fileName, data);
        }
    }
}