using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MZXRM.PSS.Data
{
    public class User
    {
        public Guid Id;
        public UserStatus Status;
        public DateTime CreatedOn;
        public Reference CreatedBy;
        public DateTime ModifiedOn;
        public Reference ModifiedBy;

        public string Name;
        public string Login;
        public string Password;
        public Reference Role;
        public string Designation;
        public string ProfileImage;
    }
    public enum UserStatus
    {
        Active,
        InActive
    }
}