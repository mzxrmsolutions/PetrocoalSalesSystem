using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PatrocoalSalesSystem.Models
{
    public class Reference
    {
        public Guid Id;
        public string Name;

        public static Reference GetNull()
        {
            return new Reference()
            {
                Id = Guid.Empty,
                Name = string.Empty
            };
        }
    }
    public class Item
    {
        public int Index;
        public string Value;
    }
}