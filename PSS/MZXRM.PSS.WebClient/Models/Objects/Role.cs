using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PatrocoalSalesSystem.Models
{
    public class Role
    {
        public Guid Id;
        public string Name;
        internal Reference Ref()
        {
            return new Reference() {
                Id = Id,
                Name = Name
            };
        }
    }
}