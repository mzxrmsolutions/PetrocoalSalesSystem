using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MZXRM.PSS.Common
{
    public static class ExceptionHandler
    {
        public static void Error(string Message)
        {
            throw new Exception(Message);
        }
    }
}