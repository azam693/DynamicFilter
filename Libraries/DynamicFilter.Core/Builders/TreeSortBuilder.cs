using System.Linq.Expressions;
using DynamicFilter.Core.Common.Constants;
using DynamicFilter.Core.Common.Enums;
using DynamicFilter.Core.Common.Interfaces;
using DynamicFilter.Core.Models;

namespace DynamicFilter.Core.Builders
{
    /// <summary>
    /// Expression tree
    /// </summary>
    public class TreeSortBuilder : ISortBuilder
    {
        private static readonly Type _queryableType = typeof(Queryable);

        private Type _entityType;
        private Expression _currentExpression;

        public TreeSortBuilder(Type entityType, Expression currentExpression = null)
        {
            _entityType = entityType;
            _currentExpression = currentExpression;
        }

        public object Build(SortInfo sortInfo)
        {
            bool first = true;
            var currentExpression = GetCurrentExpression(); 
            foreach (var field in sortInfo.Fields)
            {
                var parameter = Expression.Parameter(_entityType, ReuquestConstant.ExpressionParameter);
                var member = Expression.Property(parameter, field.Key);
                var lambdaExpression = Expression.Lambda(member, parameter);

                string sortMethodName = GetSortMethod(first, field.Value);
                var argumentTypes = new Type[] { _entityType, member.Type };
                currentExpression = Expression.Call(
                    type: _queryableType, 
                    methodName: sortMethodName, 
                    typeArguments: argumentTypes, 
                    arguments: new Expression[] { currentExpression, Expression.Quote(lambdaExpression) });

                first = false;
            }

            return currentExpression;
        }

        private Expression GetCurrentExpression()
        {
            Expression currentExpression;
            if (_currentExpression == null)
            {
                var genericQueryablyType = typeof(IQueryable<>).MakeGenericType(_entityType);
                currentExpression = Expression.Parameter(genericQueryablyType, ReuquestConstant.ExpressionData);
            }
            else
            {
                currentExpression = _currentExpression;
            }

            return currentExpression;
        }

        private static string GetSortMethod(bool first, SortTypeEnum sortType)
        {
            return first
                ? (sortType == SortTypeEnum.Asc 
                    ? nameof(Queryable.OrderBy) 
                    : nameof(Queryable.OrderByDescending))
                : (sortType == SortTypeEnum.Asc 
                    ? nameof(Queryable.ThenBy) 
                    : nameof(Queryable.ThenByDescending));
        }
    }
}
