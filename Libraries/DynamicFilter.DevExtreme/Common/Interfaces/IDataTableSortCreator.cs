using DynamicFilter.Core.Models;

namespace DynamicFilter.DevExtreme.Common.Interfaces
{
    public interface IDataTableSortCreator
    {
        SortInfo Create(object sort);
    }
}
