using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MZXRM.PSS.Data
{
    public class Role
    {
        /* UPDATED BY KASHIF ABBAS */
        public Guid Id { get; set; }
        public string Name { get; set; }

        //TO ASK: Purpose of Reference class
        public Reference Ref()
        {
            return new Reference() {
                Id = Id,
                Name = Name
            };
        }
    }
}