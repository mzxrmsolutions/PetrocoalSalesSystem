using MZXRM.PSS.Data;
using MZXRM.PSS.DataManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MZXRM.PSS.Business
{
    public class SaleUtil
    {
        private static List<SaleOrder> _AllSOs;
        public static List<SaleOrder> AllSOs
        {
            get
            {
                _AllSOs = DBUtil.ReadAllSO();
                return _AllSOs;
            }
            set
            {
                _AllSOs = value;
            }
        }

        public static SaleOrder CreateSO(Dictionary<string, string> values)
        {
            throw new NotImplementedException();
        }
    }
}