using DynamicFilter.Core.Common.Enums;
using DynamicFilter.Core.Helpers;
using DynamicFilter.Core.Models;
using DynamicFilter.DevExtreme.Common.Interfaces;
using System.Collections;
using System.Text.RegularExpressions;

namespace DynamicFilter.DevExtreme.Creators
{
    public class DxDataTableCriteriaCreator : IDataTableCriteriaCreator
    {
        private Dictionary<string, ComparisonEnum> _dxFilterComparionPairs = new Dictionary<string, ComparisonEnum>
        {
            { "=", ComparisonEnum.Equal },
            { "<>", ComparisonEnum.NotEqual },
            { ">", ComparisonEnum.GreaterThan },
            { ">=", ComparisonEnum.GreaterThanOrEqual },
            { "<", ComparisonEnum.LessThan },
            { "<=", ComparisonEnum.LessThanOrEqual },
            { "contains", ComparisonEnum.Contains },
            { "notcontains", ComparisonEnum.NotContains },
            { "startswith", ComparisonEnum.StartsWith },
            { "endswith", ComparisonEnum.EndsWith },
        };
        
        public CriteriaInfo Create(object criteria)
        {
            var dxFilter = (IList)criteria;
            if (IsGroup(dxFilter[0]))
                return CreateGroup(dxFilter);
            else if (IsUnary(dxFilter))
                return CreateUnary(dxFilter);
            else
                return CreateBinary(dxFilter);
        }

        private bool IsGroup(object item)
        {
            return item is IList && !(item is string);
        }

        private bool IsUnary(IList dxFilter)
        {
            return Convert.ToString(dxFilter[0]) == "!";
        }

        private CriteriaInfo CreateGroup(IList dxFilter)
        {
            var criterias = new List<CriteriaInfo>();
            bool isAnd = true;
            bool nextIsAnd = true;
            foreach (var item in dxFilter)
            {
                var dxOperand = item as IList;
                if (IsGroup(dxOperand))
                {
                    if (criterias.Count > 1 && isAnd != nextIsAnd)
                        throw new ArgumentException("Mixing of and/or is not allowed inside a single group");

                    isAnd = nextIsAnd;
                    criterias.Add(Create(dxOperand));
                    nextIsAnd = true;
                }
                else
                {
                    nextIsAnd = Regex.IsMatch(Convert.ToString(item), "and|&", RegexOptions.IgnoreCase);
                }
            }

            var currentOperator = isAnd ? ComparisonEnum.AND : ComparisonEnum.OR;
            var criteriaResult = new CriteriaInfo(currentOperator);
            criteriaResult.AddChild(criterias);

            return criteriaResult;
        }

        private CriteriaInfo CreateUnary(IList dxFilter)
        {
            var criteria = Create((IList)dxFilter[1]);

            return new CriteriaInfo(ComparisonEnum.NOT, new List<CriteriaInfo> { criteria });
        }

        private CriteriaInfo CreateBinary(IList dxFilter)
        {
            var hasExplicitOperation = dxFilter.Count > 2;
            var field = Convert.ToString(dxFilter[0]);
            var value = TreeBuilderHelper.UnwrapNewtonsoftValue(dxFilter[hasExplicitOperation ? 2 : 1]);
            var dxOperator = hasExplicitOperation 
                ? Convert.ToString(dxFilter[1]).ToLower() 
                : "=";
            var comparison = GetComparison(dxOperator);

            return new CriteriaInfo(field, comparison, value);
        }

        private ComparisonEnum GetComparison(string dxOperator)
        {
            ComparisonEnum comparison;

            return _dxFilterComparionPairs.TryGetValue(dxOperator, out comparison)
                ? comparison : ComparisonEnum.Equal;
        }
    }
}
