using DynamicFilter.Core.Common.Enums;
using DynamicFilter.Core.Criterias;
using DynamicFilter.Core.Models;

namespace DynamicFilter.Core.Builders
{
    public class CriteriaBuilder
    {
        public static AggregateCriteria Aggregate => new AggregateCriteria();
        public static BinaryCriteria Binary => new BinaryCriteria();
    }
}
