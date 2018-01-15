using MZXRM.PSS.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MZXRM.PSS.DataManager
{
   public class UserDataManager
    {
        static string _dataPath = ConfigurationManager.AppSettings["DataPath"];
        public static List<User> GetAllUsers()
        {
            string filePath = _dataPath + "/User";
            List<User> users = new List<User>();

            if (Directory.Exists(filePath))
            {
                string[] files = Directory.GetFiles(filePath);
                foreach (string file in files)
                {
                    User user = XMLUtil.ReadFromXmlFile<User>(file);
                    users.Add(user);
                }
            }
            else
                Directory.CreateDirectory(filePath);
            return users;
        }
        public static bool SaveUser(User user)
        {
            string poPath = _dataPath + "/User";
            string fileName = poPath + "/" + user.Name + ".xml";
            XMLUtil.WriteToXmlFile<User>(fileName, user);
            return true;
        }
    }
}
