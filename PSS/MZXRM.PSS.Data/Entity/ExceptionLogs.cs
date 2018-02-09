using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MZXRM.PSS.Data
{
    public class ExceptionLogs
    {

        public long Id { get; set; }
        public DateTime ExceptionDate { get; set; }
        public string ExceptionMessage { get; set; }
        public string ExceptionType { get; set; }
        public string ExceptionSource { get; set; }
        public string ExceptionURL { get; set; }
        public string ExceptionSystem { get; set; }

    }
}
