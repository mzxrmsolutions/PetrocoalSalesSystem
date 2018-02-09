using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MZXRM.PSS.DataManager
{
    public class CommandBuilder
    {
        #region " CommandAuthenticateUser Function "
        public static IDbCommand CommandAuthenticateUser(IDbConnection dbc, string LoginName, string Password)
        {
            IDbCommand command = dbc.CreateCommand();
            command.CommandText = "sp_AuthenticateUser";
            command.CommandType = CommandType.StoredProcedure;

            var param = command.CreateParameter();
            param.ParameterName = "@LoginName";
            param.Value = LoginName;
            command.Parameters.Add(param);

            param = command.CreateParameter();
            param.ParameterName = "@Password";
            param.Value = Password;
            command.Parameters.Add(param);

            return command;
        }
        #endregion
        public static IDbCommand CommandGetAll(IDbConnection dbc, string DB_StoredProcedure)
        {
            IDbCommand command = dbc.CreateCommand();
            command.CommandText = DB_StoredProcedure;
            command.CommandType = CommandType.StoredProcedure;
            return command;
        }
        public static IDbCommand CommandInsert(IDbConnection dbc, string SPName, Dictionary<string, object> paramKeyValues)
        {
            IDbCommand command = dbc.CreateCommand();
            command.CommandText = SPName;
            command.CommandType = CommandType.StoredProcedure;

            foreach (KeyValuePair<string, object> item in paramKeyValues)
            {
                var param = command.CreateParameter();
                param.ParameterName = item.Key;
                param.Value = item.Value;
                command.Parameters.Add(param);
            }

            //var retParam = command.CreateParameter();
            //retParam.ParameterName = "@POID";
            //retParam.Direction = ParameterDirection.Output;
            //retParam.DbType = DbType.Guid;
            //command.Parameters.Add(retParam);

            return command;

            //cmd.Parameters.Add("@usertypeid", SqlDbType.TinyInt).Direction = ParameterDirection.Output;

            /*
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
            return command;*/
        }

    }
}
