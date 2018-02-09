using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MZXRM.PSS.Data
{
    /* UPDATED BY KASHIF ABBAS */
    //[Serializable]
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

        public string Designation;
        public string Email;
        public string Mobile;
        public string Office;
        public string Home;
        public string Address;
        public string ProfileImage;

        public List<Role> Roles;
        public List<Team> Teams;
        public string Remarks;

    }
    public enum UserStatus
    {
        InActive = 0,
        Active = 1
    }
}