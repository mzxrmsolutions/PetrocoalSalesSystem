using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MZXRM.PSS.Connector.Database
{
    public class DataFactory
    {
        #region " GetConnString Function "
        public static string GetConnString()
        {
            string ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["PSSConnectionString"].ToString();
            return ConnectionString;
        }
        #endregion

        #region " GetConnection Function "
        public static SqlConnection GetConnection()
        {
            SqlConnection Con = new SqlConnection();
            Con.ConnectionString = GetConnString();
            return Con;
        }
        #endregion

        #region " CloseConnection Function "
        public static void CloseConnection(SqlConnection Con)
        {
            if ((Con != null))
            {
                Con.Close();
                Con = null;
            }
        } 
        #endregion
    }
}