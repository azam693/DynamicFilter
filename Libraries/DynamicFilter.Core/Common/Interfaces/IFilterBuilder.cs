using DynamicFilter.Core.Models;

namespace DynamicFilter.Core.Common.Interfaces;

/// <summary>
/// Filter condition for request query
/// </summary>
public interface IFilterBuilder
{
    object Build(CriteriaInfo criteriaInfo);
}
