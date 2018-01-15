using MZXRM.PSS.Data;
using MZXRM.PSS.DataManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MZXRM.PSS.Business
{
    public class PurchaseManager
    {
        public List<PurchaseOrder> GetPOs(POFilters filter)
        {
            Guid userId = Common.CurrentUser.Id;
            List<PurchaseOrder> allData = PurchaseDataManager.ReadAllPO();
            List<PurchaseOrder> filteredData = new List<PurchaseOrder>();
            if (filter == null)
                return filteredData;
            foreach (PurchaseOrder po in allData)
            {
                bool include = false;
                switch (filter.poList)
                {
                    default:
                    case FilterType.My:
                        if (po.Lead.Id == userId)
                            include = true;
                        break;
                    case FilterType.All:
                        include = true;
                        break;
                    case FilterType.InProcess:
                        if (po.Status == POStatus.InProcess)
                            include = true;
                        break;
                    case FilterType.Complete:
                        if (po.Status == POStatus.Completed)
                            include = true;
                        break;
                    case FilterType.Cancelled:
                        if (po.Status == POStatus.Cancelled)
                            include = true;
                        break;
                    case FilterType.New:
                        if (po.Status == POStatus.Created || po.Status == POStatus.PendingApproval)
                            include = true;
                        break;
                }
                if (include && filter.Origin != 0)
                {
                    if (po.Origin.Index != filter.Origin)
                        include = false;
                }
                if (include && filter.Size != 0)
                {
                    if (po.Size.Index != filter.Size)
                        include = false;
                }
                if (include && filter.Vessel != 0)
                {
                    if (po.Vessel.Index != filter.Vessel)
                        include = false;
                }
                if (include)
                    filteredData.Add(po);
            }
            return filteredData;
        }
        /*public PurchaseOrder GetPO(string ponumber)
        {
            PurchaseOrder po= DBUtil.ReadPO(ponumber);
            
            return po;
        }*/

    }
    public enum POView
    {
        My,
        All,
        InProcess,
        Completed,
        Closed,
    }

}
