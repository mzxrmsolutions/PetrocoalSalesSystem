using MZXRM.PSS.Common;
using MZXRM.PSS.Data;
using MZXRM.PSS.DataManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MZXRM.PSS.Business
{
    public class UserUtil
    {
        private static List<User> _allUsers;
        public static List<User> Users
        {
            get
            {
                if (_allUsers == null || _allUsers.Count == 0)
                    _allUsers = UserDataManager.GetAllUsers();
                return _allUsers;
            }
            set
            {
                _allUsers = value;
            }
        }
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

        public static Reference GetUserRef(string id)
        {
            foreach (User user in Users)
            {
                if (user.Id == new Guid(id))
                    return new Reference() { Id = user.Id, Name = user.Name };
            }
            ExceptionHandler.Error("User Not Found");
            return null;
        }
    }
}