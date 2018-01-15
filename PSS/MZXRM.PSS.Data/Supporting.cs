using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MZXRM.PSS.Data
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
    public class POFilters
    {
        public FilterType poList;
        public DateRange poDate;
        public Guid Customer;
        public int Origin;
        public int Size;
        public int Vessel;
    }
    public enum FilterType
    {
        My,
        All,
        New,
        InProcess,
        Complete,
        Cancelled
    }
    public class DateRange
    {
        public DateTime Start;
        public DateTime End;
    }
}