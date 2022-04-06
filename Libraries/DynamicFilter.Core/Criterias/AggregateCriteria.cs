using DynamicFilter.Core.Common.Enums;
using DynamicFilter.Core.Common.Exceptions;
using DynamicFilter.Core.Models;

namespace DynamicFilter.Core.Criterias
{
    public class AggregateCriteria
    {
        public CriteriaInfo And(BinaryCriteria binaryCriteria)
        {
            return new CriteriaInfo(
                ComparisonEnum.AND,
                binaryCriteria.GetCriteriaInfos());
        }

        public CriteriaInfo And(params CriteriaInfo[] binaryCriterias)
        {
            return new CriteriaInfo(
                ComparisonEnum.AND,
                binaryCriterias);
        }

        public CriteriaInfo And(IReadOnlyList<CriteriaInfo> criteriaInfos)
        {
            return new CriteriaInfo(
                ComparisonEnum.AND,
                criteriaInfos);
        }

        public CriteriaInfo Or(BinaryCriteria binaryCriteria)
        {
            return new CriteriaInfo(
                ComparisonEnum.OR,
                binaryCriteria.GetCriteriaInfos());
        }

        public CriteriaInfo Or(params CriteriaInfo[] criteriaInfos)
        {
            return new CriteriaInfo(
                ComparisonEnum.OR,
                criteriaInfos);
        }

        public CriteriaInfo Or(IReadOnlyList<CriteriaInfo> criteriaInfos)
        {
            return new CriteriaInfo(
                ComparisonEnum.OR,
                criteriaInfos);
        }

        public CriteriaInfo Not(BinaryCriteria binaryCriteria)
        {
            var criteriaInfos = binaryCriteria.GetCriteriaInfos();
            if (criteriaInfos.Count == 0)
                throw new DynamicFilterException("Binary criteria doesn't have any value");

            return new CriteriaInfo(
                ComparisonEnum.NOT,
                criteriaInfos.Count > 1
                    ? new[] { new CriteriaInfo(ComparisonEnum.AND, criteriaInfos) }
                    : criteriaInfos);
        }

        public CriteriaInfo Not(CriteriaInfo criteriaInfo)
        {
            return new CriteriaInfo(
                ComparisonEnum.NOT,
                new [] { criteriaInfo });
        }
    }
}
