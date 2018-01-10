using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PatrocoalSalesSystem.Models
{
    public static class ExceptionHandler
    {
        public static void Error(string Message)
        {
            throw new Exception(Message);
        }
    }
}