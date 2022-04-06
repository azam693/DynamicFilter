using DynamicFilter.Core.Common.Enums;
using DynamicFilter.Core.Models;

namespace DynamicFilter.Core.Criterias
{
    public class BinaryCriteria
    {
        private List<CriteriaInfo> _criteriaInfos;

        public BinaryCriteria()
        {
            _criteriaInfos = new List<CriteriaInfo>();
        }

        public BinaryCriteria Equal(string name, object value)
        {
            _criteriaInfos.Add(new CriteriaInfo(
                name: name,
                comparison: ComparisonEnum.Equal,
                value: value));

            return this;
        }

        public BinaryCriteria NotEqual(string name, object value)
        {
            _criteriaInfos.Add(new CriteriaInfo(
                name: name,
                comparison: ComparisonEnum.NotEqual,
                value: value));

            return this;
        }

        public BinaryCriteria Contains(string name, object value)
        {
            _criteriaInfos.Add(new CriteriaInfo(
                name: name,
                comparison: ComparisonEnum.Contains,
                value: value));

            return this;
        }

        public BinaryCriteria NotContains(string name, object value)
        {
            _criteriaInfos.Add(new CriteriaInfo(
                name: name,
                comparison: ComparisonEnum.NotContains,
                value: value));

            return this;
        }

        public BinaryCriteria StartsWith(string name, object value)
        {
            _criteriaInfos.Add(new CriteriaInfo(
                name: name,
                comparison: ComparisonEnum.StartsWith,
                value: value));

            return this;
        }

        public BinaryCriteria EndsWith(string name, object value)
        {
            _criteriaInfos.Add(new CriteriaInfo(
                name: name,
                comparison: ComparisonEnum.EndsWith,
                value: value));

            return this;
        }

        public BinaryCriteria In(string name, IReadOnlyList<object> values)
        {
            _criteriaInfos.Add(new CriteriaInfo(
                name: name,
                values: values));

            return this;
        }

        public BinaryCriteria GreaterThan(string name, object value)
        {
            _criteriaInfos.Add(new CriteriaInfo(
                name: name,
                comparison: ComparisonEnum.GreaterThan,
                value: value));

            return this;
        }

        public BinaryCriteria GreaterThanOrEqual(string name, object value)
        {
            _criteriaInfos.Add(new CriteriaInfo(
                name: name,
                comparison: ComparisonEnum.GreaterThanOrEqual,
                value: value));

            return this;
        }

        public BinaryCriteria LessThan(string name, object value)
        {
            _criteriaInfos.Add(new CriteriaInfo(
                name: name,
                comparison: ComparisonEnum.LessThan,
                value: value));

            return this;
        }

        public BinaryCriteria LessThanOrEqual(string name, object value)
        {
            _criteriaInfos.Add(new CriteriaInfo(
                name: name,
                comparison: ComparisonEnum.LessThanOrEqual,
                value: value));

            return this;
        }

        public BinaryCriteria IsNull(string name)
        {
            _criteriaInfos.Add(new CriteriaInfo(
                name: name,
                comparison: ComparisonEnum.IsNull,
                value: null));

            return this;
        }

        public BinaryCriteria IsNotNull(string name)
        {
            _criteriaInfos.Add(new CriteriaInfo(
                name: name,
                comparison: ComparisonEnum.IsNotNull,
                value: null));

            return this;
        }

        public IReadOnlyList<CriteriaInfo> GetCriteriaInfos()
        {
            return _criteriaInfos;
        }
    }
}
