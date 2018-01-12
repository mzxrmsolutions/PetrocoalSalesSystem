using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MZXRM.PSS.Data
{
    public class Role
    {
        public Guid Id;
        public string Name;
        public Reference Ref()
        {
            return new Reference() {
                Id = Id,
                Name = Name
            };
        }
    }
}