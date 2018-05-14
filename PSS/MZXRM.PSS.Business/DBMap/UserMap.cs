using MZXRM.PSS.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MZXRM.PSS.Business.DBMap
{
 public   class UserMap
    {
        public static List<User> MapUserData(DataTable DTuser, DataTable DTrole, DataTable DTteam)
        {
            List<User> AllUsers = new List<User>();
            foreach (DataRow DRuser in DTuser.Rows)
            {
                User user = new User();

                user.Id = DRuser["Id"] != null ? new Guid(DRuser["Id"].ToString()) : Guid.Empty;
                user.Status = DRuser["Status"] != null ? MapUserStatus(DRuser["Status"].ToString()) : UserStatus.InActive;
                user.Name = Convert.ToString(DRuser["Name"]);
                user.Login = Convert.ToString(DRuser["LoginName"]);
                user.Designation = Convert.ToString(DRuser["Designation"]);
                user.Email = Convert.ToString(DRuser["Email"]);
                user.Mobile = Convert.ToString(DRuser["Mobile"]);
                user.Office = Convert.ToString(DRuser["Office"]);
                user.Home = Convert.ToString(DRuser["Home"]);
                user.Address = Convert.ToString(DRuser["Address"]);

                user.Roles = new List<Role>();
                foreach (DataRow DRrole in DTrole.Rows)
                {
                    Guid userid = DRrole["UserId"] != null ? new Guid(DRrole["UserId"].ToString()) : Guid.Empty;
                    if (userid != Guid.Empty && userid == user.Id)
                    {
                        Role userRole = new Role();
                        userRole.Id = DRrole["RoleId"] != null ? new Guid(DRrole["RoleId"].ToString()) : Guid.Empty;
                        userRole.Name = DRrole["Name"] != null ? DRrole["Name"].ToString() : "";
                        user.Roles.Add(userRole);
                    }
                }

                user.Teams = new List<Team>();
                foreach (DataRow DRteam in DTteam.Rows)
                {
                    Guid userid = DRteam["UserId"] != null ? new Guid(DRteam["UserId"].ToString()) : Guid.Empty;
                    if (userid != Guid.Empty && userid == user.Id)
                    {
                        Team userTeam = new Team();
                        userTeam.Id = DRteam["TeamId"] != null ? new Guid(DRteam["TeamId"].ToString()) : Guid.Empty;
                        userTeam.Name = DRteam["Name"] != null ? DRteam["Name"].ToString() : "";
                        user.Teams.Add(userTeam);
                    }
                }
                AllUsers.Add(user);
            }
            return AllUsers;
        }

        public static UserStatus MapUserStatus(string status)
        {
            switch (status)
            {
                case "1":
                    return UserStatus.Active;
                default:
                    return UserStatus.InActive;
            }
        }

    }
}
