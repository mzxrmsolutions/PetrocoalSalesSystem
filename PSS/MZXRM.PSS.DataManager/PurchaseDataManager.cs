using MZXRM.PSS.Connector.Database;
using MZXRM.PSS.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MZXRM.PSS.DataManager
{
   public static class PurchaseDataManager
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
        public static bool SavePO(PurchaseOrder PO)
        {
            string poPath = _dataPath + "/PO";
            string fileName = poPath + "/" + PO.PONumber + ".xml";
            XMLUtil.WriteToXmlFile<PurchaseOrder>(fileName, PO);
            HttpContext.Current.Session.Remove("DBPurchaseOrders");
            ReadAllPO();
            return true;
        }

        //test purpose createPO
        public static bool CreatePO(PurchaseOrder PO)
        {

            using (var dbc = DataFactory.GetConnection())
            {

                IDbCommand command = CommandInsert(dbc,1, PO.CreatedOn, PO.CreatedBy.Id, PO.ModifiedOn, PO.ModifiedBy.Id, PO.CreatedOn, PO.Lead.Id, PO.CreatedOn, PO.CreatedBy.Id, PO.PONumber, PO.PODate, PO.Origin.Index, PO.Size.Index, PO.Vessel.Index, PO.TargetDays, Guid.NewGuid(), PO.TermsOfPayment, Convert.ToDecimal("10"),Convert.ToDecimal("10"));

                if (command.Connection.State != ConnectionState.Open)
                {
                    command.Connection.Open();
                }

                command.ExecuteNonQuery();
                return true;
            }
            //string poPath = _dataPath + "/PO";
            //string fileName = poPath + "/" + PO.PONumber + ".xml";
            //XMLUtil.WriteToXmlFile<PurchaseOrder>(fileName, PO);
            //HttpContext.Current.Session.Remove("DBPurchaseOrders");
            //ReadAllPO();
            //return true;
        }

        private static IDbCommand CommandInsert(IDbConnection dbc, int Status, DateTime CreatedOn, Guid CreatedBy, DateTime ModifiedOn, Guid ModifiedBy, DateTime CompletedOn, Guid LeadId, DateTime ApprovedDate, 
            Guid ApprovedBy, String PONumber, DateTime PODate, int origin, int size, int vessel, int targetDays, Guid supplier, String termsOfpayment, Decimal bufferQuantityMax, Decimal bufferQuantityMin)
        {
            IDbCommand command = dbc.CreateCommand();
            command.CommandText = "sp_InsertPO";
            command.CommandType = CommandType.StoredProcedure;

            var param = command.CreateParameter();
            param.ParameterName = "@Status";
            param.Value = Status;
            command.Parameters.Add(param);
            
            param = command.CreateParameter();
            param.ParameterName = "@CreatedOn";
            param.Value = CreatedOn;
            command.Parameters.Add(param);

            param = command.CreateParameter();
            param.ParameterName = "@CreatedBy";
            param.Value = CreatedBy;
            command.Parameters.Add(param);

            param = command.CreateParameter();
            param.ParameterName = "@ModifiedOn";
            param.Value = ModifiedOn;
            command.Parameters.Add(param);

            param = command.CreateParameter();
            param.ParameterName = "@ModifedBy";
            param.Value = ModifiedBy;
            command.Parameters.Add(param);

            param = command.CreateParameter();
            param.ParameterName = "@CompletedOn";
            param.Value = CompletedOn;
            command.Parameters.Add(param);
             
            param = command.CreateParameter();
            param.ParameterName = "@LeadId";
            param.Value = LeadId;
            command.Parameters.Add(param);

            param = command.CreateParameter();
            param.ParameterName = "@ApprovedDate";
            param.Value = ApprovedDate;
            command.Parameters.Add(param);

            param = command.CreateParameter();
            param.ParameterName = "@ApprovedBy";
            param.Value = ApprovedBy;
            command.Parameters.Add(param);
                
            param = command.CreateParameter();
            param.ParameterName = "@PONumber";
            param.Value = PONumber;
            command.Parameters.Add(param);

            param = command.CreateParameter();
            param.ParameterName = "@PODate";
            param.Value = PODate;
            command.Parameters.Add(param);

            param = command.CreateParameter();
            param.ParameterName = "@Origin";
            param.Value = origin;
            command.Parameters.Add(param);

            param = command.CreateParameter();
            param.ParameterName = "@Size";
            param.Value = size;
            command.Parameters.Add(param);
                 
            param = command.CreateParameter();
            param.ParameterName = "@Vessel";
            param.Value = vessel;
            command.Parameters.Add(param);

            param = command.CreateParameter();
            param.ParameterName = "@TargetDays";
            param.Value = targetDays;
            command.Parameters.Add(param);

            param = command.CreateParameter();
            param.ParameterName = "@Supplier";
            param.Value = supplier;
            command.Parameters.Add(param);

            param = command.CreateParameter();
            param.ParameterName = "@TermsOfPayment";
            param.Value = termsOfpayment;
            command.Parameters.Add(param);

            param = command.CreateParameter();
            param.ParameterName = "@BufferQuantityMax";
            param.Value = bufferQuantityMax;
            command.Parameters.Add(param);

            param = command.CreateParameter();
            param.ParameterName = "@BufferQuantityMin";
            param.Value = bufferQuantityMin;
            command.Parameters.Add(param);
            return command;
        }

    }
}
