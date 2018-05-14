using MZXRM.PSS.Connector.Database;
using MZXRM.PSS.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MZXRM.PSS.DataManager
{
    public class UserDataManager
    {
        public static User CalculateUser(User user)
        {
            return user;
        }

        #region " AuthenticateUser Function "
        public static User AuthenticateUser(string LoginName, string Password)
        {
            //USER OBJECT TO BE SAVED WHEN USER IS AUTHENTICATED
            User objUser = new User();
            using (var dbc = DataFactory.GetConnection())
            {
                IDbCommand command = CommandBuilder.CommandAuthenticateUser(dbc, LoginName, Password);

                if (command.Connection.State != ConnectionState.Open)
                {
                    command.Connection.Open();
                }
                var result = command.ExecuteReader();

                //ROLE RELATED INFORMATION TO BE SAVED IN "USER" OBJECTED DECLARE ABOVE
                Role role = null;

                //TEAM RELATED INFORMATION TO BE SAVED IN "USER" OBJECTED DECLARE ABOVE
                Team team = null;
                objUser.Roles = new List<Role>();
                objUser.Teams = new List<Team>();
                while (result.Read())
                {
                    if (String.IsNullOrEmpty(result["Message"].ToString()))
                    {
                        objUser.Id = Guid.Parse(result["UserId"].ToString());
                        objUser.Status = (UserStatus)Enum.Parse(typeof(UserStatus), result["UserStatus"].ToString());
                        objUser.Name = Convert.ToString(result["UserName"]);
                        objUser.Login = Convert.ToString(result["LoginName"]);
                        objUser.Designation = Convert.ToString(result["Designation"]);
                        objUser.Email = Convert.ToString(result["Email"]);
                        objUser.Mobile = Convert.ToString(result["Mobile"]);
                        objUser.Office = Convert.ToString(result["Office"]);
                        objUser.Home = Convert.ToString(result["Home"]);
                        objUser.Address = Convert.ToString(result["Address"]);
                        objUser.ProfileImage = Convert.ToString(result["Picture"]);
                        /* Role population goes here */
                        if (objUser.Roles.Count(x => x.Id == Guid.Parse(result["RoleId"].ToString())) <= 0)
                        {
                            role = new Role();
                            role.Id = Guid.Parse(result["RoleId"].ToString());
                            role.Name = result["RoleName"].ToString();
                            objUser.Roles.Add(role);
                            role = null;
                        }
                        /* Team population goes here */
                        if (objUser.Teams.Count(x => x.Id == Guid.Parse(result["TeamId"].ToString())) <= 0)
                        {
                            team = new Team();
                            team.Id = Guid.Parse(result["TeamId"].ToString());
                            team.Name = result["TeamName"].ToString();
                            objUser.Teams.Add(team);
                            team = null;
                        }
                    }
                    else
                    {
                        objUser.Remarks = result["Message"].ToString();
                    }
                }
            }
            return objUser;
        }


        #endregion


        #region DB Get all functions
        public static DataTable GetAllUsers()
        {
            try
            {
                using (var dbc = DataFactory.GetConnection())
                {

                    IDbCommand command = CommandBuilder.CommandGetAll(dbc, "sp_GetAllUser");

                    if (command.Connection.State != ConnectionState.Open)
                    {
                        command.Connection.Open();
                    }

                    IDataReader datareader = command.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(datareader);
                    return dt;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error! Get all User from DataBase", ex);
            }
        }
        public static DataTable GetAllRoles()
        {
            try
            {
                using (var dbc = DataFactory.GetConnection())
                {

                    IDbCommand command = CommandBuilder.CommandGetAll(dbc, "sp_GetAllUserRole");

                    if (command.Connection.State != ConnectionState.Open)
                    {
                        command.Connection.Open();
                    }

                    IDataReader datareader = command.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(datareader);
                    return dt;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error! Get all User from DataBase", ex);
            }
        }
        public static DataTable GetAllTeams()
        {
            try
            {
                using (var dbc = DataFactory.GetConnection())
                {

                    IDbCommand command = CommandBuilder.CommandGetAll(dbc, "sp_GetAllUserTeam");

                    if (command.Connection.State != ConnectionState.Open)
                    {
                        command.Connection.Open();
                    }

                    IDataReader datareader = command.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(datareader);
                    return dt;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error! Get all Team from DataBase", ex);
            }
        }
        #endregion
    }
}
