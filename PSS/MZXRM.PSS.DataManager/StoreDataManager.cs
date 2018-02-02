using System;
using MZXRM.PSS.Data;

namespace MZXRM.PSS.DataManager
{
    public class StoreDataManager
    {
        public static Reference GetStoreRef(string id)
        {
            return new Reference() { Id = Guid.Empty, Name = "" };
        }

        public static Reference GetDefaultRef()
        {
            return new Reference() { Id = Guid.Empty, Name = "" };
        }
    }
}