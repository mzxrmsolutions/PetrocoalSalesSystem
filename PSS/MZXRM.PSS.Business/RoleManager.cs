using MZXRM.PSS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MZXRM.PSS.Business
{
    public class RoleManager
    {
        #region " IsInRole Function "
        public static Boolean IsInRole(String RoleName)
        {
            Boolean isAdmin = false;
            if (HttpContext.Current.Session["User"] != null)
            {
                User objUser = (User)HttpContext.Current.Session["User"];
                isAdmin = objUser.Roles.Count(x => x.Name.Equals(RoleName, StringComparison.InvariantCultureIgnoreCase)) > 0;
            }
            return isAdmin;
        } 
        #endregion
    }
}