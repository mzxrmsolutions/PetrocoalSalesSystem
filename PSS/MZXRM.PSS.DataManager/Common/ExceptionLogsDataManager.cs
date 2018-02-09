using MZXRM.PSS.Connector.Database;
using MZXRM.PSS.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MZXRM.PSS.DataManager
{
    public class ExceptionLogsDataManager
    {
        #region " Insert Function "
        public static void Insert(ExceptionLogs item)
        {
            using (var dbc = DataFactory.GetConnection())
            {
                IDbCommand command = CommandInsert(dbc, item);
                if (command.Connection.State != ConnectionState.Open)
                {
                    command.Connection.Open();
                }
                command.ExecuteNonQuery();
            }
        }
        #endregion

        #region " Private methods "

        #region " CommandInsert Function "
        private static IDbCommand CommandInsert(IDbConnection dbc, ExceptionLogs item)
        {
            IDbCommand command = dbc.CreateCommand();
            command.CommandText = "Insert_Exception";
            command.CommandType = CommandType.StoredProcedure;

            var param = command.CreateParameter();

            param.ParameterName = "@ExceptionMessage";
            param.Value = item.ExceptionMessage;
            command.Parameters.Add(param);

            param = command.CreateParameter();
            param.ParameterName = "@ExceptionSource";
            param.Value = item.ExceptionSource;
            command.Parameters.Add(param);

            param = command.CreateParameter();
            param.ParameterName = "@ExceptionSystem";
            param.Value = item.ExceptionSystem;
            command.Parameters.Add(param);

            param = command.CreateParameter();
            param.ParameterName = "@ExceptionType";
            param.Value = item.ExceptionType;
            command.Parameters.Add(param);

            param = command.CreateParameter();
            param.ParameterName = "@ExceptionURL";
            param.Value = item.ExceptionURL;
            command.Parameters.Add(param);
            return command;
        }
        #endregion

        #region " CommandGet Function "
        private static IDbCommand CommandGet(IDbConnection dbc)
        {
            IDbCommand command = dbc.CreateCommand();
            command.CommandText = "Select_AllExceptions";
            command.CommandType = CommandType.StoredProcedure;
            return command;
        } 
        #endregion

        #endregion
    }
}

