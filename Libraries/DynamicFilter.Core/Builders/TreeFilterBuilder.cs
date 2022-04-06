using DynamicFilter.Core.Helpers;
using System.Linq.Expressions;
using System.Reflection;
using DynamicFilter.Core.Common.Constants;
using DynamicFilter.Core.Common.Enums;
using DynamicFilter.Core.Common.Exceptions;
using DynamicFilter.Core.Common.Interfaces;
using DynamicFilter.Core.Models;
using DynamicFilter.Core.Criterias;

namespace DynamicFilter.Core.Builders
{
    /// <summary>
    /// Expresion tree
    /// </summary>
    public class TreeFilterBuilder : IFilterBuilder
    {
        private Type _entityType;
        private readonly MethodInfo _stringContains = typeof(string).GetMethod(nameof(string.Contains), new[] { typeof(string) });
        private readonly MethodInfo _stringStartsWith = typeof(string).GetMethod(nameof(String.StartsWith), new[] { typeof(string) });
        private readonly MethodInfo _stringEndsWith = typeof(string).GetMethod(nameof(String.EndsWith), new[] { typeof(string) });
        private readonly Dictionary<ComparisonEnum, ExpressionType> _binaryAnalog = new Dictionary<ComparisonEnum, ExpressionType>()
        {
            {ComparisonEnum.Equal, ExpressionType.Equal},
            {ComparisonEnum.NotEqual, ExpressionType.NotEqual},
            {ComparisonEnum.LessThan, ExpressionType.LessThan},
            {ComparisonEnum.LessThanOrEqual, ExpressionType.LessThanOrEqual},
            {ComparisonEnum.GreaterThan, ExpressionType.GreaterThan},
            {ComparisonEnum.GreaterThanOrEqual, ExpressionType.GreaterThanOrEqual}
        };

        public TreeFilterBuilder(Type entityType)
        {
            _entityType = entityType;
        }

        public object Build(BinaryCriteria binaryCriteria)
        {
            var criteriaInfos = binaryCriteria.GetCriteriaInfos();
            if (criteriaInfos.Count == 0)
                throw new DynamicFilterException("Binary criteria doesn't have any value");

            return criteriaInfos.Count == 1
                ? Build(criteriaInfos.First())
                : Build(new CriteriaInfo(ComparisonEnum.AND, criteriaInfos));
        }
        
        public object Build(CriteriaInfo criteria)
        {
            var parameter = Expression.Parameter(_entityType, ReuquestConstant.ExpressionParameter);
            var expression = GenerateBody(criteria, _entityType, parameter);

            return expression != null
                ? Expression.Lambda(expression, parameter) 
                : null;
        }

        // Top level operations
        private Expression GenerateBody(CriteriaInfo criteria, Type entityType, ParameterExpression parameter)
        {
            Expression expression = null;
            if (criteria.Comparison == ComparisonEnum.OR)
            {
                var expressions = criteria.Criterias
                    .Select(childCriteria => GenerateBodySimple(childCriteria, entityType, parameter))
                    .Where(expr => expr != null);
                expression = expressions.Aggregate((expr, eq) => Expression.Or(expr, eq));
            }
            else if (criteria.Comparison == ComparisonEnum.AND)
            {
                var expressions = criteria.Criterias
                    .Select(childCriteria => GenerateBodySimple(childCriteria, entityType, parameter))
                    .Where(expr => expr != null);
                expression = expressions.Aggregate((expr, eq) => Expression.And(expr, eq));
            }
            else if (criteria.Comparison == ComparisonEnum.NOT)
            {
                if (criteria.Criterias.Count == 0)
                    throw new DynamicFilterException("Operator not can't contains null expression");
                if (criteria.Criterias.Count > 1)
                    throw new DynamicFilterException("Operator not can't contains more than 1 expression");

                var childCriteria = criteria.Criterias[0];
                expression = Expression.Not(GenerateBodySimple(childCriteria, entityType, parameter));
            }
            else
            {
                expression = GenerateBodySimple(criteria, entityType, parameter);
            }

            return expression;
        }

        private Type[] GetQueryableGenericArguments(Expression expression)
        {
            const string queryable1 = "IQueryable`1";
            var type = expression.Type;

            if (type.IsInterface && type.Name == queryable1)
                return type.GenericTypeArguments;

            return type.GetInterface(queryable1).GenericTypeArguments;
        }

        // Simple operations
        private Expression GenerateBodySimple(CriteriaInfo criteria, Type entityType, ParameterExpression parameter)
        {
            if (criteria.Comparison == ComparisonEnum.AND
                || criteria.Comparison == ComparisonEnum.OR
                || criteria.Comparison == ComparisonEnum.NOT)
                return GenerateBody(criteria, entityType, parameter);

            Expression expression = null;
            var member = Expression.Property(parameter, criteria.Name);
            var memberType = member.Type;

            object newValue = criteria.Value;
            TreeBuilderHelper.TryConvertValue(ref newValue, memberType);

            if (_binaryAnalog.ContainsKey(criteria.Comparison))
            {
                var constant = Expression.Constant(newValue, memberType);
                expression = BinaryExpression.MakeBinary(_binaryAnalog[criteria.Comparison], member, constant);
            }
            else if (criteria.Comparison == ComparisonEnum.IN)
            {
                var newValues = ConvertValues(criteria.Values, memberType);
                var listExpression = newValues.Select(dt =>
                    Expression.Equal(member, Expression.Constant(dt, memberType)));
                expression = listExpression.Aggregate<Expression>((expr, eq) => Expression.Or(expr, eq));
            }
            else if (criteria.Comparison == ComparisonEnum.IsNull)
            {
                expression = Expression.Equal(member, Expression.Constant(null));
            }
            else if (criteria.Comparison == ComparisonEnum.IsNotNull)
            {
                expression = Expression.NotEqual(member, Expression.Constant(null));
            }
            else if (criteria.Comparison == ComparisonEnum.Contains)
            {
                var constant = Expression.Constant(newValue, memberType);
                expression = Expression.Call(member, _stringContains, constant);
            }
            else if (criteria.Comparison == ComparisonEnum.NotContains)
            {
                var constant = Expression.Constant(newValue, memberType);
                var methodExpression = Expression.Call(member, _stringContains, constant);
                expression = Expression.Not(methodExpression);
            }
            else if (criteria.Comparison == ComparisonEnum.StartsWith)
            {
                var constant = Expression.Constant(newValue, memberType);
                expression = Expression.Call(member, _stringStartsWith, constant);
            }
            else if (criteria.Comparison == ComparisonEnum.EndsWith)
            {
                var constant = Expression.Constant(newValue, memberType);
                expression = Expression.Call(member, _stringEndsWith, constant);
            }

            return expression;
        }

        private List<object> ConvertValues(IReadOnlyList<object> values, Type memberType)
        {
            var newValues = new List<object>();
            for (int i = 0; i < values.Count; i++)
            {
                var newValue = values[i];
                TreeBuilderHelper.TryConvertValue(ref newValue, memberType);
                newValues.Add(newValue);
            }

            return newValues;
        }
    }
}
