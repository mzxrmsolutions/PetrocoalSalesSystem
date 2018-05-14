using MZXRM.PSS.Business.DBMap;
using MZXRM.PSS.Common;
using MZXRM.PSS.Data;
using MZXRM.PSS.DataManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace MZXRM.PSS.Business
{
    public class UserManager
    {
        private static List<User> _allUsers;
        public static List<User> Users
        {
            get
            {
                if (HttpContext.Current.Session[SessionManager.UserSession] != null)
                    _allUsers = HttpContext.Current.Session[SessionManager.UserSession] as List<User>;
                if (_allUsers == null || _allUsers.Count == 0)
                    _allUsers = ReadAllUsers();
                return _allUsers;
            }
        }
        public static List<User> ReadAllUsers()
        {
            DataTable DTuser = UserDataManager.GetAllUsers();
            DataTable DTrole = UserDataManager.GetAllRoles();
            DataTable DTteam = UserDataManager.GetAllTeams();
            List<User> allUsers = UserMap.MapUserData(DTuser, DTrole, DTteam);
            HttpContext.Current.Session.Add(SessionManager.UserSession, allUsers);
            _allUsers = allUsers;
            return _allUsers;

            //string filePath = _dataPath + "/User";
            //List<User> users = new List<User>();

            //if (Directory.Exists(filePath))
            //{
            //    string[] files = Directory.GetFiles(filePath);
            //    foreach (string file in files)
            //    {
            //        User user = XMLUtil.ReadFromXmlFile<User>(file);
            //        users.Add(user);
            //    }
            //}
            //else
            //    Directory.CreateDirectory(filePath);
            //return users;
        }
        public static User GetUser(Guid userId)
        {
            List<User> allUsers = ReadAllUsers();
            foreach (User user in allUsers)
            {
                if (user.Id == userId)
                    return user;
            }
            return null;
        }
        public static Reference GetDefaultRef()
        {
            return new Reference() { Id = Guid.Empty, Name = "" };
        }

        public static Reference GetUserRef(string id)
        {
            if (string.IsNullOrEmpty(id))
                return GetDefaultRef();
            Guid userId = new Guid(id);
            User user = GetUser(userId);
            if (user != null)
                return new Reference() { Id = user.Id, Name = user.Name };
            return GetDefaultRef();
        }
        internal static Reference GetCurrentUserRef()
        {
            throw new NotImplementedException();
        }

        #region " AuthenticateUser Function "
        public static User AuthenticateUser(String LoginName, String Password)
        {
            User objUser = UserDataManager.AuthenticateUser(LoginName, Password);
            HttpContext.Current.Session.Add("User", objUser);
            if (String.IsNullOrEmpty(objUser.Remarks))
            {
                Common.CurrentUser = objUser;
            }
            else
            {
                Common.CurrentUser = null;
            }
            return objUser;
        }
        #endregion
        public static Reference FindRole(string roleName)
        {
            foreach (Role role in Common.AllRole)
                if (role.Name.ToLower() == roleName.ToLower())
                    return role.Ref();
            return Reference.GetNull();
        }

        public static void Login(string login, string password)
        {
            foreach (User user in Users)
            {
                if (user.Login.ToLower() == login.ToLower() && user.Password == password)
                {
                    Common.CurrentUser = user;
                    return;
                }
            }
            Common.CurrentUser = null;
        }

        public static User GetUser(string id)
        {
            foreach (User user in Users)
            {
                if (user.Id == new Guid(id))
                    return user;
            }
            ExceptionHandler.Error("User Not Found");
            return null;
        }



    }
}