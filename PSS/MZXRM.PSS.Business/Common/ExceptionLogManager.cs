using MZXRM.PSS.Data;
using MZXRM.PSS.DataManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MZXRM.PSS.Business
{
    public class ExceptionLogManager
    {
        public static void Log(Exception ex, string url = null, string systemName = null)
        {
            var log = new ExceptionLogs
            {
                ExceptionMessage = ex.Message,
                ExceptionType = ex.GetType().Name,
                ExceptionURL = url,
                ExceptionSource = ex.StackTrace,
                ExceptionSystem = systemName
            };
            ExceptionLogsDataManager.Insert(log);
        }
    }
}
