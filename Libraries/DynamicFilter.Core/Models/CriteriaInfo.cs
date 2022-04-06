using DynamicFilter.Core.Common.Enums;
using DynamicFilter.Core.Common.Extensions;

namespace DynamicFilter.Core.Models
{
    public class CriteriaInfo
    {
        private IReadOnlyList<CriteriaInfo> _criterias = new List<CriteriaInfo>();

        public string Name { get; protected set; }
        public object Value { get; protected set; }
        public IReadOnlyList<object> Values { get; protected set; }
        public ComparisonEnum Comparison { get; protected set; }
        public IReadOnlyList<CriteriaInfo> Criterias => _criterias;

        public CriteriaInfo(
            ComparisonEnum comparison,
            IReadOnlyList<CriteriaInfo> criterias = null)
        {
            Comparison = comparison;
            if (criterias != null)
                _criterias = criterias;
        }

        public CriteriaInfo(
            string name, 
            ComparisonEnum comparison, 
            object value,
            IReadOnlyList<CriteriaInfo> criterias = null)
        {
            if (name.IsNothing())
                throw new ArgumentNullException("name");

            Name = name;
            Comparison = comparison;
            Value = value;
            if (criterias != null)
                _criterias = criterias;
        }
        
        public CriteriaInfo(
            string name, 
            IReadOnlyList<object> values, 
            List<CriteriaInfo> criterias = null)
        {
            if (name.IsNothing())
                throw new ArgumentNullException("name");

            Name = name;
            Comparison = ComparisonEnum.IN;
            Values = values;
            if (criterias != null)
                _criterias = criterias;
        }

        public void AddChild(IReadOnlyList<CriteriaInfo> criterias)
        {
            if (criterias != null)
                _criterias = criterias;
        }
    }
}
