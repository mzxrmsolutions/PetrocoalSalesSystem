using MZXRM.PSS.Data;
using MZXRM.PSS.DataManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MZXRM.PSS.Business
{
   public class StoreManager
    {
        private static List<Store> _allStores;
        public static List<Store> AllStore
        {
            get
            {
                _allStores = StoreDataManager.ReadAllStore();
                return _allStores;
            }
            set
            {
                _allStores = value;
            }
        }
    }
}
