using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MZXRM.PSS.Common
{
    public class ExceptionHandler
    {
        public static ExceptionType ExType;
        public static Exception Ex = null;
        public static void Error(string message)
        {
            Ex = new Exception(message);
            ExType = ExceptionType.Error;
        }
        public static void Error(string message, Exception ex)
        {
            Ex = new Exception(message, ex);
            ExType = ExceptionType.Error;
        }
        public static void Warning(string message)
        {
            Ex = new Exception(message);
            ExType = ExceptionType.Warning;
        }
        public static void CloseException()
        {
            Ex = null;
        }
        public static bool HasException()
        {
            if (Ex == null)
                return false;
            return true;
        }
        public static string GetExceptionMessage()
        {
           return Ex.Message;
        }
    }
  public  enum ExceptionType
    {
        Trace,
        Log,
        Info,
        Warning,
        Error
    }
}

    
